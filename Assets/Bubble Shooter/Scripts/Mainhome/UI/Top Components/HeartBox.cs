using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Feedbacks;
using BubbleShooter.Scripts.Mainhome.UI.PopupBoxes.PlayGamePopup;
using BubbleShooter.Scripts.Mainhome.GameManagers;
using BubbleShooter.Scripts.Common.PlayDatas;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

namespace BubbleShooter.Scripts.Mainhome.UI.TopComponents
{
    public class HeartBox : MonoBehaviour, IReactiveComponent<HeartBox>
    {
        [SerializeField] private string elementID;
        [SerializeField] private Transform heartIcon;
        [SerializeField] private TMP_Text heartCount;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Button heartButton;

        private UniTaskCompletionSource _tcs;
        private const string PlayGamePopupPath = "Popups/Play Game Popup";

        public bool IsPause { get; set; }

        public HeartBox Receiver => this;

        public Vector3 Position => heartIcon.position;

        public string InstanceID => elementID;

        private void Awake()
        {
            heartButton.onClick.AddListener(OnHeartButton);
        }

        private void Start()
        {
            Emittable.Default.Subscribe(this);
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

        public async UniTask OnReactive(UniTask task)
        {
            await transform.DOPunchScale(Vector3.one * 0.1f, 0.25f, 1, 1)
                           .SetEase(Ease.InOutSine);
            _tcs.TrySetResult();
        }

        public void SetCompletionSource(UniTaskCompletionSource tcs)
        {
            _tcs = tcs;
            OnReactive(DoSomething()).Forget();
        }

        private UniTask DoSomething()
        {
            return UniTask.CompletedTask;
        }

        private void OnDestroy()
        {
            Emittable.Default.Unsubscribe(this);
        }
    }
}
