using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Common.Tutorials
{
    public abstract class BaseTutorial : MonoBehaviour, ITutorial
    {
        [SerializeField] protected Animator tutorialAnimator;

        protected int step = 0;
        protected CancellationToken destroyToken;

        private void Awake()
        {
            destroyToken = this.GetCancellationTokenOnDestroy();
            OnAwake();
        }

        private void OnEnable()
        {
            OnAppear();
        }

        protected virtual void OnAwake() { }

        protected virtual void OnAppear() { }

        protected virtual void OnDisappear() { }

        public abstract void DoNextStep();

        public abstract UniTask Hide();

        public abstract UniTask Show();

        protected virtual void DoClose()
        {
            SimplePool.Despawn(gameObject);
        }

        protected void Close()
        {
            DoClose();
        }

        private void OnDisable()
        {
            OnDisappear();
        }
    }
}
