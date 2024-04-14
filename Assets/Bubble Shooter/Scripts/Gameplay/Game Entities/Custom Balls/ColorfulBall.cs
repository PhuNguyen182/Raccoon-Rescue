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
    public class ColorfulBall : BaseEntity, IFixedUpdateHandler, IBallMovement, IBallPhysics, IBreakable
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

        private IPublisher<AddScoreMessage> _addScorePublisher;
        private IPublisher<CheckMatchMessage> _checkMatchPublisher;

        public bool CanMove
        {
            get => ballMovement.CanMove;
            set => ballMovement.CanMove = value;
        }

        public override EntityType EntityType => EntityType.ColorfulBall;

        public override bool IsMatchable => true;

        public override int Score => 10;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public BallMovementState MovementState
        {
            get => ballMovement.MovementState;
            set => ballMovement.MovementState = value;
        }

        private void OnEnable()
        {
            UpdateHandlerManager.Instance.AddFixedUpdateBehaviour(this);
        }

        public override void InitMessages()
        {
            _addScorePublisher = GlobalMessagePipe.GetPublisher<AddScoreMessage>();
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
            _addScorePublisher.Publish(new AddScoreMessage { Score = Score });
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

        public void ChangeLayerMask(bool isFixed)
        {
            ballMovement.ChangeLayerMask(isFixed);
        }

        public override void OnSnapped()
        {
            _checkMatchPublisher.Publish(new CheckMatchMessage { Position = GridPosition });
        }

        private void OnDisable()
        {
            UpdateHandlerManager.Instance.RemoveFixedUpdateBehaviour(this);
        }
    }
}
