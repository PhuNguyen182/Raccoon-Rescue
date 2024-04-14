using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Constants;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public abstract class BaseEntity : BaseBallEntity, IBallEntity, IBallGraphics
    {
        [SerializeField] protected LayerMask destroyerLayer;
        [SerializeField] protected BallMovement ballMovement;
        [SerializeField] protected EntityGraphics entityGraphics;
        [SerializeField] protected EntityAudio entityAudio;

        protected CancellationToken onDestroyToken;

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

        public virtual void ResetBall() 
        {
            ChangeLayer(BallConstants.NormalLayer);
            ballMovement.MovementState = BallMovementState.Fixed;
        }

        public abstract void InitMessages();

        public abstract UniTask Blast();

        public abstract void DestroyEntity();

        public abstract void SetWorldPosition(Vector3 position);

        public abstract void OnSnapped();

        public void ChangeLayer(string layerName)
        {
            entityGraphics.ChangeLayer(layerName);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if((destroyerLayer.value & (1 << collision.gameObject.layer)) > 0)
            {
                DestroyEntity();
            }
        }
    }
}
