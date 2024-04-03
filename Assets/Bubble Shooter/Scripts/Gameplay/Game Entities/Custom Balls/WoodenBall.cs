using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class WoodenBall : BaseEntity, IFixedUpdateHandler, IBallMovement, IBallHealth, IBreakable
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

        public override void InitMessages()
        {
            
        }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public bool Break()
        {
            if (_hp > 0)
            {
                _hp = _hp - 1;
                SetRenderer();
                return _hp <= 0;
            }

            return true;
        }

        public override void Destroy()
        {
            SimplePool.Despawn(this.gameObject);
        }

        public void OnFixedUpdate()
        {
            
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
        }

        public void SetMoveDirection(Vector2 direction)
        {
            
        }

        public UniTask SnapTo(Vector3 position)
        {
            return UniTask.CompletedTask;
        }

        private void SetRenderer()
        {
            entityGraphics.SetEntitySprite(hpStates[_hp - 1]);
        }

        public override void SetWorldPosition(Vector3 position)
        {
            throw new System.NotImplementedException();
        }
    }
}
