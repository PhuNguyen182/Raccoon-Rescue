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
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private StarBar starBar;
        [SerializeField] private TargetHolder targetHolder;

        public void SetMoveCount(int move)
        {
            moveText.text = $"{move}";
        }

        public void SetScore(int score, int maxScore)
        {
            scoreText.text = $"{score}";
            starBar.ShowScoreFill((float)score / maxScore);
        }
    }
}
