using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Mainhome.GameManagers;
using BubbleShooter.Scripts.Mainhome.PopupBoxes.PlayGamePopup;
using BubbleShooter.Scripts.Common.PlayDatas;
using TMPro;

namespace BubbleShooter.Scripts.Mainhome.UI.TopComponents
{
    public class HeartBox : MonoBehaviour
    {
        [SerializeField] private TMP_Text heartCount;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Button heartButton;

        private const string PlayGamePopupPath = "Popups/Play Game Popup";

        public bool IsPause { get; set; }

        private void Awake()
        {
            heartButton.onClick.AddListener(OnHeartButton);
        }

        private void Update()
        {
            if (!IsPause)
            {
                UpdateHeart();
            }
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

        private void OnHeartButton()
        {
            int heart = GameData.Instance.GetHeart();

            if (heart > 0)
            {
                int star = 0;
                int currentLevel = GameData.Instance.GetCurrentLevel();
                if (GameData.Instance.IsLevelComplete(currentLevel))
                    star = GameData.Instance.GetLevelProgress(currentLevel).Star;

                var popup = PlayGamePopup.Create(PlayGamePopupPath);
                popup.SetLevelBoxData(new LevelBoxData
                {
                    Level = currentLevel,
                    Star = star
                });
            }

            else MainhomeController.Instance.ShowLifePopup();
        }
    }
}
