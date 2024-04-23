using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class MoveAnimationUtils : MonoBehaviour
    {
        public Sequence CreateMoveToTargetTween(Vector3 toPosition, float toScale, float duration, Ease easeX, Ease easeY, Ease scaleEase)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Insert(0, transform.DOMoveX(toPosition.x, duration).SetEase(easeX));
            sequence.Insert(0, transform.DOMoveY(toPosition.y, duration).SetEase(easeY));
            sequence.Insert(0, transform.DOScale(toScale, duration).SetEase(scaleEase));
            sequence.SetAutoKill(false);

            return sequence;
        }
    }
}
