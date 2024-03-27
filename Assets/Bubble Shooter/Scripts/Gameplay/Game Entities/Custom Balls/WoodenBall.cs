using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class WoodenBall : BaseBall, IFixedUpdateHandler, IBallMovement, IBallHealth
    {
        [Header("Health Sprites")]
        [SerializeField] private Sprite hp2;
        [SerializeField] private Sprite hp1;

        private int _hp = 0;
        private int _maxHp = 0;

        public bool CanMove { get => false; set { } }
        public override EntityType EntityType => EntityType.WoodenBall;

        public int MaxHP => _maxHp;

        public override void InitMessages()
        {
            
        }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public override bool Break()
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
            if (_hp == 2)
                entityGraphics.SetEntitySprite(hp2);
            else if(_hp == 1)
                entityGraphics.SetEntitySprite(hp1);
        }
    }
}
