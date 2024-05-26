using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace BubbleShooter.Scripts.Mainhome.Effects
{
    public class MovementUIObject : MonoBehaviour
    {
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

        public UniTask MoveTo(Vector3 toPosition)
        {
            _moveTween ??= CreateMoveTween(toPosition);
            _moveTween.ChangeStartValue(transform.position);
            _moveTween.ChangeEndValue(toPosition);
            _moveTween.Rewind();
            _moveTween.Play();

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

            return sequence.AwaitForComplete(TweenCancelBehaviour.Complete);
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
