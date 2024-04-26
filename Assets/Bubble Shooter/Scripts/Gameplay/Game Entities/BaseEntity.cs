using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Constants;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using MessagePipe;
using BubbleShooter.Scripts.Common.Messages;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public abstract class BaseEntity : BaseBallEntity, IBallEntity, IBallGraphics
    {
        [SerializeField] protected LayerMask destroyerLayer;
        [SerializeField] protected BallMovement ballMovement;
        [SerializeField] protected EntityGraphics entityGraphics;
        [SerializeField] protected EntityAudio entityAudio;

        protected CancellationToken onDestroyToken;

        #region Common primary messages
        protected IPublisher<PublishScoreMessage> _addScorePublisher;
        protected IPublisher<BallDestroyMessage> _ballDestroyMessage;
        #endregion

        public int Score => 10;

        public int ScoreMultiplier { get; set; }

        public bool IsEndOfGame { get; set; }

        public bool IsFallen { get; set; }
        
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if((destroyerLayer.value & (1 << collision.gameObject.layer)) > 0)
            {
                if(IsFallen)
                    DestroyOnFallen();
            }
        }

        protected virtual void OnDisable()
        {
            
        }
    }
}
