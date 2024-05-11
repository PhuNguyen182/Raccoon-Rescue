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
using BubbleShooter.Scripts.Gameplay.GameBoard;
using BubbleShooter.Scripts.Gameplay.Inputs;
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

        [Header("Line Colors")]
        [FoldoutGroup("Line Colors")]
        [SerializeField] private Color blueColor;
        [FoldoutGroup("Line Colors")]
        [SerializeField] private Color greenColor;
        [FoldoutGroup("Line Colors")]
        [SerializeField] private Color orangeColor;
        [FoldoutGroup("Line Colors")]
        [SerializeField] private Color redColor;
        [FoldoutGroup("Line Colors")]
        [SerializeField] private Color violetColor;
        [FoldoutGroup("Line Colors")]
        [SerializeField] private Color yellowColor;
        [FoldoutGroup("Line Colors")]
        [SerializeField] private Color colorfulColor;
        [FoldoutGroup("Line Colors")]
        [SerializeField] private Color fireballColor;
        [FoldoutGroup("Line Colors")]
        [SerializeField] private Color leafBallColor;
        [FoldoutGroup("Line Colors")]
        [SerializeField] private Color sunBallColor;
        [FoldoutGroup("Line Colors")]
        [SerializeField] private Color waterBallColor;

        [Space(10)]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform ballContainer;

        [Space(10)]
        [SerializeField] private AimingLine[] aimingLines;
        [SerializeField] private InputController inputHandler;
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
        private Color _lineColor;

        private IPublisher<DecreaseMoveMessage> _decreaseMovePublisher;

        public DummyBall DummyBall { get; set; }
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

            if (inputHandler.IsActive)
            {
                if (inputHandler.IsHolden)
                {
                    if (!inputHandler.IsPointerOverlapUI())
                    {
                        SetLineAngles();
                        _lineColor = GetLineColor(_ballModel.BallColor);
                        DrawLineColors(true, _lineColor);
                    }
                }

                else 
                    DrawLineColors(false, new Color(0, 0, 0, 0));

                if (inputHandler.IsRelease)
                {
                    if (!inputHandler.IsPointerOverlapUI() && _limitAngleSine > 0.15f)
                    {
                        inputHandler.IsActive = false;
                        ShootBall(_ballModel);
                        SetPremierState(false);
                    }
                }
            }

            else
                DrawLineColors(false, new Color(0, 0, 0, 0));
        }

        public void SetPremierState(bool isPremier)
        {
            for (int i = 0; i < aimingLines.Length; i++)
            {
                aimingLines[i].IsPremier = isPremier;
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

        private void SetLineAngles()
        {
            if (_ballModel.BallCount > 1)
            {
                int _haftBallCount = _ballModel.BallCount / 2;
                for (int i = -_haftBallCount; i <= _haftBallCount; i++)
                {
                    aimingLines[i + _haftBallCount].gameObject.SetActive(true);
                    aimingLines[i + _haftBallCount].SetAngle(i * BallConstants.SpreadShootAngle);
                }
            }

            else
            {
                int _haftBallCount = aimingLines.Length / 2;
                for (int i = -_haftBallCount; i <= _haftBallCount; i++)
                {
                    if (i != 0)
                        aimingLines[i + _haftBallCount].gameObject.SetActive(false);
                }

                aimingLines[_haftBallCount].gameObject.SetActive(true);
                aimingLines[_haftBallCount].SetAngle(0);
            }
        }

        private void DrawLineColors(bool isDraw, Color color)
        {
            if (_ballModel.BallCount > 1)
            {
                for (int i = 0; i < aimingLines.Length; i++)
                {
                    aimingLines[i].DrawAimingLine(isDraw, color);
                }
            }

            else aimingLines[aimingLines.Length / 2].DrawAimingLine(isDraw, color);
        }

        private Color GetLineColor(EntityType ballColor)
        {
            Color color = ballColor switch
            {
                EntityType.Blue => blueColor,
                EntityType.Green => greenColor,
                EntityType.Orange => orangeColor,
                EntityType.Red => redColor,
                EntityType.Violet => violetColor,
                EntityType.Yellow => yellowColor,
                EntityType.ColorfulBall => colorfulColor,
                EntityType.FireBall => fireballColor,
                EntityType.LeafBall => leafBallColor,
                EntityType.SunBall => sunBallColor,
                EntityType.WaterBall => waterBallColor,
                _ => default
            };

            return color;
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
                    ShootABall(newBall, direction, i + _haftBallCount);
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

        private void ShootABall(BaseEntity ball, Vector3 direction, int index = 1)
        {
            if (ball == null)
                return;

            ball.IsFixedOnStart = false;
            ball.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.identity);
            GridCellHolder gridCell = aimingLines[index].ReportGridCell();

            if (ball.TryGetComponent(out BallMovement ballMovement) && ball.TryGetComponent(out IBallPhysics ballPhysics))
            {
                ballMovement.CanMove = false;
                ballMovement.MovementState = BallMovementState.Ready;
                ballPhysics.ChangeLayerMask(false);

                ballPhysics.SetBodyActive(false);
                ballMovement.SetMoveDirection(direction);
                ballMovement.SetGridCellHolder(gridCell);

                ballMovement.MovementState = BallMovementState.Moving;
                ballMovement.CanMove = true;
            }
        }

        private void GetInputDirection()
        {
            _direction = inputHandler.Pointer - spawnPoint.position;
        }

        private void RotatePointer()
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
            _limitAngleSine = Mathf.Sin((angle + 90f) / Mathf.Rad2Deg);
        }
    }
}
