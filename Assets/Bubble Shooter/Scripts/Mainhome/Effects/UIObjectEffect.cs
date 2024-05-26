using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

namespace BubbleShooter.Scripts.Mainhome.Effects
{
    public class UIObjectEffect : MonoBehaviour
    {
        [SerializeField] private Image objectImage;

        [Header("Linear Move")]
        [SerializeField] private Ease moveEase;
        [SerializeField] private float moveDuration;

        [Header("Separated Move")]
        [SerializeField] private Ease separatedMoveEaseX;
        [SerializeField] private Ease separatedMoveEaseY;
        [SerializeField] private float separatedMoveDuration;

        private Tweener _moveTween;

        private CancellationToken _destroyToken;

        private void Awake()
        {
            _destroyToken = this.GetCancellationTokenOnDestroy();
        }

        public void SetFade(float fade)
        {
            Color c = objectImage.color;
            c.a = fade;
            objectImage.color = c;
        }

        public UniTask FadeIn(float duration)
        {
            return objectImage.DOFade(1, duration).SetEase(Ease.InOutSine).ToUniTask();
        }

        public UniTask FadeOut(float duration)
        {
            return objectImage.DOFade(0, duration).SetEase(Ease.InOutSine).ToUniTask();
        }

        public UniTask MoveTo(Vector3 toPosition, Action onComplete = null)
        {
            _moveTween ??= CreateMoveTween(toPosition);
            _moveTween.ChangeStartValue(transform.position);
            _moveTween.ChangeEndValue(toPosition);
            _moveTween.Rewind();
            _moveTween.Play();

            _moveTween.OnComplete(() => onComplete?.Invoke());
            return UniTask.Delay(TimeSpan.FromSeconds(_moveTween.Duration())
                                , cancellationToken: _destroyToken);
        }

        public UniTask MoveSeparatedWithScale(Vector3 toPosition, float toScale)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Insert(0, transform.DOScale(toScale, separatedMoveDuration).SetEase(Ease.InOutSine));
            sequence.Insert(0, transform.DOMoveX(toPosition.x, separatedMoveDuration).SetEase(separatedMoveEaseX));
            sequence.Insert(0, transform.DOMoveY(toPosition.y, separatedMoveDuration).SetEase(separatedMoveEaseY));

            sequence.SetAutoKill(false);
            sequence.Rewind();
            sequence.PlayForward();

            return UniTask.Delay(TimeSpan.FromSeconds(sequence.Duration())
                                , cancellationToken: _destroyToken);
        }

        public UniTask MoveSeparated(Vector3 toPosition)
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.Insert(0, transform.DOMoveX(toPosition.x, separatedMoveDuration).SetEase(separatedMoveEaseX));
            sequence.Insert(0, transform.DOMoveY(toPosition.y, separatedMoveDuration).SetEase(separatedMoveEaseY));
            
            sequence.SetAutoKill(false);
            sequence.Rewind();
            
            return sequence.AwaitForComplete(TweenCancelBehaviour.Complete);
        }

        private Tweener CreateMoveTween(Vector3 toPosition)
        {
            return transform.DOMove(toPosition, moveDuration).SetEase(moveEase).SetAutoKill(false);
        }

        private void OnDestroy()
        {
            _moveTween?.Kill();
        }
    }
}
