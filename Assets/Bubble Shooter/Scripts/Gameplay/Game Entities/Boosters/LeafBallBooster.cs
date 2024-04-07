using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.Boosters
{
    public class LeafBallBooster : BaseEntity, IBallBooster, IBallMovement
    {
        public override EntityType EntityType => EntityType.LeafBall;

        public override bool IsMatchable => false;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public bool CanMove { get => false; set { } }

        public BallMovementState MovementState
        {
            get => ballMovement.MovementState;
            set => ballMovement.MovementState = value;
        }

        public UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public override void Destroy()
        {

        }

        public UniTask Explode()
        {
            return UniTask.CompletedTask;
        }

        public override void InitMessages()
        {

        }

        public override void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }

        public override void ResetBall()
        {
            base.ResetBall();
            IsFixedOnStart = true;
            MovementState = BallMovementState.Fixed;
        }

        public void SetMoveDirection(Vector2 direction)
        {
            
        }

        public UniTask SnapTo(Vector3 position)
        {
            return UniTask.CompletedTask;
        }

        public UniTask MoveTo(Vector3 position)
        {
            return UniTask.CompletedTask;
        }
    }
}
