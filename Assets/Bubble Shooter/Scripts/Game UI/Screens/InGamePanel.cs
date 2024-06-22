using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.GameUI.IngameElements;
using BubbleShooter.Scripts.Mainhome.UI.PopupBoxes;
using DG.Tweening;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.Screens
{
    public class InGamePanel : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private TMP_Text moveText;
        [SerializeField] private ScorePanel scorePanel;
        [SerializeField] private TargetHolder targetHolder;

        private const string SettingPopupPath = "Popups/Setting Popup";

        public Transform TargetPoint => targetHolder.TargetPoint;
        public TargetHolder TargetHolder => targetHolder;

        private void Awake()
        {
            pauseButton.onClick.AddListener(OpenSetting);
        }

        public void SetMoveCount(int move)
        {
            moveText.text = $"{move}";
            moveText.transform.DOPunchScale(Vector3.one * 0.3f, 0.15f, 1, 1)
                    .SetEase(Ease.InOutSine);
        }

        public void UpdateTarget(int target, int requiredTargets)
        {
            targetHolder.UpdateTarget(target, requiredTargets);
        }

        public void SetScoreStreak(int tier1, int tier2, int tier3)
        {
            scorePanel.SetScoreStreak(tier1, tier2, tier3);
        }

        public void SetScore(int score, int maxScore)
        {
            scorePanel.SetScore(score, maxScore);
        }

        private void OpenSetting()
        {
            SettingPopup.Create(SettingPopupPath).SetBackHomeButtonActive(true);
        }
    }
}
