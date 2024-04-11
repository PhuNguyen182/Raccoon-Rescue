using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class TargetBall : BaseEntity, IBallMovement, ITargetBall, IBreakable
    {
        [Header("Target Setting")]
        [SerializeField] private int id;
        [SerializeField] private EntityType entityColor;
        [SerializeField] private TargetType targetColor;
        [SerializeField] private Animator targetAnimator;

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

        private static readonly int _sadEmotionHash = Animator.StringToHash("SadEmotion");

        public override bool IsMatchable => true;

        public override bool IsFixedOnStart { get => true; set { } }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public override EntityType EntityType => entityColor;

        public TargetType TargetColor => targetColor;

        public bool CanMove { get => false; set { } }

        public int ID => id;

        public BallMovementState MovementState
        {
            get => ballMovement.MovementState;
            set => ballMovement.MovementState = value;
        }

        protected override void OnStart()
        {
            PlaySadEmotion();
        }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public override void DestroyEntity()
        {
            SimplePool.Despawn(this.gameObject);
        }

        public bool Break()
        {
            return true;
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

        public override void ResetBall()
        {
            base.ResetBall();
            IsFixedOnStart = true;
        }

        public UniTask SnapTo(Vector3 position)
        {
            return ballMovement.SnapTo(position);
        }

        public override void OnSnapped()
        {
            
        }

        public void FreeTarget()
        {
            
        }

        public void PlaySadEmotion()
        {
            if (targetAnimator != null)
            {
                int rand = Random.Range(1, 7);
                targetAnimator.SetInteger(_sadEmotionHash, rand);
            }
        }

    }
}
