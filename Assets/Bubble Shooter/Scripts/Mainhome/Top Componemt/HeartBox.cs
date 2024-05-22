using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Mainhome.GameManagers;
using TMPro;

namespace BubbleShooter.Scripts.Mainhome.TopComponents
{
    public class HeartBox : MonoBehaviour
    {
        [SerializeField] private TMP_Text heartCount;
        [SerializeField] private TMP_Text timerText;

        private void Update()
        {
            UpdateHeart();
        }

        public void UpdateHeart()
        {
            int heart = GameData.Instance.GetHeart();
            heartCount.text = $"{heart}";
            TimeSpan time = GameManager.Instance.HeartTime.HeartTimeDiff;
            timerText.text = heart != GameDataConstants.MaxHeart 
                             ? $"{time.Minutes:D2}:{time.Seconds:D2}" 
                             : "FULL";
        }
    }
}
