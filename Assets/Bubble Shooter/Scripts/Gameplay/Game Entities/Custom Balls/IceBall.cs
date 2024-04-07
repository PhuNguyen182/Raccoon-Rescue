using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class IceBall : BaseEntity, IBallHealth, IBreakable, IBallTransformation
    {
        [SerializeField] private EntityType entityType;
        
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

        private bool _isMatchable;

        public override bool IsMatchable => _isMatchable;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public override EntityType EntityType => entityType;

        public int MaxHP => _maxHp;

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public bool Break()
        {
            if(_hp > 0)
            {
                _hp = _hp - 1;
                int rand = Random.Range(1, 7); // Get random color between blue, green, orange, red, violet and yellow
                EntityType color = (EntityType)rand;
                
                _isMatchable = true;
                entityType = color;
                TransformTo(color);

                return false;
            }

            return true;
        }

        public override void Destroy()
        {
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

        public override void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }

        public override void ResetBall()
        {
            base.ResetBall();
            _isMatchable = false;
            entityType = EntityType.IceBall;
            IsFixedOnStart = true;
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
    }
}
