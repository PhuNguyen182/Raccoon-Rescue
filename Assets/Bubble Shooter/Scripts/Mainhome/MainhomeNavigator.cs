using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Common.Configs;
using BubbleShooter.Scripts.Mainhome.PopupBoxes;
using BubbleShooter.Scripts.Mainhome.ProgressMaps;
using BubbleShooter.Scripts.Mainhome.TopComponents;
using BubbleShooter.Scripts.Mainhome.Handlers;
using BubbleShooter.Scripts.Mainhome.Shop;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Mainhome
{
    public class MainhomeNavigator : MonoBehaviour
    {
        [SerializeField] private Button settingButton;
        [SerializeField] private CoinHolder coinHolder;
        [SerializeField] private ProgressMap progressMap;
        [SerializeField] private CameraController cameraController;

        private const string ShopPanelPath = "Popups/Shop";
        private const string SettingPopupPath = "Popups/Setting Popup";

        private CancellationToken _token;

        public static MainhomeNavigator Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _token = this.GetCancellationTokenOnDestroy();

            RegisterButtons();
        }

        private void Start()
        {
            BackHomeConfig.Current = new BackHomeConfig
            {
                Level = 2 - 1, // Path node index is started from 0
                Star = 3
            };

            OnStartMainhome();
        }

        private void OnStartMainhome()
        {
            if (BackHomeConfig.Current != null)
                OnBackHome().Forget();
            else
                ShowCurrentMainhome();
        }

        private void ShowCurrentMainhome()
        {
            int level = 1; // test level
            int star = 2; // test star
            LevelNodePath levelNode = progressMap.GetLevelNode(level);
            cameraController.TranslateTo(levelNode.transform.position);
            levelNode.SetIdleState(star, false);
        }

        private async UniTask OnBackHome()
        {
            int level = BackHomeConfig.Current.Level;
            int star = BackHomeConfig.Current.Star;

            LevelNodePath levelNode = progressMap.GetLevelNode(level);
            cameraController.TranslateTo(levelNode.transform.position);

            levelNode.SetIdleState(star, true);
            progressMap.Translate(level - 1);

            await UniTask.Delay(TimeSpan.FromSeconds(1.75f), cancellationToken: _token);
            await progressMap.Move(level - 1, level);
            BackHomeConfig.Current = null;
        }

        private void RegisterButtons()
        {
            settingButton.onClick.AddListener(ShowSettingPopup);
        }

        private void ShowSettingPopup()
        {
            SettingPopup.Create(SettingPopupPath);
        }

        public void ShowShopPanel()
        {
            ShopPanel.Create(ShopPanelPath);
        }
    }
}
