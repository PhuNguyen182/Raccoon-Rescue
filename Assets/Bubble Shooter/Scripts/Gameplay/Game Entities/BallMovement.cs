using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Configs;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public class BallMovement : MonoBehaviour, IBallMovement, IBallPhysics
    {
        [SerializeField] private Rigidbody2D ballBody;
        [SerializeField] private Collider2D ballCollider;

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
        [SerializeField] private LayerMask reflectMask;

        private float _ballSpeed = 0;
        private bool _isReflect = false;

        private Tweener _movingTween;
        private Tweener _snappingTween;
        private CancellationToken _token;
        private Vector2 _moveDirection = Vector2.zero;

        public bool CanMove
        {
            get => _ballSpeed > 0; 
            set => _ballSpeed = value ? BallConstants.BallMoveSpeed : 0;
        }

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        public void Move()
        {
            MoveBall();
            CheckReflection().Forget();
        }

        private async UniTaskVoid CheckReflection()
        {
            RaycastHit2D hitInfor = Physics2D.CircleCast(transform.position, ballRadius, transform.up, ballDistance, reflectMask);

            if (hitInfor && !_isReflect)
            {
                _isReflect = true;
                Vector2 hitNormal = hitInfor.normal;
                Vector2 newDirection = Vector2.Reflect(_moveDirection, hitNormal);
                _moveDirection = newDirection.normalized;

                await UniTask.DelayFrame(delayFrame, delayTiming: PlayerLoopTiming.FixedUpdate, cancellationToken: _token);
                _isReflect = false;
            }
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
            _snappingTween?.Kill();
        }
    }
}
