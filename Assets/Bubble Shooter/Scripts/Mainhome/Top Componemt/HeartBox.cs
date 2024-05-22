using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BubbleShooter.Scripts.Mainhome.TopComponents
{
    public class HeartBox : MonoBehaviour
    {
        [SerializeField] private TMP_Text heartCount;
        [SerializeField] private TMP_Text timerText;

        public void UpdateHeart(int heart)
        {
            heartCount.text = $"{heart}";
        }

        public void UpdateTimer(TimeSpan time)
        {
            timerText.text = $"{time.Minutes:D2}:{time.Seconds:D2}";
        }
    }
}
