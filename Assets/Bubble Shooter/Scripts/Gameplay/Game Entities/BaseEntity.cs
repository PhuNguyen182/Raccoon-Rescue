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

        private const string CeilLayerMask = "Ceil";

        private int _ceilLayerMask = 0;
        protected CancellationToken onDestroyToken;

        public bool IsCeilAttached { get; set; }
        public abstract bool IsMatchable { get; }

        public abstract bool IsFixedOnStart { get; set; }

        public abstract Vector3 WorldPosition { get; }

        public abstract Vector3Int GridPosition { get; set; }

        public abstract EntityType EntityType { get; }

        private void Awake()
        {
            _ceilLayerMask = LayerMask.NameToLayer(CeilLayerMask);
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
            ballMovement.MovementState = BallMovementState.Fixed;
        }

        public abstract void InitMessages();

        public abstract UniTask Blast();

        public abstract void Destroy();

        public abstract void SetWorldPosition(Vector3 position);

        public abstract void OnSnapped();

        public void CheckCeilAttach()
        {
            Collider2D ceilCollider = Physics2D.OverlapCircle(transform.position, 0.3f, _ceilLayerMask);
            IsCeilAttached = ceilCollider != null;
        }
    }
}
