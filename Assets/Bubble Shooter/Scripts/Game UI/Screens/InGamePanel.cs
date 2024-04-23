using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.GameUI.IngameElements;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.Screens
{
    public class InGamePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text moveText;
        [SerializeField] private ScorePanel scorePanel;
        [SerializeField] private TargetHolder targetHolder;

        public Transform TargetPoint => targetHolder.TargetPoint;
        public TargetHolder TargetHolder => targetHolder;

        public void SetMoveCount(int move)
        {
            moveText.text = $"{move}";
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
    }
}
