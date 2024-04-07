using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public abstract class BaseEntity : BaseBallEntity, IBallEntity
    {
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

        public virtual void ResetBall() { }

        public abstract void InitMessages();

        public abstract UniTask Blast();

        public abstract void Destroy();

        public abstract void SetWorldPosition(Vector3 position);

        public abstract void OnSnapped();
    }
}
