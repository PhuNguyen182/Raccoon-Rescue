using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.Gameplay.GameEntities;
using BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls;
using BubbleShooter.Scripts.Gameplay.GameDatas;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Factories;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.Common.Constants;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameHandlers
{
    public class BallShooter : MonoBehaviour
    {
        [SerializeField] private CommonBall prefab;
        [SerializeField] private BallProvider ballProvider;

        [Header("Dummy Balls")]
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall blue;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall green;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall orange;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall red;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall violet;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall yellow;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall colorful;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall fireball;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall leafBall;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall sunBall;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall waterBall;

        [Space(10)]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform ballContainer;

        [Space(10)]
        [SerializeField] private LineDrawer lineDrawer;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private MainCharacter mainCharacter;
        
        [Space(10)]
        [Header("Start Position Setting")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Vector2 normalizePosition;

        private bool _canFire = true;
        private float _limitAngleSine = 0;

        private Vector2 _direction;
        private Vector3 _startPosition;
        private BallShootModel _ballModel;

        private CancellationToken _token;
        private EntityFactory _entityFactory;

        private IPublisher<DecreaseMoveMessage> _decreaseMovePublisher;

        public Action OnOutOfMove;
        public DummyBall DummyBall;
        public BallShootModel BallModel => _ballModel;
        public Transform ShotPoint => spawnPoint;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        private void Start()
        {
            _decreaseMovePublisher = GlobalMessagePipe.GetPublisher<DecreaseMoveMessage>();
        }

        private void Update()
        {
            GetInputDirection();
            RotatePointer();

            if (inputHandler.IsReleased && _limitAngleSine > 0.15f)
            {
                ShootBall(_ballModel);
            }
        }

        public void SetBallFactory(EntityFactory factory)
        {
            _entityFactory = factory;
        }

        public void SetStartPosition()
        {
            _startPosition = mainCamera.ViewportToWorldPoint(normalizePosition);
            _startPosition.z = 0;
            transform.position = _startPosition;
        }

        public void SetColorModel(BallShootModel model, bool isActive)
        {
            _ballModel = model;
            SetBallColor(isActive, _ballModel.BallColor);
        }

        public void SetBallColor(bool isActive, EntityType color)
        {
            if (DummyBall != null)
                SimplePool.Despawn(DummyBall.gameObject);

            if (!isActive)
                return;

            DummyBall ballPrefab = color switch
            {
                EntityType.Red => red,
                EntityType.Yellow => yellow,
                EntityType.Green => green,
                EntityType.Blue => blue,
                EntityType.Violet => violet,
                EntityType.Orange => orange,
                EntityType.FireBall => fireball,
                EntityType.LeafBall => leafBall,
                EntityType.WaterBall => waterBall,
                EntityType.SunBall => sunBall,
                EntityType.ColorfulBall => colorful,
                _ => null
            };

            DummyBall = SimplePool.Spawn(ballPrefab, spawnPoint
                                          , spawnPoint.position
                                          , Quaternion.identity);
        }

        private void GetInputDirection()
        {
            _direction = inputHandler.InputPosition - spawnPoint.position;
        }

        private void ShootBall(BallShootModel shootModel)
        {
            SpawnBallAsync(shootModel).Forget();
        }

        private async UniTaskVoid SpawnBallAsync(BallShootModel shootModel)
        {
            if (!_canFire)
                return;

            _canFire = false;
            mainCharacter.Shoot();

            await UniTask.Delay(TimeSpan.FromSeconds(0.167f), cancellationToken: _token);
            if (_token.IsCancellationRequested)
                return;

            EntityMapData ballData = new EntityMapData
            {
                HP = 1,
                EntityType = shootModel.BallColor
            };

            if (shootModel.BallCount == 1)
            {
                BaseEntity newBall = _entityFactory.Create(ballData);
                ShootABall(newBall, _direction);
            }

            else
            {
                int _haftBallCount = shootModel.BallCount / 2;
                for (int i = -_haftBallCount; i <= _haftBallCount; i++)
                {
                    BaseEntity newBall = _entityFactory.Create(ballData);
                    Vector3 direction = Quaternion.AngleAxis(i * BallConstants.SpreadShootAngle, Vector3.forward) * _direction;
                    ShootABall(newBall, direction);
                }
            }

            SetBallColor(false, EntityType.None);
            await UniTask.Delay(TimeSpan.FromSeconds(0.333f), cancellationToken: _token);

            ballProvider.PopSequence().Forget();
            _decreaseMovePublisher.Publish(new DecreaseMoveMessage
            {
                CanDecreaseMove = !shootModel.IsPowerup
            });

            _canFire = true;
        }

        private void ShootABall(BaseEntity ball, Vector3 direction)
        {
            ball.IsFixedOnStart = false;
            ball.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.identity);

            if (ball.TryGetComponent(out IBallMovement ballMovement) && ball.TryGetComponent(out IBallPhysics ballPhysics))
            {
                ballMovement.CanMove = false;
                ballMovement.MovementState = BallMovementState.Ready;
                ballPhysics.ChangeLayerMask(false);

                ballPhysics.SetBodyActive(false);
                ballMovement.SetMoveDirection(direction);
                ballMovement.MovementState = BallMovementState.Moving;
                ballMovement.CanMove = true;
            }
        }

        private void RotatePointer()
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
            _limitAngleSine = Mathf.Sin((angle + 90f) / Mathf.Rad2Deg);
        }
    }
}
