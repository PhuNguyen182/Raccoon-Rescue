using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.IngameElements
{
    public class ScorePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private StarBar starBar;

        private int _maxScore = 0;

        public void SetScoreStreak(int tier1, int tier2, int tier3)
        {
            starBar.SetupStarBar(tier1, tier2, tier3);
        }

        public void SetScore(int score, int maxScore)
        {
            _maxScore = maxScore;
            scoreText.text = $"{score}";
            starBar.ShowScoreFill((float)score / maxScore);
        }
    }
}
