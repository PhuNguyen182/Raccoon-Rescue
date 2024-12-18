using R3;
using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.PlayDatas;
using BubbleShooter.Scripts.Common.Configs;
using BubbleShooter.Scripts.Gameplay.Models;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using Scripts.SceneUtils;
using Newtonsoft.Json;
using TMPro;
using Dreamteck.Splines.Primitives;

namespace BubbleShooter.Scripts.Mainhome.UI.PopupBoxes.PlayGamePopup
{
    public class PlayGamePopup : BaseBox<PlayGamePopup>
    {
        [SerializeField] private AudioClip alertClip;
        [SerializeField] private Animator boxAnimator;
        [SerializeField] private GameObject[] stars;

        [Header("Buttons")]
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button playButton;

        [Header("Booster Toggles")]
        [SerializeField] private BoosterToggle colorfulBooster;
        [SerializeField] private BoosterToggle aimingBooster;
        [SerializeField] private BoosterToggle extraBallBooster;

        [Header("Texts")]
        [SerializeField] private TMP_Text levelText;

        private readonly int _disappearHash = Animator.StringToHash("Disappear");

        private int _level;
        private int _star;

        private bool _useColorful;
        private bool _useAiming;
        private bool _useExtraBall;
        private bool _isPlayPressed;

        private CancellationToken _token;

        protected override void OnAwake()
        {
            _token = this.GetCancellationTokenOnDestroy();

            RegisterButtons();
            InitBoosterToggle();
        }

        private void RegisterButtons()
        {
            playButton.onClick.AddListener(OnPlayGameButton);
            closeButton.onClick.AddListener(Close);
            backgroundButton.onClick.AddListener(Close);
        }

        private void InitBoosterToggle()
        {
            DisposableBuilder builder = Disposable.CreateBuilder();
            
            colorfulBooster.OnClickObservable
                           .Subscribe(value => AddBooster(value, IngameBoosterType.Colorful))
                           .AddTo(ref builder);
            
            aimingBooster.OnClickObservable
                         .Subscribe(value => AddBooster(value, IngameBoosterType.PreciseAimer))
                         .AddTo(ref builder);

            extraBallBooster.OnClickObservable
                            .Subscribe(value => AddBooster(value, IngameBoosterType.ChangeBall))
                            .AddTo(ref builder);
            
            builder.RegisterTo(_token);
        }

        private void OnPlayGameButton()
        {
            int heart = GameData.Instance.GetHeart();
            UniTask onPlayTask = heart > 0 ? PlayGame() 
                                 : CloseAndShowLifePopup();
            onPlayTask.Forget();
        }

        private async UniTask PlayGame()
        {
            _isPlayPressed = true;
            LevelModel levelModel;
            int lives = GameData.Instance.GetHeart();

            string levelData = await MainhomeController.Instance.LevelPlayInfo.GetLevelData(_level);
            using (StringReader streamReader = new(levelData))
            {
                using (JsonReader jsonReader = new JsonTextReader(streamReader))
                {
                    JsonSerializer jsonSerializer = new();
                    levelModel = jsonSerializer.Deserialize<LevelModel>(jsonReader);
                }
            }

            GameData.Instance.UseHeart(1);
            if (lives >= GameDataConstants.MaxHeart)
                GameData.Instance.SaveHeartTime(DateTime.Now);

            PlayConfig.Current = new PlayConfig
            {
                IsTest = false,
                Level = _level,
                LevelModel = levelModel,
                UseAiming = _useAiming,
                UseColorful = _useColorful,
                UseExtraBall = _useExtraBall
            };

            int addColorful = _useColorful ? 1 : 0;
            int addPreciseAim = _useAiming ? 1 : 0;
            int addExtraBall = _useExtraBall ? 1 : 0;

            GameData.Instance.AddBooster(IngameBoosterType.Colorful, addColorful);
            GameData.Instance.AddBooster(IngameBoosterType.PreciseAimer, addPreciseAim);
            GameData.Instance.AddBooster(IngameBoosterType.ChangeBall, addExtraBall);

            TransitionConfig.Current = new TransitionConfig
            {
                SceneName = SceneName.Gameplay
            };

            MusicManager.Instance.PlaySoundEffect(alertClip, 0.6f);
            await SceneLoader.LoadScene(SceneConstants.Transition, LoadSceneMode.Single);
        }

        private void AddBooster(bool isUsed, IngameBoosterType boosterType)
        {
            switch (boosterType)
            {
                case IngameBoosterType.Colorful:
                    _useColorful = isUsed;
                    break;
                case IngameBoosterType.PreciseAimer:
                    _useAiming = isUsed;
                    break;
                case IngameBoosterType.ChangeBall:
                    _useExtraBall = isUsed;
                    break;
            }
        }

        protected override void DoClose()
        {
            CloseDelayed().Forget();
        }

        private async UniTask CloseAndShowLifePopup()
        {
            await CloseDelayed();
            await UniTask.Delay(TimeSpan.FromSeconds(0.15f), cancellationToken: _token);
            MainhomeController.Instance.ShowLifePopup();
        }

        private async UniTask CloseDelayed()
        {
            boxAnimator.SetTrigger(_disappearHash);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: _token);
            base.DoClose();
        }

        public void SetLevelBoxData(LevelBoxData boxData)
        {
            _level = boxData.Level;
            _star = boxData.Star;

            levelText.text = $"Level {_level}";

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].SetActive(_star == 0 ? false : i <= _star - 1);
            }
        }

        protected override void OnDisable()
        {
            if(!_isPlayPressed)
                PlayConfig.Current = null;

            _useColorful = _useAiming = _useExtraBall = false;
            boxAnimator.ResetTrigger(_disappearHash);
            base.OnDisable();
        }
    }
}
