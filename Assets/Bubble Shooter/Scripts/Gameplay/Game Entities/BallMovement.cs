using System.Threading;
using Cysharp.Threading.Tasks;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Configs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public class BallMovement : MonoBehaviour, IFixedUpdateHandler, IBallMovement
    {
        [SerializeField] private Rigidbody2D ballBody;
        [SerializeField] private Collider2D ballCollider;

        [Header("Snapping")]
        [SerializeField] private float snapDuration = 0.2f;
        [SerializeField] private Ease snapEase = Ease.OutQuad;

        [Header("Check Reflection")]
        [Tooltip("This value is used to delay reflection checking when the ball hit the wall and bounce again")]
        [SerializeField] private int delayFrame = 3;
        [SerializeField] private float ballRadius = 0.3f;
        [SerializeField] private float ballDistance = 1f;

        [Header("Check Layer Maskes")]
        [SerializeField] private LayerMask ceilMask;
        [SerializeField] private LayerMask ballMask;
        [SerializeField] private LayerMask reflectMask;

        private float _ballSpeed = 0;
        private bool _canMove = false;
        private bool _isReflect = false;

        private CancellationToken _token;
        private Vector2 _moveDirection = Vector2.zero;
        private Tweener _snappingTween;

        public bool CanMove
        {
            get => _canMove; 
            set
            {
                _canMove = value;
                _ballSpeed = _canMove ? BubbleConstants.BallMoveSpeed : 0;
            }
        }

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        private void OnEnable()
        {
            UpdateHandlerManager.Instance.AddFixedUpdateBehaviour(this);
        }

        public void OnFixedUpdate()
        {
            if (_canMove)
            {
                MoveBall();
                CheckReflection().Forget();
            }
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

                await UniTask.DelayFrame(4, delayTiming: PlayerLoopTiming.FixedUpdate, cancellationToken: _token);
                _isReflect = false;
            }
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

        private void MoveBall()
        {
            ballBody.position = ballBody.position + Time.fixedDeltaTime * _ballSpeed * _moveDirection;
        }

        private Tweener CreateSnapTween(Vector3 position)
        {
            return transform.DOMove(position, snapDuration).SetEase(snapEase).SetAutoKill(false);
        }

        private void OnDisable()
        {
            UpdateHandlerManager.Instance.RemoveFixedUpdateBehaviour(this);
        }

        private void OnDestroy()
        {
            _snappingTween?.Kill();
        }
    }
}
