using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class ColorfulBall : BaseBall, IFixedUpdateHandler, IBallMovement, IBallTransformation
    {
        [Header("Colorful Balls")]
        [SerializeField] private Sprite red;
        [SerializeField] private Sprite yellow;
        [SerializeField] private Sprite green;
        [SerializeField] private Sprite blue;
        [SerializeField] private Sprite violet;
        [SerializeField] private Sprite orange;

        private EntityType _entityType = EntityType.ColorfulBall;

        public bool CanMove
        {
            get => ballMovement.CanMove;
            set => ballMovement.CanMove = value;
        }

        public override EntityType EntityType => _entityType;

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

        public override bool Break()
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
    }
}
