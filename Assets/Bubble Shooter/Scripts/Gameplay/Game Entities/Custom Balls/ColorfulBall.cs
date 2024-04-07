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
    public class ColorfulBall : BaseEntity, IFixedUpdateHandler, IBallMovement, IBallPhysics, IBallTransformation, IBreakable
    {
        [Header("Colorful Balls")]
        [FoldoutGroup("Colors")]
        [SerializeField] private Sprite red;
        [FoldoutGroup("Colors")]
        [SerializeField] private Sprite yellow;
        [FoldoutGroup("Colors")]
        [SerializeField] private Sprite green;
        [FoldoutGroup("Colors")]
        [SerializeField] private Sprite blue;
        [FoldoutGroup("Colors")]
        [SerializeField] private Sprite violet;
        [FoldoutGroup("Colors")]
        [SerializeField] private Sprite orange;

        private EntityType _entityType = EntityType.ColorfulBall;

        public bool CanMove
        {
            get => ballMovement.CanMove;
            set => ballMovement.CanMove = value;
        }

        public override EntityType EntityType => _entityType;

        public override bool IsMatchable => true;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public BallMovementState MovementState
        {
            get => ballMovement.MovementState;
            set => ballMovement.MovementState = value;
        }

        public override void InitMessages()
        {
            
        }

        public void TransformTo(EntityType color)
        {
            _entityType = color;

            Sprite colorSprite = color switch
            {
                EntityType.Red => red,
                EntityType.Yellow => yellow,
                EntityType.Green => green,
                EntityType.Blue => blue,
                EntityType.Violet => violet,
                EntityType.Orange => orange,
                _ => null
            };

            entityGraphics.SetEntitySprite(colorSprite);
        }

        public override async UniTask Blast()
        {
            await UniTask.CompletedTask;
        }

        public bool Break()
        {
            return false;
        }

        public override void Destroy()
        {
            SimplePool.Despawn(this.gameObject);
        }

        public void OnFixedUpdate()
        {
            if (CanMove && !IsFixedOnStart)
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

        public UniTask MoveTo(Vector3 position)
        {
            return ballMovement.MoveTo(position);
        }

        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            ballMovement.AddForce(force, forceMode);
        }

        public override void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
