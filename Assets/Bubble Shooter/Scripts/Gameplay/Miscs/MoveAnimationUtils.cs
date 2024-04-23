using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class MoveAnimationUtils : MonoBehaviour
    {
        public Sequence CreateMoveToTargetTween(Vector3 toPosition, float toScale1, float toScale2, float duration, Ease moveUpEase, Ease easeX, Ease easeY, Ease scaleEase)
        {
            Sequence sequence = DOTween.Sequence();
            Vector3 upPos = transform.position + Vector3.up * 1f;
            Tween moveUpTween = transform.DOMove(upPos, duration * 0.6f).SetEase(moveUpEase);
            Tween scaleTween1 = transform.DOScale(Vector3.one * toScale1, duration * 0.6f).SetEase(scaleEase);

            Tween scaleTween2 = transform.DOScale(Vector3.one * toScale2, duration * 0.8f).SetEase(scaleEase);
            Tween moveXTween = transform.DOMoveX(toPosition.x, duration * 0.8f).SetEase(easeX);
            Tween moveYTween = transform.DOMoveY(toPosition.y, duration * 0.8f).SetEase(easeY);

            sequence.Insert(0, moveUpTween);
            sequence.Insert(0, scaleTween1);
            sequence.Insert(duration * 0.5f, scaleTween2);
            sequence.Insert(duration * 0.5f, moveXTween);
            sequence.Insert(duration * 0.5f, moveYTween);
            sequence.SetAutoKill(false);

            return sequence;
        }
    }
}
