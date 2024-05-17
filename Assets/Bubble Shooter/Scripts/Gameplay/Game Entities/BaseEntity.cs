using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Constants;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Effects;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public abstract class BaseEntity : BaseBallEntity, IBallEntity, IBallGraphics, IBallEffect, IBallPlayBoosterEffect
    {
        [SerializeField] protected LayerMask destroyerLayer;
        [SerializeField] protected BallMovement ballMovement;
        [SerializeField] protected EntityGraphics entityGraphics;
        [SerializeField] protected EntityAudio entityAudio;

        [Header("Booster Effects")]
        [FoldoutGroup("Booster Effects")]
        [SerializeField] private GameObject leafEffect;
        [FoldoutGroup("Booster Effects")]
        [SerializeField] private ParticleSystem waterEffect;

        protected CancellationToken onDestroyToken;

        #region Common primary messages
        protected IPublisher<PublishScoreMessage> _addScorePublisher;
        protected IPublisher<BallDestroyMessage> _ballDestroyMessage;
        #endregion

        private GameObject _leafEffect;

        public int Score => 10;

        public int ScoreMultiplier { get; set; }

        public bool IsEndOfGame { get; set; }

        public bool IsFallen { get; set; }
        
        public bool RippleIgnore { get; set; }
        
        public abstract bool IsMatchable { get; }

        public abstract bool IsFixedOnStart { get; set; }

        public abstract Vector3 WorldPosition { get; }

        public abstract Vector3Int GridPosition { get; set; }

        public abstract EntityType EntityType { get; }

        private void Awake()
        {
            onDestroyToken = this.GetCancellationTokenOnDestroy();
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        protected virtual void OnAwake() { }

        protected virtual void OnStart() { }

        public abstract void InitMessages();

        public abstract UniTask Blast();

        public abstract void DestroyEntity();

        public abstract void OnSnapped();

        public virtual void ResetBall() 
        {
            IsFallen = false;
            ScoreMultiplier = 1;

            ChangeLayer(BallConstants.NormalLayer);
            InitPrimaryMessages();
            
            ballMovement.MovementState = BallMovementState.Fixed;
        }

        public void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void ChangeLayer(string layerName)
        {
            entityGraphics.ChangeLayer(layerName);
        }

        protected void InitPrimaryMessages()
        {
            _addScorePublisher = GlobalMessagePipe.GetPublisher<PublishScoreMessage>();
            _ballDestroyMessage = GlobalMessagePipe.GetPublisher<BallDestroyMessage>();
        }

        public void PublishScore()
        {
            _addScorePublisher.Publish(new PublishScoreMessage { Score = Score * ScoreMultiplier });
        }

        public void OnFallenDestroy()
        {
            _ballDestroyMessage.Publish(new BallDestroyMessage
            {
                IsEndOfGame = IsEndOfGame
            });
        }

        protected void DestroyOnFallen()
        {
            ScoreMultiplier = 1;

            OnFallenDestroy();
            PublishScore();
            DestroyEntity();
        }

        public void SetWorldToGridFunction(Func<Vector3, Vector3Int> function)
        {
            ballMovement.WorldToGridFunction = function;
        }

        public void SetTakeGridCell(Func<Vector3Int, IGridCell> function)
        {
            ballMovement.TakeGridCellFunction = function;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if((destroyerLayer.value & (1 << collision.gameObject.layer)) > 0)
            {
                if (IsFallen)
                {
                    PlayBlastEffect(true);
                    DestroyOnFallen();
                }
            }
        }

        public abstract void PlayBlastEffect(bool isFallen);

        public abstract void ToggleEffect(bool active);

        public abstract void PlayColorfulEffect();

        public virtual async UniTask PlayBoosterEffect(EntityType booster)
        {
            switch (booster)
            {
                case EntityType.FireBall:
                    await UniTask.CompletedTask;
                    break;
                case EntityType.WaterBall:
                    EffectManager.Instance.SpawnBoosterEffect(EntityType.WaterBall, transform.position, Quaternion.identity);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: destroyCancellationToken);
                    break;
                case EntityType.SunBall:
                    await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: destroyCancellationToken);
                    break;
                case EntityType.LeafBall:
                    _leafEffect = SimplePool.Spawn(leafEffect, transform, transform.position, Quaternion.identity);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: destroyCancellationToken);
                    break;
            }
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

        protected virtual void OnDisable()
        {
            
        }
    }
}
