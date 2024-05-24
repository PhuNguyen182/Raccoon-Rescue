using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Feedbacks;
using TMPro;

namespace BubbleShooter.Scripts.Mainhome.PopupBoxes
{
    public class LifePopup : BaseBox<LifePopup>
    {
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
                BuyAndAddHearts();
            }
            else
            {
                CloseAndOpenShop().Forget();
            }
        }

        private void BuyAndAddHearts()
        {
            GameData.Instance.SpendCoins(_price);
            Emittable.Default.Emit("CoinHolder").Forget();

            int hearts = GameData.Instance.GameInventory.GetMaxHeart();
            GameData.Instance.AddHeart(hearts);
            CloseDelayed().Forget();
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
