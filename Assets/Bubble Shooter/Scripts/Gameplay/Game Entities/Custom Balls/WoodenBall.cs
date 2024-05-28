using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Effects.BallEffects;
using BubbleShooter.Scripts.Effects;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class WoodenBall : BaseEntity, IBallMovement, IBallPhysics, IBallHealth, IBreakable
    {
        [SerializeField] private Color textColor;
        [SerializeField] private AudioClip woodenBreak;

        [Header("Health Sprites")]
        [SerializeField] private Sprite[] hpStates;

        private int _hp = 0;
        private int _maxHp = 0;

        public bool CanMove { get => false; set { } }
        public override EntityType EntityType => EntityType.WoodenBall;

        public int MaxHP => _maxHp;

        public override bool IsMatchable => false;

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

        public override void InitMessages() { }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public bool Break()
        {
            PlayBlastEffect(false);
            entityAudio.PlaySoundOneShot(woodenBreak);

            if (_hp > 0)
            {
                _hp = _hp - 1;
                SetRenderer();
                return _hp <= 0;
            }

            return true;
        }

        public override void DestroyEntity()
        {
            PublishScore();
            SimplePool.Despawn(this.gameObject);
        }

        public void SetBodyActive(bool active)
        {
            ballMovement.SetBodyActive(active);
        }

        public void SetMaxHP(int maxHP)
        {
            _maxHp = maxHP;
            _hp = _maxHp;
        }

        public override void ResetBall()
        {
            base.ResetBall();
            SetRenderer();
            IsFixedOnStart = true;
        }

        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            ballMovement.AddForce(force, forceMode);
        }

        public void SetMoveDirection(Vector2 direction) { }

        public UniTask SnapTo(Vector3 position)
        {
            return UniTask.CompletedTask;
        }

        public UniTask BounceMove(Vector3 position)
        {
            return ballMovement.BounceMove(position);
        }

        private void SetRenderer()
        {
            if (_hp > 0)
            {
                entityGraphics.SetEntitySprite(hpStates[_hp - 1]);
            }
        }

        public void ChangeLayerMask(bool isFixed) { }

        public override void OnSnapped() { }

        public override void PlayBlastEffect(bool isFallen)
        {
            if (!isFallen)
            {
                if (_hp > 0)
                    EffectManager.Instance.SpawnWoodenEffect(transform.position, Quaternion.identity);
                
                else
                {
                    EffectManager.Instance.SpawnWoodenEffect(transform.position, Quaternion.identity);
                    EffectManager.Instance.SpawnBallPopEffect(transform.position, Quaternion.identity);
                }
            }

            else
                EffectManager.Instance.SpawnBallPopEffect(transform.position, Quaternion.identity);

            FlyTextEffect flyText = EffectManager.Instance.SpawnFlyText(transform.position, Quaternion.identity);
            flyText.SetScore(Score);
            flyText.SetTextColor(textColor);
        }

        public override void ToggleEffect(bool active) { }

        public override void PlayColorfulEffect() { }
    }
}
