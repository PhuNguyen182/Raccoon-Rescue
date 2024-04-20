using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Effects.Tweens;

namespace BubbleShooter.Scripts.GameUI.IngameElements
{
    public class StarBar : MonoBehaviour
    {
        [SerializeField] private Image starFill;
        [SerializeField] private StarStreak[] starStreaks;
        [SerializeField] private TweenValueEffect starTween;

        private float _streak1 = 0;
        private float _streak2 = 0;
        private float _streak3 = 0;

        private ReactiveProperty<float> _reactiveStarFill = new(0);

        private void Start()
        {
            starTween.BindFloat(_reactiveStarFill, StarFill);
            _reactiveStarFill.Value = 0;
        }

        public void SetupStarBar(int tier1Score, int tier2Score, int tier3Score, int maxScore)
        {
            _streak1 = (float)tier1Score / maxScore;
            _streak2 = (float)tier2Score / maxScore;
            _streak3 = (float)tier3Score / maxScore;

            starStreaks[0].SetStarActive(false);
            starStreaks[0].SetHorizontalPosition(_streak1 * 299.5f);
            starStreaks[1].SetStarActive(false);
            starStreaks[1].SetHorizontalPosition(_streak2 * 299.5f);
            starStreaks[2].SetStarActive(false);
            starStreaks[2].SetHorizontalPosition(_streak3 * 299.5f);
        }

        public void ShowScoreFill(float fillAmount)
        {
            _reactiveStarFill.Value = fillAmount;

            if(fillAmount >= _streak3)
                starStreaks[2].SetStarActive(true);

            else if(fillAmount >= _streak2)
                starStreaks[1].SetStarActive(true);

            else if(fillAmount >= _streak1)
                starStreaks[0].SetStarActive(true);
        }

        private void StarFill(float fillAmount)
        {
            starFill.fillAmount = fillAmount;
        }
    }
}
