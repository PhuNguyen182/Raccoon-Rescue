using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BubbleShooter.Scripts.Effects.BallEffects
{
    public class FlyTextEffect : MonoBehaviour
    {
        [SerializeField] private Canvas textCanvas;
        [SerializeField] private TMP_Text scoreText;

        private void Awake()
        {
            textCanvas.worldCamera = EffectManager.Instance.MainCamera;
        }

        public void SetScore(int score)
        {
            scoreText.text = $"{score}";
        }

        public void SetTextColor(Color color)
        {
            scoreText.color = color;
        }
    }
}
