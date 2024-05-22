using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Effects.BallEffects;
using BubbleShooter.Scripts.Effects;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class ColorfulBall : BaseEntity, IFixedUpdateHandler, IBallMovement, IBallPhysics, IBreakable
    {
        [SerializeField] private Color textColor;

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

        private IPublisher<CheckMatchMessage> _checkMatchPublisher;

        public bool CanMove
        {
            get => ballMovement.CanMove;
            set => ballMovement.CanMove = value;
        }

        public override EntityType EntityType => EntityType.ColorfulBall;

        public override bool IsMatchable => true;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

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

        protected override void OnAwake()
        {
            base.OnAwake();
            UpdateHandlerManager.Instance.AddFixedUpdateBehaviour(this);
        }

        public override void InitMessages()
        {
            _checkMatchPublisher = GlobalMessagePipe.GetPublisher<CheckMatchMessage>();
        }

        public override async UniTask Blast()
        {
            await UniTask.CompletedTask;
        }

        public bool Break()
        {
            return true;
        }

        public override void DestroyEntity()
        {
            PublishScore();
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

        public UniTask BounceMove(Vector3 position)
        {
            return ballMovement.BounceMove(position);
        }

        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            ballMovement.AddForce(force, forceMode);
        }

        public void ChangeLayerMask(bool isFixed)
        {
            ballMovement.ChangeLayerMask(isFixed);
        }

        public override void OnSnapped()
        {
            _checkMatchPublisher.Publish(new CheckMatchMessage { Position = GridPosition });
        }

        public override void PlayBlastEffect(bool isFallen)
        {
            if (!isFallen)
                EffectManager.Instance.SpawnBallPopEffect(transform.position, Quaternion.identity);
            else
                EffectManager.Instance.SpawnBallPopEffect(transform.position, Quaternion.identity);

            FlyTextEffect flyText = EffectManager.Instance.SpawnFlyText(transform.position, Quaternion.identity);
            flyText.SetScore(Score);
            flyText.SetTextColor(textColor);
        }

        public override void ToggleEffect(bool active) { }

        public override void PlayColorfulEffect()
        {
            EffectManager.Instance.SpawnBallPopEffect(transform.position, Quaternion.identity);
            EffectManager.Instance.SpawnColorfulEffect(transform.position, Quaternion.identity);
        }

        private void OnDestroy()
        {
            UpdateHandlerManager.Instance?.RemoveFixedUpdateBehaviour(this);
        }
    }
}
