using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Messages;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class UnbreakbleBall : BaseEntity, IBallMovement, IBallPhysics
    {
        public override bool IsMatchable => false;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public override EntityType EntityType => EntityType.UnbreakableBall;

        public bool CanMove { get => false; set { } }

        public BallMovementState MovementState
        {
            get => ballMovement.MovementState;
            set => ballMovement.MovementState = value;
        }

        private IPublisher<AddScoreMessage> _addScorePublisher;

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public override void DestroyEntity()
        {
            if (IsFallen)
                _addScorePublisher.Publish(new AddScoreMessage { Score = Score });

            SimplePool.Despawn(this.gameObject);
        }

        public override void InitMessages()
        {
            _addScorePublisher = GlobalMessagePipe.GetPublisher<AddScoreMessage>();
        }

        public UniTask MoveTo(Vector3 position)
        {
            return ballMovement.MoveTo(position);
        }

        public void SetBodyActive(bool active)
        {
            ballMovement.SetBodyActive(active);
        }

        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            ballMovement.AddForce(force, forceMode);
        }

        public void SetMoveDirection(Vector2 direction)
        {
            
        }

        public override void ResetBall()
        {
            base.ResetBall();
            IsFixedOnStart = true;
        }

        public UniTask SnapTo(Vector3 position)
        {
            return ballMovement.SnapTo(position);
        }

        public void ChangeLayerMask(bool isFixed)
        {
            
        }

        public override void OnSnapped()
        {
            
        }
    }
}
