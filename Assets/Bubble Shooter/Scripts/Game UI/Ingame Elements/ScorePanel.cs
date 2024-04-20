using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Effects.Tweens;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.IngameElements
{
    public class ScorePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private StarBar starBar;
        [SerializeField] private TweenValueEffect scoreTween;

        private int _maxScore = 0;
        private ReactiveProperty<int> _reactiveScore = new();

        private void Awake()
        {
            scoreTween.BindInt(_reactiveScore, UpdateScore);
            _reactiveScore.Value = 0;
        }

        // This function is called first
        public void SetScore(int score, int maxScore)
        {
            _maxScore = maxScore;
            _reactiveScore.Value = score;
            starBar.ShowScoreFill((float)score / maxScore);
        }

        // This function is called second
        public void SetScoreStreak(int tier1, int tier2, int tier3)
        {
            starBar.SetupStarBar(tier1, tier2, tier3, _maxScore);
        }

        private void UpdateScore(int score)
        {
            scoreText.text = $"{score}";
        }
    }
}
