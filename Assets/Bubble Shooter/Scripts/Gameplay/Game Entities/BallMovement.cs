using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.GameBoard;
using BubbleShooter.Scripts.Gameplay.GameManagers;
using BubbleShooter.Scripts.Gameplay.GameTasks;
using BubbleShooter.Scripts.Common.Constants;
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
        [SerializeField] private float bounceDuration = 0.25f;
        [BoxGroup(GroupID = "Move")]
        [SerializeField] private Ease bounceEase = Ease.InOutSine;
        
        [Header("Snapping")]
        [BoxGroup(GroupID = "Snap")]
        [SerializeField] private float snapDuration = 0.25f;
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
        private GridCellHolder _gridCellHolder;

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

        public Vector2 MoveDirection => _moveDirection;

        public Func<Vector3, Vector3Int> WorldToGridFunction { get; set; }
        public Func<Vector3Int, IGridCell> TakeGridCellFunction { get; set; }

        private void Awake()
        {
            _currentBall = GetComponent<IBallEntity>();
            _ballLayer = LayerMask.NameToLayer(BallConstants.BallLayer);
            _defaultLayer = LayerMask.NameToLayer(BallConstants.DefaultLayer);
            _token = this.GetCancellationTokenOnDestroy();
        }

        public void Move()
        {
            MoveToStopPosition();
            CheckReflection();
            MoveBall();
        }

        private void MoveToStopPosition()
        {
            if (_gridCellHolder != null)
            {
                IGridCell gridCell = TakeGridCellFunction.Invoke(_gridCellHolder.GridPosition);

                if (gridCell != null && !gridCell.ContainsBall && HasAnyNeighborAt(gridCell.GridPosition))
                    SnapToReportedStopPosition();

                else FreeMoveToStopPosition();
            }

            else FreeMoveToStopPosition();
        }

        private void FreeMoveToStopPosition()
        {
            CheckSnapToCeil();
            CheckNeighborBallToSnap();
        }

        private void SnapToReportedStopPosition()
        {
            SnapToReportedCellAsync().Forget();
        }

        private void CheckReflection()
        {
            CheckReflectionAsync().Forget();
        }

        private void CheckSnapToCeil()
        {
            CheckSnapToCeilAsync().Forget();
        }

        private async UniTask SnapToReportedCellAsync()
        {
            float squaredSnapDistance = BallConstants.GridSnapDistance * BallConstants.GridSnapDistance;
            if (Vector3.SqrMagnitude(_gridCellHolder.transform.position - transform.position) <= squaredSnapDistance)
            {
                CanMove = false;
                ChangeLayerMask(true);

                Vector3Int position = _gridCellHolder.GridPosition;
                IGridCell checkCell = TakeGridCellFunction.Invoke(position);

                await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate, _token);
                SnapToCell(checkCell).Forget();
            }
        }

        private async UniTaskVoid CheckReflectionAsync()
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

        private async UniTaskVoid CheckSnapToCeilAsync()
        {
            Vector3Int position = WorldToGridFunction.Invoke(transform.position);
            IGridCell checkCell = TakeGridCellFunction.Invoke(position);

            if (checkCell == null)
                return;

            if (checkCell.IsCeil)
            {
                CanMove = false;
                ChangeLayerMask(true);

                await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate, _token);
                SnapToCell(checkCell).Forget();
            }
        }

        private void CheckNeighborBallToSnap()
        {
            _neighborBallCollider = Physics2D.OverlapCircle(transform.position, ballRadius, ballMask);

            if (_neighborBallCollider == null)
                return;

            Vector3Int position = WorldToGridFunction.Invoke(_neighborBallCollider.transform.position);
            IGridCell checkCell = TakeGridCellFunction.Invoke(position);

            if (checkCell == null)
                return;

            if (!checkCell.ContainsBall)
                return;

            if (checkCell.BallEntity is not IBallMovement movement)
                return;

            if (movement.MovementState == BallMovementState.Fixed)
            {
                CanMove = false;
                ChangeLayerMask(true);
                CheckNearestGrid().Forget();
            }
        }

        private async UniTaskVoid SnapToCell(IGridCell cell)
        {
            MovementState = BallMovementState.Fixed;

            SetItemToGrid(cell);
            SnapTo(cell.WorldPosition).Forget();

            await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate, _token);
            _currentBall.OnSnapped();
        }

        private async UniTask CheckNearestGrid()
        {
            float nearestGridDistance = Mathf.Infinity;
            _nearestGridHitInfos = Physics2D.CircleCastAll(transform.position, ballRadius
                                                          , transform.up, ballDistance, cellMask);

            IGridCell targetGridCell;
            RaycastHit2D targetCellInfo = _nearestGridHitInfos[0];

            for (int i = 0; i < _nearestGridHitInfos.Length; i++)
            {
                if (!_nearestGridHitInfos[i].collider.TryGetComponent(out GridCellHolder grid))
                    continue;

                IGridCell gridCell = TakeGridCellFunction.Invoke(grid.GridPosition);
                if (gridCell == null || gridCell.ContainsBall)
                    continue;

                float checkDistance = Vector3.SqrMagnitude(_nearestGridHitInfos[i].transform.position - transform.position);
                
                if(checkDistance < nearestGridDistance)
                {
                    nearestGridDistance = checkDistance;
                    targetCellInfo = _nearestGridHitInfos[i];
                }
            }

            GridCellHolder gridHolder = targetCellInfo.collider.GetComponent<GridCellHolder>();
            targetGridCell = TakeGridCellFunction.Invoke(gridHolder.GridPosition);
            MovementState = BallMovementState.Fixed;
            
            SetItemToGrid(targetGridCell);
            SnapTo(targetCellInfo.transform.position).Forget();

            await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate, _token);
            _currentBall.OnSnapped();
        }

        private void SetItemToGrid(IGridCell gridCell)
        {
            gridCell.SetBall(_currentBall);
            GameController.Instance.AddEntity(_currentBall);
        }

        public void SetGridCellHolder(GridCellHolder gridCellHolder)
        {
            _gridCellHolder = gridCellHolder;
        }

        public void ChangeLayerMask(bool isFixed)
        {
            gameObject.layer = isFixed ? _ballLayer : _defaultLayer;
        }

        public UniTask BounceMove(Vector3 position)
        {
            _movingTween ??= CreateMoveBounceTween(position);
            _movingTween.ChangeStartValue(transform.position);
            _movingTween.ChangeEndValue(position);

            _movingTween.Rewind();
            _movingTween.Play();

            float duration = _movingTween.Duration();
            return UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _token);
        }

        public UniTask SnapTo(Vector3 position)
        {
            _snappingTween ??= CreateSnapTween(position);
            _snappingTween.ChangeStartValue(transform.position);
            _snappingTween.ChangeEndValue(position);
            
            _snappingTween.Rewind();
            _snappingTween.Play();

            float duration = _snappingTween.Duration();
            return UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _token);
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

        private bool HasAnyNeighborAt(Vector3Int position)
        {
            IGridCell gridCell;

            for (int i = 0; i < CommonProperties.MaxNeighborCount; i++)
            {
                Vector3Int neighborOffset = position.y % 2 == 0
                                            ? CommonProperties.EvenYNeighborOffsets[i]
                                            : CommonProperties.OddYNeighborOffsets[i];
                gridCell = TakeGridCellFunction.Invoke(position + neighborOffset);

                if (gridCell == null)
                    continue;

                if (gridCell.ContainsBall)
                    return true;
            }

            return false;
        }

        private void MoveBall()
        {
            ballBody.position = ballBody.position + Time.fixedDeltaTime * _ballSpeed * _moveDirection;
        }

        private Tweener CreateSnapTween(Vector3 position)
        {
            return transform.DOMove(position, snapDuration)
                            .SetEase(snapEase).SetAutoKill(false);
        }

        private Tweener CreateMoveBounceTween(Vector3 position)
        {
            return transform.DOMove(position, bounceDuration)
                            .SetEase(bounceEase).SetLoops(2, LoopType.Yoyo)
                            .SetAutoKill(false);
        }

        private void OnDestroy()
        {
            _movingTween?.Kill();
            _snappingTween?.Kill();
        }
    }
}
