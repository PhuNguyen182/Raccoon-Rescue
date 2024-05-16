using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Interfaces;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Effects;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.Boosters
{
    public class FireBallBooster : BaseEntity, IBallBooster, IFixedUpdateHandler, IBallMovement, IBallPhysics, IBallEffect
    {
        [SerializeField] private GameObject burningEffect;

        private Vector3 _direction;
        private Vector3 _previousPosition;

        public override EntityType EntityType => EntityType.FireBall;

        public override bool IsMatchable => false;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public bool CanMove
        {
            get => ballMovement.CanMove;
            set => ballMovement.CanMove = value;
        }

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

        public bool IsIgnored { get; set; }

        private IPublisher<ActiveBoosterMessage> _boosterPublisher;

        protected override void OnAwake()
        {
            base.OnAwake();
            _previousPosition = transform.position;
            UpdateHandlerManager.Instance.AddFixedUpdateBehaviour(this);
        }

        public void OnFixedUpdate()
        {
            if (CanMove && !IsFixedOnStart)
            {
                ToggleEffect(true);
                ballMovement.Move();
            }

            else ToggleEffect(false);

            CalculateBurnEffectAngle();
        }

        public async UniTask Activate()
        {
            await Explode();
        }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public override void DestroyEntity()
        {
            PublishScore();
            SimplePool.Despawn(this.gameObject);
        }

        public UniTask Explode()
        {
            PlayBlastEffect();
            return UniTask.CompletedTask;
        }

        public override void InitMessages()
        {
            _boosterPublisher = GlobalMessagePipe.GetPublisher<ActiveBoosterMessage>();
        }

        public override void ResetBall()
        {
            base.ResetBall();
            IsFixedOnStart = true;
            IsIgnored = false;
        }

        public void SetMoveDirection(Vector2 direction)
        {
            ballMovement.SetMoveDirection(direction);
        }

        public UniTask SnapTo(Vector3 position)
        {
            return UniTask.CompletedTask;
        }

        public UniTask BounceMove(Vector3 position)
        {
            return ballMovement.BounceMove(position);
        }

        public void ChangeLayerMask(bool isFixed)
        {
            ballMovement.ChangeLayerMask(isFixed);
        }

        public void SetBodyActive(bool active)
        {
            ballMovement.SetBodyActive(active);
        }

        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            ballMovement.AddForce(force, forceMode);
        }

        public override void OnSnapped()
        {
            IsIgnored = true;
            // To do: execute active booster
            _boosterPublisher.Publish(new ActiveBoosterMessage
            {
                Position = GridPosition
            });
        }

        public void PlayBlastEffect()
        {
            EffectManager.Instance.SpawnBoosterEffect(EntityType.FireBall, transform.position, Quaternion.identity);
        }

        public void ToggleEffect(bool active)
        {
            if (burningEffect != null)
                burningEffect.SetActive(active);
        }

        public void PlayColorfulEffect()
        {
            EffectManager.Instance.SpawnColorfulEffect(transform.position, Quaternion.identity);
        }

        private void CalculateBurnEffectAngle()
        {
            _direction = transform.position - _previousPosition;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg + 180f;
            burningEffect.transform.rotation = Quaternion.Euler(0, 0, angle);
            _previousPosition = transform.position;
        }

        private void OnDestroy()
        {
            UpdateHandlerManager.Instance.RemoveFixedUpdateBehaviour(this);
        }
    }
}
