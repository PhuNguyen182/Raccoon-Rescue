using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Mainhome.Effects;
using BubbleShooter.Scripts.Feedbacks;
using DG.Tweening;
using TMPro;

namespace BubbleShooter.Scripts.Mainhome.UI.PopupBoxes
{
    public class LifePopup : BaseBox<LifePopup>
    {
        [SerializeField] private AudioClip coinClip;
        [SerializeField] private UIObjectEffect heart;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private Animator boxAnimator;

        private static readonly int _disappearHash = Animator.StringToHash("Disappear");

        private int _price = 0;
        private CancellationToken _token;

        protected override void OnAwake()
        {
            base.OnAwake();
            buyButton.onClick.AddListener(BuyLife);
            closeButton.onClick.AddListener(Close);
            backgroundButton.onClick.AddListener(Close);
        }

        protected override void DoAppear()
        {
            _price = GameData.Instance.GameInventory.GetReviveLifeCoins();
            coinText.text = $"{_price}";
        }

        private void BuyLife()
        {
            int currentCoin = GameData.Instance.GetCoins();
            if(currentCoin >= _price)
            {
                GameData.Instance.SpendCoins(_price);
                MusicManager.Instance.PlaySoundEffect(coinClip, 0.6f);
                BuyAndAddHearts().Forget();
            }
            else
            {
                CloseAndOpenShop().Forget();
            }
        }

        private async UniTask BuyAndAddHearts()
        {
            await CloseDelayed();
            Emittable.Default.Emit("CoinHolder").Forget();

            MainhomeController.Instance.HeartBox.IsPause = true;
            int hearts = GameData.Instance.GameInventory.GetMaxHeart();
            GameData.Instance.AddHeart(hearts);
            
            await DoFlyHeart();
            Emittable.Default.Emit("HeartBox").Forget();
            MainhomeController.Instance.HeartBox.IsPause = false;
        }

        private async UniTask DoFlyHeart()
        {
            MainhomeController.Instance.SetInteractive(false);
            var flyHeart = SimplePool.Spawn(heart, UIEffectContainer.Transform
                                            , transform.position, Quaternion.identity);
            
            flyHeart.transform.localScale = Vector3.zero;
            await flyHeart.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            await UniTask.Delay(TimeSpan.FromSeconds(0.4f), cancellationToken: _token);
            Vector3 toPoint = MainhomeController.Instance.HeartBox.Position;

            await flyHeart.MoveSeparatedWithScale(toPoint, 0.75f);
            SimplePool.Despawn(flyHeart.gameObject);
            MainhomeController.Instance.SetInteractive(true);
        }

        private async UniTask CloseAndOpenShop()
        {
            await CloseDelayed();
            await UniTask.Delay(TimeSpan.FromSeconds(0.15f), cancellationToken: _token);
            MainhomeController.Instance.ShowShopPanel();
        }

        protected override void DoClose()
        {
            CloseDelayed().Forget();
        }

        private async UniTask CloseDelayed()
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
