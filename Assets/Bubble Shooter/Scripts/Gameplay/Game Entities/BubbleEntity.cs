using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public abstract class BubbleEntity : BaseEntity
    {
        [SerializeField] protected BallMovement ballMovement;
        [SerializeField] protected EntityGraphics entityGraphics;

        protected CancellationToken onDestroyToken;

        private void Awake()
        {
            onDestroyToken = this.GetCancellationTokenOnDestroy();
            OnAwake();
        }

        protected virtual void OnAwake() { }

        public virtual void ResetBall() { }

        public abstract void InitMessages();
    }
}
