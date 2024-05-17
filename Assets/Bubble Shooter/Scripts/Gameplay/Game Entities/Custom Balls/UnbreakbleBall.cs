using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Effects;

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

        public Func<Vector3, Vector3Int> WorldToGridFunction
        {
            get => ballMovement.WorldToGridFunction;
            set => ballMovement.WorldToGridFunction = value;
        }

        public Func<Vector3Int, IGridCell> TakeGridCellFunction
        {
            get => ballMovement.TakeGridCellFunction;
            set => ballMovement.TakeGridCellFunction = value;
        }

        public Vector2 MoveDirection => ballMovement.MoveDirection;

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public override void DestroyEntity()
        {
            PublishScore();
            SimplePool.Despawn(this.gameObject);
        }

        public override void InitMessages() { }

        public UniTask BounceMove(Vector3 position)
        {
            return ballMovement.BounceMove(position);
        }

        public void SetBodyActive(bool active)
        {
            ballMovement.SetBodyActive(active);
        }

        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            ballMovement.AddForce(force, forceMode);
        }

        public void SetMoveDirection(Vector2 direction) { }

        public override void ResetBall()
        {
            base.ResetBall();
            IsFixedOnStart = true;
        }

        public UniTask SnapTo(Vector3 position)
        {
            return ballMovement.SnapTo(position);
        }

        public void ChangeLayerMask(bool isFixed) { }

        public override void OnSnapped() { }

        public override void PlayBlastEffect(bool isFallen) 
        {
            EffectManager.Instance.SpawnBallPopEffect(transform.position, Quaternion.identity);
        }

        public override void ToggleEffect(bool active) { }

        public override void PlayColorfulEffect() { }
    }
}
