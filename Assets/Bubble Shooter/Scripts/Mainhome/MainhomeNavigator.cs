using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Common.Configs;
using BubbleShooter.Scripts.Mainhome.PopupBoxes;
using BubbleShooter.Scripts.Mainhome.ProgressMap;
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
        [SerializeField] private ProgressMapManager progressMap;
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

        }

        private async UniTask OnBackHome()
        {
            int level = BackHomeConfig.Current.Level;
            int star = BackHomeConfig.Current.Star;

            LevelNodePath levelNode = progressMap.GetLevelNode(level);
            cameraController.TranslateTo(levelNode.transform.position);
            progressMap.Translate(level);

            await progressMap.Move(level - 1, level);
            levelNode.SetIdleState(star, true);

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
