using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BubbleShooter.Scripts.Effects.BallEffects
{
    public class FlyTextEffect : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        public void SetScore(int score)
        {
            scoreText.text = $"{score}";
        }
    }
}
