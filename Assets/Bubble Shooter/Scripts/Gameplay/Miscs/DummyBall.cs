using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class DummyBall : MonoBehaviour
    {
        [Header("Swap Move")]
        [SerializeField] private float swapDuration = 0.35f;
        [SerializeField] private Ease swapEase = Ease.OutQuad;

        private Tweener _swapTween;

        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        public UniTask SwapTo(Vector3 toPosition)
        {
            _swapTween ??= CreateSwapTween(toPosition);
            _swapTween.ChangeStartValue(transform.position);
            _swapTween.ChangeEndValue(toPosition);
            _swapTween.Rewind();
            _swapTween.Play();

            return UniTask.Delay(TimeSpan.FromSeconds(_swapTween.Duration()), cancellationToken: _token);
        }

        private Tweener CreateSwapTween(Vector3 toPosition)
        {
            return transform.DOMove(toPosition, swapDuration).SetEase(swapEase).SetAutoKill(false);
        }

        private void OnDestroy()
        {
            _swapTween?.Kill();
        }
    }
}
