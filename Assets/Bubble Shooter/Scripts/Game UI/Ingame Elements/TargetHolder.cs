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
        [SerializeField] private Animator animator;

        private readonly int _bouncingHash = Animator.StringToHash("Bounce");

        public Transform TargetPoint => targetImage.transform;

        public void UpdateTarget(int currentTarget, int requiredTarget)
        {
            targetAmount.text = $"{currentTarget}/{requiredTarget}";
        }

        public void PlayTargetAnimation()
        {
            animator.SetTrigger(_bouncingHash);
        }
    }
}
