using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BubbleShooter.Scripts.Mainhome.ProgressMaps
{
    public class LevelNodePath : MonoBehaviour
    {
        [SerializeField] private int level;

        [Header("Node Graphics")]
        [SerializeField] private Image nodeImage;
        [SerializeField] private Sprite availableState;
        [SerializeField] private Sprite notAvailableState;
        [SerializeField] private Animator nodeAnimator;

        [Header("UI Elements")]
        [SerializeField] private Button nodeButton;
        [SerializeField] private TMP_Text levelText;

        private readonly int _starIdleHash = Animator.StringToHash("StarIdle");
        private readonly int _starCompleteHash = Animator.StringToHash("StarComplete");

        private int _star = 0;
        private bool _isAvailable;

        public int Level => level;

        public Observable<(int Level, int Star)> OnClickObservable 
            => nodeButton.OnClickAsObservable()
                         .Where(unit => _isAvailable)
                         .Select(unit => (level, _star));

        public void SetAvailableState(bool isAvailable)
        {
            _isAvailable = isAvailable;
            nodeImage.sprite = isAvailable ? availableState : notAvailableState;
        }

        public void SetIdleState(int star, bool isRecentComplete)
        {
            _star = star;
            nodeAnimator.SetInteger(!isRecentComplete ? _starIdleHash : _starCompleteHash, star);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetLevelText();
        }
#endif

        public void SetLevelText()
        {
            levelText.text = $"{level}";
        }
    }
}
