using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

namespace BubbleShooter.Scripts.Mainhome.ProgressMap
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

        private static readonly int _starIdleHash = Animator.StringToHash("StarIdle");
        private static readonly int _starCompleteHash = Animator.StringToHash("StarComplete");

        public int Level => level;

        private void Awake()
        {
            nodeButton.onClick.AddListener(OnPlayLevelClick);
        }

        public void SetAvailableState(bool isAvailable)
        {
            nodeImage.sprite = isAvailable ? availableState : notAvailableState;
        }

        public void SetIdleState(int star, bool isRecentComplete)
        {
            nodeAnimator.SetInteger(!isRecentComplete ? _starIdleHash : _starCompleteHash, star);
        }

        [Button]
        public void SetLevelText()
        {
            levelText.text = $"{level}";
        }

        private void OnPlayLevelClick()
        {

        }
    }
}
