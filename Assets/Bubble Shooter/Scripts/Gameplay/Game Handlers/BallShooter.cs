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
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.Common.Constants;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace BubbleShooter.Scripts.Gameplay.GameHandlers
{
    public class BallShooter : MonoBehaviour
    {
        [SerializeField] private CommonBall prefab;
        [SerializeField] private SpriteRenderer ballPreview;

        [Space(10)]
        [SerializeField] private Transform pointer;
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

        [Header("Ball Colors")]
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite blue;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite green;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite orange;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite red;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite violet;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite yellow;

        private Vector2 _direction;
        private Vector3 _startPosition;
        private BallShootModel _ballModel;
        private float _limitAngleSine = 0;
        private bool _canFire = true;

        private CancellationToken _token;
        private EntityFactory _entityFactory;
        private List<int> _shootSequence;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
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

        public void SetShootSequence(List<int> queue)
        {
            _shootSequence = queue;
            PopSequence();
        }

        public void SetStartPosition()
        {
            _startPosition = mainCamera.ViewportToWorldPoint(normalizePosition);
            _startPosition.z = 0;
            transform.position = _startPosition;
        }

        public void SetColorModel(BallShootModel model)
        {
            _ballModel = model;
        }

        public bool IsIngamePowerupHolding()
        {
            return _ballModel.IsPowerup;
        }

        public void SwitchBall()
        {
            int endIndex = _shootSequence.Count - 1;
            EntityType currentColor = _ballModel.BallColor;
            EntityType nextColor = (EntityType)_shootSequence[endIndex];
            
            if (currentColor == nextColor)
                return;

            _ballModel.BallColor = nextColor;
            _shootSequence[endIndex] = (int)currentColor;
        }

        private void PopSequence()
        {
            if (_shootSequence.Count > 0)
            {
                EntityType color = (EntityType)_shootSequence[_shootSequence.Count - 1];

                SetColorModel(new BallShootModel
                {
                    IsPowerup = false,
                    BallColor = color,
                    BallCount = 1
                });

                SetBallColor(true, color);
                _shootSequence.RemoveAt(_shootSequence.Count - 1);
            }

            else Debug.Log("Out of move!");
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

            var ballData = new EntityMapData
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
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: _token);

            if (!shootModel.IsPowerup)
                PopSequence();

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
            pointer.rotation = Quaternion.Euler(0, 0, angle);
            _limitAngleSine = Mathf.Sin((angle + 90f) / Mathf.Rad2Deg);
        }

        private void SetBallColor(bool isActive, EntityType ballColor)
        {
            Sprite color = ballColor switch
            {
                EntityType.Blue => blue,
                EntityType.Green => green,
                EntityType.Orange => orange,
                EntityType.Red => red,
                EntityType.Violet => violet,
                EntityType.Yellow => yellow,
                _ => null
            };

            ballPreview.sprite = color;
            ballPreview.gameObject.SetActive(isActive);
        }
    }
}
