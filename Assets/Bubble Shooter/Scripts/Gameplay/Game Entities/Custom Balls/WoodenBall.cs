using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Messages;
using MessagePipe;
using System;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class WoodenBall : BaseEntity, IBallMovement, IBallPhysics, IBallHealth, IBreakable, IBallEffect
    {
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

        public override void InitMessages()
        {
            
        }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public bool Break()
        {
            PlayBlastEffect();

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

        public void SetMoveDirection(Vector2 direction)
        {
            
        }

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

        public void ChangeLayerMask(bool isFixed)
        {
            
        }

        public override void OnSnapped()
        {
            
        }

        public void PlayBlastEffect()
        {
            
        }
    }
}
