using Cysharp.Threading.Tasks;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Configs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public class BallMovement : MonoBehaviour, IFixedUpdateHandler, IBallMovement
    {
        [SerializeField] private Rigidbody2D ballBody;
        [SerializeField] private Collider2D ballCollider;

        [Header("Check Layer Maskes")]
        [SerializeField] private LayerMask ceilMask;
        [SerializeField] private LayerMask ballMask;
        [SerializeField] private LayerMask reflectMask;

        private float _ballSpeed = 0;
        private bool _canMove = false;
        private Vector2 _moveDirection = Vector2.zero;

        public bool CanMove
        {
            get => _canMove; 
            set
            {
                _canMove = value;
                _ballSpeed = _canMove ? BubbleConfig.BALL_MOVE_SPEED : 0;
            }
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
            }
        }

        public UniTask SnapTo(Vector3 position)
        {
            transform.position = position;
            return UniTask.CompletedTask;
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

        private void OnDestroy()
        {
            UpdateHandlerManager.Instance.RemoveFixedUpdateBehaviour(this);
        }
    }
}
