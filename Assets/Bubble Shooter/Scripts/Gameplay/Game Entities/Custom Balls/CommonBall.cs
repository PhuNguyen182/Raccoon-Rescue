using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class CommonBall : BaseEntity, IFixedUpdateHandler, IBallMovement, IBallAnimation, IBallEffect, IBreakable
    {
        [SerializeField] private EntityType ballColor;

        public bool CanMove 
        { 
            get => ballMovement.CanMove; 
            set => ballMovement.CanMove = value; 
        }

        public override EntityType EntityType => ballColor;

        public override bool IsMatchable => true;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        private void OnEnable()
        {
            UpdateHandlerManager.Instance.AddFixedUpdateBehaviour(this);
        }

        public override void InitMessages()
        {
            
        }

        public void OnFixedUpdate()
        {
            if(CanMove && !IsFixedOnStart)
            {
                ballMovement.Move();
            }
        }

        public void SetBodyActive(bool active)
        {
            ballMovement.SetBodyActive(active);
        }

        public void SetMoveDirection(Vector2 direction)
        {
            ballMovement.SetMoveDirection(direction);
        }

        public UniTask SnapTo(Vector3 position)
        {
            return ballMovement.SnapTo(position);
        }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public bool Break()
        {
            return true;
        }

        public void PlayBounceAnimation()
        {
            
        }

        public void PlayBounceEffect()
        {
            
        }

        public override void Destroy()
        {
            SimplePool.Despawn(this.gameObject);
        }

        private void OnDisable()
        {
            UpdateHandlerManager.Instance.RemoveFixedUpdateBehaviour(this);
        }

        public override void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
