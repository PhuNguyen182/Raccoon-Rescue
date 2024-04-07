using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.GameBoard;
using BubbleShooter.Scripts.Gameplay.GameManagers;
using BubbleShooter.Scripts.Common.Configs;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public class BallMovement : MonoBehaviour, IBallMovement, IBallPhysics
    {
        [SerializeField] private Rigidbody2D ballBody;
        [SerializeField] private Collider2D ballCollider;
        [SerializeField] private BallMovementState moveState;

        [Header("Moving")]
        [BoxGroup(GroupID = "Move")]
        [SerializeField] private float moveDuration = 0.2f;
        [BoxGroup(GroupID = "Move")]
        [SerializeField] private Ease moveEase = Ease.OutQuad;
        
        [Header("Snapping")]
        [BoxGroup(GroupID = "Snap")]
        [SerializeField] private float snapDuration = 0.2f;
        [BoxGroup(GroupID = "Snap")]
        [SerializeField] private Ease snapEase = Ease.OutQuad;

        [Header("Check Reflection")]
        [Tooltip("This value is used to delay reflection checking when the ball hit the wall and bounce again")]
        [SerializeField] private int delayFrame = 4;
        [SerializeField] private float ballRadius = 0.3f;
        [SerializeField] private float ballDistance = 1f;

        [FoldoutGroup("Check Layer Maskes")]
        [SerializeField] private LayerMask ceilMask;
        [FoldoutGroup("Check Layer Maskes")]
        [SerializeField] private LayerMask ballMask;
        [FoldoutGroup("Check Layer Maskes")]
        [SerializeField] private LayerMask cellMask;
        [FoldoutGroup("Check Layer Maskes")]
        [SerializeField] private LayerMask reflectMask;

        private const string BallLayer = "Ball";
        private const string DefaultLayer = "Default";

        private int _ballLayer;
        private int _defaultLayer;

        private float _ballSpeed = 0;
        private bool _isReflect = false;

        private Tweener _movingTween;
        private Tweener _snappingTween;

        private CancellationToken _token;
        private Vector2 _moveDirection = Vector2.zero;

        private IBallEntity _currentBall;
        private RaycastHit2D _reflectHitInfo;
        private RaycastHit2D[] _nearestGridHitInfos;
        private Collider2D _neighborBallCollider;

        public bool CanMove
        {
            get => _ballSpeed > 0; 
            set => _ballSpeed = value ? BallConstants.BallMoveSpeed : 0;
        }

        public BallMovementState MovementState 
        { 
            get => moveState; 
            set => moveState = value; 
        }

        private void Awake()
        {
            _currentBall = GetComponent<IBallEntity>();
            _ballLayer = LayerMask.NameToLayer(BallLayer);
            _defaultLayer = LayerMask.NameToLayer(DefaultLayer);
            _token = this.GetCancellationTokenOnDestroy();
        }

        public void Move()
        {
            MoveBall();
            CheckNeighborBallToSnap();
            CheckReflection().Forget();
        }

        private async UniTaskVoid CheckReflection()
        {
            if (!CanMove)
                return;

            _reflectHitInfo = Physics2D.CircleCast(transform.position, ballRadius
                                                   , transform.up, ballDistance
                                                   , reflectMask);
            if (_reflectHitInfo && !_isReflect)
            {
                _isReflect = true;
                Vector2 hitNormal = _reflectHitInfo.normal;
                Vector2 newDirection = Vector2.Reflect(_moveDirection, hitNormal);
                _moveDirection = newDirection.normalized;

                await UniTask.DelayFrame(delayFrame, PlayerLoopTiming.FixedUpdate, _token);
                _isReflect = false;
            }
        }

        private void CheckNeighborBallToSnap()
        {
            _neighborBallCollider = Physics2D.OverlapCircle(transform.position
                                                            , ballRadius * 0.8f
                                                            , ballMask);
            if (_neighborBallCollider == null)
                return;

            if(_neighborBallCollider.TryGetComponent(out IBallMovement movement))
            {
                if (movement.MovementState == BallMovementState.Fixed)
                {
                    // Check nearest grid cell an snap to it;
                    CanMove = false;
                    ChangeLayerMask(true);
                    CheckNearestGrid();
                }
            }
        }

        private void CheckNearestGrid()
        {
            float nearestGridDistance = Mathf.Infinity;
            _nearestGridHitInfos = Physics2D.CircleCastAll(transform.position, ballRadius
                                                          , transform.up, ballDistance, cellMask);

            IGridCell targetGridCell;
            RaycastHit2D targetCellInfo = _nearestGridHitInfos[0];
            for (int i = 0; i < _nearestGridHitInfos.Length; i++)
            {
                if (!_nearestGridHitInfos[i].collider.TryGetComponent(out GridCellHolder gridHolder))
                    continue;

                IGridCell gridCell = GameController.Instance.GridCellManager.Get(gridHolder.GridPosition);
                if (gridCell == null || gridCell.ContainsBall)
                    continue;

                float checkDistance = Vector3.SqrMagnitude(_nearestGridHitInfos[i].transform.position - transform.position);
                
                if(checkDistance < nearestGridDistance)
                {
                    nearestGridDistance = checkDistance;
                    targetCellInfo = _nearestGridHitInfos[i];
                }
            }

            GridCellHolder cellHolder = targetCellInfo.collider.GetComponent<GridCellHolder>();
            targetGridCell = GameController.Instance.GridCellManager.Get(cellHolder.GridPosition);
            MovementState = BallMovementState.Fixed;
            
            SnapTo(targetCellInfo.transform.position);
            SetItemToGrid(targetGridCell);
        }

        private void SetItemToGrid(IGridCell gridCell)
        {
            gridCell.SetBall(_currentBall);
        }

        public void ChangeLayerMask(bool isFixed)
        {
            gameObject.layer = isFixed ? _ballLayer : _defaultLayer;
        }

        public UniTask MoveTo(Vector3 position)
        {
            _movingTween ??= CreateMoveTween(position);
            _movingTween.ChangeStartValue(transform.position);
            _movingTween.ChangeEndValue(position);

            _movingTween.Rewind();
            _movingTween.Play();

            return UniTask.Delay(TimeSpan.FromSeconds(_movingTween.Duration()), cancellationToken: _token);
        }

        public UniTask SnapTo(Vector3 position)
        {
            _snappingTween ??= CreateSnapTween(position);
            _snappingTween.ChangeStartValue(transform.position);
            _snappingTween.ChangeEndValue(position);
            
            _snappingTween.Rewind();
            _snappingTween.Play();

            return UniTask.Delay(TimeSpan.FromSeconds(_snappingTween.Duration()), cancellationToken: _token);
        }

        public void SetBodyActive(bool active)
        {
            if (active)
                MovementState = BallMovementState.Fall;

            ballBody.bodyType = active ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        }

        public void SetMoveDirection(Vector2 direction)
        {
            _moveDirection = direction.sqrMagnitude == 1 ? direction : direction.normalized;
        }

        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            ballBody.AddForce(force, forceMode);
        }

        private void MoveBall()
        {
            ballBody.position = ballBody.position + Time.fixedDeltaTime * _ballSpeed * _moveDirection;
        }

        private Tweener CreateSnapTween(Vector3 position)
        {
            return transform.DOMove(position, snapDuration).SetEase(snapEase).SetAutoKill(false);
        }

        private Tweener CreateMoveTween(Vector3 position)
        {
            return transform.DOMove(position, moveDuration).SetEase(moveEase).SetAutoKill(false);
        }

        private void OnDestroy()
        {
            _movingTween?.Kill();
            _snappingTween?.Kill();
        }
    }
}
