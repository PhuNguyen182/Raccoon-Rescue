using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.IngameElements
{
    public class TargetHolder : MonoBehaviour
    {
        [SerializeField] private Image targetImage;
        [SerializeField] private TMP_Text targetAmount;

        public Transform TargetPoint => targetImage.transform;

        public void UpdateTarget(int currentTarget, int requiredTarget)
        {
            targetAmount.text = $"{currentTarget}/{requiredTarget}";
            PlayTargetAnimation();
        }

        private void PlayTargetAnimation()
        {
            targetImage.DOKill();
            targetImage.transform
                       .DOPunchScale(Vector3.one * 0.3f, 0.15f, 1, 1)
                       .SetEase(Ease.InOutSine);
        }
    }
}
