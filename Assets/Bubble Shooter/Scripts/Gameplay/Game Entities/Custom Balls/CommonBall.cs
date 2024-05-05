using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class CommonBall : BaseEntity, IFixedUpdateHandler, IBallMovement, IBallPhysics, IBallEffect, IBreakable
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

        private IPublisher<CheckMatchMessage> _checkMatchPublisher;

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

        public BallMovementState MovementState
        {
            get => ballMovement.MovementState;
            set => ballMovement.MovementState = value;
        }

        public bool EasyBreak => false;

        protected override void OnAwake()
        {
            base.OnAwake();
            UpdateHandlerManager.Instance.AddFixedUpdateBehaviour(this);
        }

        public override void InitMessages()
        {
            _checkMatchPublisher = GlobalMessagePipe.GetPublisher<CheckMatchMessage>();
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

        public void ChangeLayerMask(bool isFixed)
        {
            ballMovement.ChangeLayerMask(isFixed);
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

        public UniTask MoveTo(Vector3 position)
        {
            return ballMovement.MoveTo(position);
        }

        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            ballMovement.AddForce(force, forceMode);
        }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public bool Break()
        {
            return true;
        }

        public void PlayBlastEffect()
        {
            
        }

        public override void DestroyEntity()
        {
            PublishScore();
            SimplePool.Despawn(this.gameObject);
        }

        public override void ResetBall()
        {
            base.ResetBall();
            CanMove = false;
            IsFixedOnStart = true;
            SetBodyActive(false);
        }

        public override void OnSnapped()
        {
            _checkMatchPublisher.Publish(new CheckMatchMessage { Position = GridPosition });
        }

        private void OnDestroy()
        {
            UpdateHandlerManager.Instance.RemoveFixedUpdateBehaviour(this);
        }
    }
}
