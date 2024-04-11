using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Interfaces;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.Boosters
{
    public class FireBallBooster : BaseEntity, IBallBooster, IFixedUpdateHandler, IBallMovement, IBallPhysics
    {
        public override EntityType EntityType => EntityType.FireBall;

        public override bool IsMatchable => false;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public bool CanMove
        {
            get => ballMovement.CanMove;
            set => ballMovement.CanMove = value;
        }

        public BallMovementState MovementState
        {
            get => ballMovement.MovementState;
            set => ballMovement.MovementState = value;
        }

        public bool IsIgnored { get; set; }

        private IPublisher<ActiveBoosterMessage> _boosterPublisher;

        private void OnEnable()
        {
            UpdateHandlerManager.Instance.AddFixedUpdateBehaviour(this);
        }

        public void OnFixedUpdate()
        {
            if (CanMove && !IsFixedOnStart)
            {
                ballMovement.Move();
            }
        }

        public async UniTask Activate()
        {
            await Blast();
        }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public override void DestroyEntity()
        {
            SimplePool.Despawn(this.gameObject);
        }

        public UniTask Explode()
        {
            return UniTask.CompletedTask;
        }

        public override void InitMessages()
        {
            _boosterPublisher = GlobalMessagePipe.GetPublisher<ActiveBoosterMessage>();
        }

        public override void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }

        public override void ResetBall()
        {
            base.ResetBall();
            IsFixedOnStart = true;
            IsIgnored = false;
        }

        public void SetMoveDirection(Vector2 direction)
        {
            ballMovement.SetMoveDirection(direction);
        }

        public UniTask SnapTo(Vector3 position)
        {
            return UniTask.CompletedTask;
        }

        public UniTask MoveTo(Vector3 position)
        {
            return UniTask.CompletedTask;
        }

        public void ChangeLayerMask(bool isFixed)
        {
            ballMovement.ChangeLayerMask(isFixed);
        }

        public void SetBodyActive(bool active)
        {
            ballMovement.SetBodyActive(active);
        }

        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            
        }

        public override void OnSnapped()
        {
            IsIgnored = true;
            // To do: execute active booster
            _boosterPublisher.Publish(new ActiveBoosterMessage
            {
                Position = GridPosition
            });
        }

        private void OnDisable()
        {
            UpdateHandlerManager.Instance.RemoveFixedUpdateBehaviour(this);
        }
    }
}
