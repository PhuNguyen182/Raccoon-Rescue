using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.Common.Constants;
using BubbleShooter.Scripts.Common.PlayDatas;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class TargetBall : BaseEntity, IBallMovement, ITargetBall, IBreakable
    {
        [SerializeField] private FreedTarget freedTarget;
        [SerializeField] private GameObject boomEffect;

        [Header("Target Setting")]
        [SerializeField] private int id;
        [SerializeField] private EntityType entityColor;
        [SerializeField] private TargetType targetColor;
        [SerializeField] private Animator targetAnimator;
        [SerializeField] private FlyToTargetObject flyObject;

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

        private IPublisher<MoveToTargetMessage> _moveTargetPublisher;
        private IPublisher<AddTargetMessage> _checkTargetPublisher;

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

        public bool EasyBreak => false;

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
            FreeTargetAsync().Forget();
            SimplePool.Despawn(this.gameObject);
        }

        public bool Break()
        {
            return true;
        }

        public override void InitMessages()
        {
            _moveTargetPublisher = GlobalMessagePipe.GetPublisher<MoveToTargetMessage>();
            _checkTargetPublisher = GlobalMessagePipe.GetPublisher<AddTargetMessage>();
        }

        public UniTask BounceMove(Vector3 position)
        {
            return ballMovement.BounceMove(position);
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

        private async UniTask FreeTargetAsync()
        {
            if (boomEffect != null)
                SimplePool.Spawn(boomEffect, EffectContainer.Transform, transform.position, Quaternion.identity);

            SimplePool.Spawn(freedTarget, EffectContainer.Transform
                             , transform.position, Quaternion.identity);

            MoveTargetData targetInfo = await SendMoveToTargetMessage();
            FlyToTargetObject flyTarget = SimplePool.Spawn(flyObject, EffectContainer.Transform,
                                                           transform.position, Quaternion.identity);
            
            flyTarget.transform.localScale = Vector3.one;
            float distance = Vector3.Distance(targetInfo.Destination, transform.position);
            
            float speed = Mathf.Lerp(EntityConstants.MoveToNearTargetSpeed, 
                                     EntityConstants.MoveToFarTargetSpeed, 
                                     distance / EntityConstants.MaxMoveDistance);
            
            float duration = Vector3.Distance(targetInfo.Destination, transform.position) / speed;
            
            UniTask moveTask = flyTarget.MoveToTarget(targetInfo.Destination, duration);
            await CheckTargetAsync(new AddTargetMessage(), moveTask);
            SimplePool.Despawn(flyTarget.gameObject);
        }

        private async UniTask CheckTargetAsync(AddTargetMessage message, UniTask moveTask)
        {
            await moveTask.ContinueWith(() => _checkTargetPublisher.Publish(message));
        }

        private UniTask<MoveTargetData> SendMoveToTargetMessage()
        {
            MoveToTargetMessage message = new MoveToTargetMessage
            {
                Source = new UniTaskCompletionSource<MoveTargetData>()
            };

            _moveTargetPublisher.Publish(message);
            return message.Source.Task;
        }
    }
}
