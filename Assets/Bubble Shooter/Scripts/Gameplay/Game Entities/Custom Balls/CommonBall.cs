using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Effects;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using MessagePipe;
using BubbleShooter.Scripts.Effects.BallEffects;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class CommonBall : BaseEntity, IFixedUpdateHandler, IBallMovement, IBallPhysics, IBallEffect, IBallPlayBoosterEffect, IBreakable
    {
        [SerializeField] private EntityType ballColor;

        [Header("Booster Effects")]
        [FoldoutGroup("Booster Effects")]
        [SerializeField] private GameObject leafEffect;
        [FoldoutGroup("Booster Effects")]
        [SerializeField] private ParticleSystem waterEffect;

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

        [Header("Text Colors")]
        [FoldoutGroup("Text Colors")]
        [SerializeField] private Color blueColor;
        [FoldoutGroup("Text Colors")]
        [SerializeField] private Color greenColor;
        [FoldoutGroup("Text Colors")]
        [SerializeField] private Color orangeColor;
        [FoldoutGroup("Text Colors")]
        [SerializeField] private Color redColor;
        [FoldoutGroup("Text Colors")]
        [SerializeField] private Color violetColor;
        [FoldoutGroup("Text Colors")]
        [SerializeField] private Color yellowColor;

        private Color _textColor;
        private IPublisher<CheckMatchMessage> _checkMatchPublisher;
        private GameObject _leafEffect;

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

        public Vector2 MoveDirection => ballMovement.MoveDirection;

        public bool EasyBreak => false;

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
                    _textColor = redColor;
                    entityGraphics.SetEntitySprite(red);
                    break;
                case EntityType.Yellow:
                    _textColor = yellowColor;
                    entityGraphics.SetEntitySprite(yellow);
                    break;
                case EntityType.Green:
                    _textColor = greenColor;
                    entityGraphics.SetEntitySprite(green);
                    break;
                case EntityType.Blue:
                    _textColor = blueColor;
                    entityGraphics.SetEntitySprite(blue);
                    break;
                case EntityType.Violet:
                    _textColor = violetColor;
                    entityGraphics.SetEntitySprite(violet);
                    break;
                case EntityType.Orange:
                    _textColor = orangeColor;
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

        public UniTask BounceMove(Vector3 position)
        {
            return ballMovement.BounceMove(position);
        }

        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            ballMovement.AddForce(force, forceMode);
        }

        public override UniTask Blast()
        {
            PlayBlastEffect();
            return UniTask.Delay(TimeSpan.FromSeconds(0.03f), cancellationToken: onDestroyToken);
        }

        public bool Break()
        {
            return true;
        }

        public void PlayBlastEffect()
        {
            EffectManager.Instance.SpawnBallPopEffect(transform.position, Quaternion.identity);
            FlyTextEffect flyText = EffectManager.Instance.SpawnFlyText(transform.position, Quaternion.identity);
            flyText.SetScore(Score);
            flyText.SetTextColor(_textColor);
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

        public async UniTask PlayBoosterEffect(EntityType booster)
        {
            switch (booster)
            {
                case EntityType.FireBall:
                case EntityType.WaterBall:
                case EntityType.SunBall:
                    await UniTask.CompletedTask;
                    break;
                case EntityType.LeafBall:
                    _leafEffect = SimplePool.Spawn(leafEffect, transform, transform.position, Quaternion.identity);
                    await UniTask.CompletedTask;
                    break;
            }

            await UniTask.CompletedTask;
        }

        public void ReleaseEffect()
        {
            ReleaseObject(_leafEffect);
        }

        private void ReleaseObject(GameObject obj)
        {
            if (obj != null)
            {
                obj.transform.SetParent(EffectContainer.Transform);
                SimplePool.Despawn(obj);
            }
        }

        public void ToggleEffect(bool active)
        {
            
        }

        public void PlayColorfulEffect()
        {
            EffectManager.Instance.SpawnColorfulEffect(transform.position, Quaternion.identity);
        }

        private void OnDestroy()
        {
            UpdateHandlerManager.Instance.RemoveFixedUpdateBehaviour(this);
        }
    }
}
