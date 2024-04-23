using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class FlyToTargetObject : MonoBehaviour
    {
        [SerializeField] private MoveAnimationUtils moveAnimation;

        [Header("Move To Target")]
        [SerializeField] private Ease easeX;
        [SerializeField] private Ease easeY;
        [SerializeField] private Ease easeScale;

        private Sequence _moveToTargetSequence;

        public async UniTask MoveToTarget(Vector3 targetPosition, float duration)
        {
            Vector3 toPosition = targetPosition;
            _moveToTargetSequence ??= moveAnimation.CreateMoveToTargetTween(toPosition, 0.75f, duration, easeX, easeY, easeScale);

            await _moveToTargetSequence.AwaitForComplete(TweenCancelBehaviour.KillWithCompleteCallbackAndCancelAwait);
        }

        private void OnDestroy()
        {
            _moveToTargetSequence?.Kill();
        }
    }
}
