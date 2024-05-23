using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Common.Configs;
using BubbleShooter.Scripts.Mainhome.PopupBoxes;
using BubbleShooter.Scripts.Mainhome.ProgressMaps;
using BubbleShooter.Scripts.Mainhome.PopupBoxes.PlayGamePopup;
using BubbleShooter.Scripts.Mainhome.UI.TopComponents;
using BubbleShooter.Scripts.Mainhome.Handlers;
using BubbleShooter.Scripts.Common.Features.Shop;
using Cysharp.Threading.Tasks;
using Scripts.Configs;

namespace BubbleShooter.Scripts.Mainhome
{
    public class MainhomeController : MonoBehaviour
    {
        [SerializeField] private Button settingButton;
        [SerializeField] private CoinHolder coinHolder;
        [SerializeField] private ProgressMap progressMap;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private InteractiveController interactiveController;

        private const string ShopPanelPath = "Popups/Shop";
        private const string PlayGamePopupPath = "Popups/Play Game Popup";
        private const string SettingPopupPath = "Popups/Setting Popup";

        private CancellationToken _token;

        public static MainhomeController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _token = this.GetCancellationTokenOnDestroy();

            RegisterButtons();
        }

        private void Start()
        {
            PreloadPopups();
            SetInteractive(true);

            BackHomeConfig.Current = new BackHomeConfig
            {
                Level = 2 - 1, // Path node index is started from 0
                Star = 3
            };

            OnStartMainhome();
        }

        private void PreloadPopups()
        {
            ShopPanel.Preload(ShopPanelPath);
            PlayGamePopup.Preload(PlayGamePopupPath);
            SettingPopup.Preload(SettingPopupPath);
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
            SettingPopup.Create(SettingPopupPath).SetBackHomeButtonActive(false);
        }

        private void SetupScene()
        {
            Application.targetFrameRate = GameSetupConstants.NormalTargetFramerate;
        }

        public void ShowShopPanel()
        {
            ShopPanel.Create(ShopPanelPath);
        }

        public void SetInteractive(bool interactable)
        {
            interactiveController.SetInteractive(interactable);
        }

        public void SetProgressMap(ProgressMap progressMap)
        {
            if(this.progressMap != null)
                Destroy(this.progressMap); // Use addressable instead

            this.progressMap = progressMap;
        }
    }
}
