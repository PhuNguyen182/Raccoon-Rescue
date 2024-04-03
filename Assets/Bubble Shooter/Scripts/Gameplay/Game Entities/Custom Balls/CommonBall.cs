using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class CommonBall : BaseEntity, IFixedUpdateHandler, IBallMovement, IBallAnimation, IBallEffect, IBreakable
    {
        [SerializeField] private EntityType ballColor;

        [Header("Ball Colors")]
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite blue;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite green;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite orange;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite red;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite violet;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite yellow;

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

        public void SetColor(EntityType color)
        {
            ballColor = color;

            switch (color)
            {
                case EntityType.Red:
                    entityGraphics.SetEntitySprite(red);
                    break;
                case EntityType.Yellow:
                    entityGraphics.SetEntitySprite(yellow);
                    break;
                case EntityType.Green:
                    entityGraphics.SetEntitySprite(green);
                    break;
                case EntityType.Blue:
                    entityGraphics.SetEntitySprite(blue);
                    break;
                case EntityType.Violet:
                    entityGraphics.SetEntitySprite(violet);
                    break;
                case EntityType.Orange:
                    entityGraphics.SetEntitySprite(orange);
                    break;
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

        public override void ResetBall()
        {
            base.ResetBall();
            CanMove = false;
            IsFixedOnStart = true;
            SetBodyActive(false);
        }

        public override void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void OnDisable()
        {
            UpdateHandlerManager.Instance.RemoveFixedUpdateBehaviour(this);
        }
    }
}
