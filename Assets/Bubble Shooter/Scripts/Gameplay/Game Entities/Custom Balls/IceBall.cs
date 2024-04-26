using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Messages;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class IceBall : BaseEntity, IBallHealth, IBallPhysics, IBreakable, IBallTransformation, IBallEffect
    {
        [SerializeField] private EntityType entityType;
        [SerializeField] private Animator ballAnimator;
        
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

        private int _hp = 0;
        private int _maxHp = 0;

        private bool _isEasyBreak;
        private bool _isMatchable;

        public override bool IsMatchable => _isMatchable;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public override EntityType EntityType => entityType;

        public int MaxHP => _maxHp;

        public bool EasyBreak => _isEasyBreak;

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public bool Break()
        {
            if(_hp > 0)
            {
                _hp = _hp - 1;
                _isEasyBreak = false;
                ballAnimator.enabled = false;
                
                int rand = Random.Range(1, 7); 
                EntityType color = (EntityType)rand;
                PlayBlastEffect();

                _isMatchable = true;
                entityType = color;
                TransformTo(color);

                return _hp <= 0;
            }

            return true;
        }

        public override void DestroyEntity()
        {
            PublishScore();
            SimplePool.Despawn(this.gameObject);
        }

        public override void InitMessages()
        {
            
        }

        public void SetMaxHP(int maxHP)
        {
            _maxHp = maxHP;
            _hp = _maxHp;
        }

        public override void ResetBall()
        {
            base.ResetBall();
            _isEasyBreak = true;
            _isMatchable = false;

            IsFixedOnStart = true;
            ballAnimator.enabled = true;

            entityType = EntityType.IceBall;
        }

        public void TransformTo(EntityType color)
        {
            Sprite toColor = color switch
            {
                EntityType.Blue => blue,
                EntityType.Green => green,
                EntityType.Orange => orange,
                EntityType.Red => red,
                EntityType.Violet => violet,
                EntityType.Yellow => yellow,
                _ => null
            };

            entityGraphics.SetEntitySprite(toColor);
        }

        public override void OnSnapped()
        {
            
        }

        public void ChangeLayerMask(bool isFixed)
        {
            
        }

        public void SetBodyActive(bool active)
        {
            ballMovement.SetBodyActive(active);
        }

        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            ballMovement.AddForce(force, forceMode);
        }

        public void PlayBlastEffect()
        {
            
        }
    }
}
