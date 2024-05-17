using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Effects.BallEffects;
using BubbleShooter.Scripts.Effects;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls
{
    public class IceBall : BaseEntity, IBallHealth, IBallPhysics, IBreakable, IBallTransformation, IBallMovement
    {
        [SerializeField] private Color textColor;
        [SerializeField] private EntityType entityType;
        [SerializeField] private Animator ballAnimator;
        [SerializeField] private GameObject iceBlink;
        [SerializeField] private ParticleSystem iceBreak;
        
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

        private int _hp = 0;
        private int _maxHp = 0;

        private bool _isEasyBreak;
        private bool _isMatchable;

        private Color _textColor;

        public override bool IsMatchable => _isMatchable;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public override EntityType EntityType => entityType;

        public int MaxHP => _maxHp;

        public bool EasyBreak => _isEasyBreak;

        public bool CanMove 
        { 
            get => ballMovement.CanMove; 
            set => ballMovement.CanMove = value; 
        }

        public Vector2 MoveDirection => ballMovement.MoveDirection;

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
                PlayIceBreak();

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

        public override void InitMessages() { }

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

            _textColor = textColor;
            entityType = EntityType.IceBall;
            ToggleEffect(true);
        }

        public void TransformTo(EntityType color)
        {
            (Sprite toColor, Color scoreColor) = color switch
            {
                EntityType.Blue => (blue, blueColor),
                EntityType.Green => (green, greenColor),
                EntityType.Orange => (orange, orangeColor),
                EntityType.Red => (red, redColor),
                EntityType.Violet => (violet, violetColor),
                EntityType.Yellow => (yellow, yellowColor),
                _ => (null, Color.black)
            };

            _textColor = scoreColor;
            entityGraphics.SetEntitySprite(toColor);
            ToggleEffect(false);
        }

        public override void OnSnapped() { }

        public void ChangeLayerMask(bool isFixed) { }

        public void SetBodyActive(bool active)
        {
            ballMovement.SetBodyActive(active);
        }

        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            ballMovement.AddForce(force, forceMode);
        }

        public override void PlayBlastEffect(bool isFallen)
        {
            EffectManager.Instance.SpawnBallPopEffect(transform.position, Quaternion.identity);
            FlyTextEffect flyText = EffectManager.Instance.SpawnFlyText(transform.position, Quaternion.identity);
            flyText.SetScore(Score);
            flyText.SetTextColor(_textColor);
        }

        public void SetMoveDirection(Vector2 direction) { }

        public UniTask SnapTo(Vector3 position)
        {
            return ballMovement.SnapTo(position);
        }

        public UniTask BounceMove(Vector3 position)
        {
            return ballMovement.BounceMove(position);
        }

        public override void ToggleEffect(bool active)
        {
            if (iceBlink != null)
                iceBlink.SetActive(active);
        }

        public override void PlayColorfulEffect()
        {
            EffectManager.Instance.SpawnColorfulEffect(transform.position, Quaternion.identity);
        }

        private void PlayIceBreak()
        {
            if (iceBreak != null)
                SimplePool.Spawn(iceBreak, EffectContainer.Transform, transform.position, Quaternion.identity);
        }
    }
}
