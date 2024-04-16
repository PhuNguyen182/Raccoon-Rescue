using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleShooter.Scripts.GameUI.IngameElements
{
    public class StarBar : MonoBehaviour
    {
        [SerializeField] private Image starFill;
        [SerializeField] private StarStreak[] starStreaks;

        public void SetupStarBar(int tier1Score, int tier2Score, int tier3Score)
        {
            // To do: arrange star streaks' position relative to tiered scores
        }

        public void ShowScoreFill(float fillAmount)
        {
            starFill.fillAmount = fillAmount;
        }
    }
}
