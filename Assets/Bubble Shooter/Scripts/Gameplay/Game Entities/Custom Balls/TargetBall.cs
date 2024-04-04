using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class TargetBall : BaseEntity, IBallMovement, ITargetBall
    {
        [SerializeField] private int id;
        [SerializeField] private EntityType entityColor;
        [SerializeField] private TargetType targetColor;

        public override bool IsMatchable => true;

        public override bool IsFixedOnStart { get => true; set { } }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public override EntityType EntityType => entityColor;

        public TargetType TargetColor => targetColor;

        public bool CanMove { get => false; set { } }

        public int ID => id;

        protected override void OnStart()
        {
            entityGraphics.PlaySadEmotion();
        }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public override void Destroy()
        {
            SimplePool.Despawn(this.gameObject);
        }

        public override void InitMessages()
        {
            
        }

        public UniTask MoveTo(Vector3 position)
        {
            return ballMovement.MoveTo(position);
        }

        public void SetColor(EntityType color)
        {
            entityColor = color;
        }

        public void SetID(int id)
        {
            this.id = id;
        }

        public void SetMoveDirection(Vector2 direction)
        {
            
        }

        public void SetTargetColor(TargetType targetColor)
        {
            this.targetColor = targetColor;
        }

        public override void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }

        public UniTask SnapTo(Vector3 position)
        {
            return ballMovement.SnapTo(position);
        }
    }
}
