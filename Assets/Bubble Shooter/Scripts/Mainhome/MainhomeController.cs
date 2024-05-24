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
using BubbleShooter.Scripts.Common.Databases;
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
        [SerializeField] private LevelStreakData levelStreakData;

        private const string ShopPanelPath = "Popups/Shop";
        private const string PlayGamePopupPath = "Popups/Play Game Popup";
        private const string SettingPopupPath = "Popups/Setting Popup";
        private const string LifePopupPath = "Popups/Life Popup";

        private CancellationToken _token;

        public LevelPlayInfo LevelPlayInfo { get; private set; }
        public static MainhomeController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _token = this.GetCancellationTokenOnDestroy();

            RegisterButtons();
            CreateHandlers();
        }

        private void Start()
        {
            SetupScene();
            PreloadPopups();
            SetInteractive(true);
            OnStartMainhome();
        }

        private void PreloadPopups()
        {
            ShopPanel.Preload(ShopPanelPath);
            PlayGamePopup.Preload(PlayGamePopupPath);
            SettingPopup.Preload(SettingPopupPath);
            LifePopup.Preload(LifePopupPath);
        }

        private void OnStartMainhome()
        {
            if (BackHomeConfig.Current != null)
            {
                if(BackHomeConfig.Current.IsWin)
                    OnBackHome().Forget();
                
                else
                    ShowCurrentMainhome();
            }

            else ShowCurrentMainhome();
            BackHomeConfig.Current = null;
        }

        private void ShowCurrentMainhome()
        {
            int level = GameData.Instance.GetCurrentLevel();
            LevelNodePath levelNode = progressMap.GetLevelNode(level);
            cameraController.TranslateTo(levelNode.transform.position);
            levelNode.SetIdleState(0, false);
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
        }

        private void CreateHandlers()
        {
            LevelPlayInfo = new(levelStreakData);
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

        public void ShowLifePopup()
        {
            LifePopup.Create(LifePopupPath);
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
