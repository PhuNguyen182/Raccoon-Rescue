using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Mainhome.UI.PopupBoxes
{
    public class SettingPopup : BaseBox<SettingPopup>
    {
        [SerializeField] private Animator boxAnimator;

        [Header("Execute Buttons")]
        [SerializeField] private Button homeButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button soundButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;

        [Header("Button Images")]
        [SerializeField] private Image musicButtonImage;
        [SerializeField] private Image soundButtonImage;

        [Header("Button Sprites")]
        [SerializeField] private Sprite musicOn;
        [SerializeField] private Sprite soundOn;
        [SerializeField] private Sprite musicOff;
        [SerializeField] private Sprite soundOff;

        private static readonly int _disappearHash = Animator.StringToHash("Disappear");

        private bool _musicToggle;
        private bool _soundToggle;
        private CancellationToken _token;

        public Action OnBackHome;

        protected override void OnAwake()
        {
            _token = this.GetCancellationTokenOnDestroy();

            homeButton.onClick.AddListener(BackHome);
            musicButton.onClick.AddListener(MusicButton);
            soundButton.onClick.AddListener(SoundButton);
            closeButton.onClick.AddListener(Close);
            backgroundButton.onClick.AddListener(Close);
        }

        protected override void DoAppear()
        {
            _musicToggle = MusicManager.Instance.MusicVolume > 0;
            _soundToggle = MusicManager.Instance.SoundVolume > 0;
            CheckAudioButtons();
        }

        public void SetBackHomeButtonActive(bool active)
        {
            homeButton.gameObject.SetActive(active);
        }

        private void BackHome()
        {
            OnBackHome?.Invoke();
        }

        private void CheckAudioButtons()
        {
            musicButtonImage.sprite = _musicToggle ? musicOn : musicOff;
            soundButtonImage.sprite = _soundToggle ? soundOn : soundOff;
        }

        private void MusicButton()
        {
            float volume = _musicToggle ? 0 : 1;
            MusicManager.Instance.MusicVolume = volume;
            _musicToggle = MusicManager.Instance.MusicVolume > 0;
            CheckAudioButtons();
        }

        private void SoundButton()
        {
            float volume = _soundToggle ? 0 : 1;
            MusicManager.Instance.SoundVolume = volume;
            _soundToggle = MusicManager.Instance.SoundVolume > 0;
            CheckAudioButtons();
        }

        protected override void DoClose()
        {
            CloseDelayed().Forget();
        }

        private async UniTaskVoid CloseDelayed()
        {
            boxAnimator.SetTrigger(_disappearHash);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: _token);
            base.DoClose();
        }

        protected override void OnDisable()
        {
            boxAnimator.ResetTrigger(_disappearHash);
            base.OnDisable();
        }
    }
}
