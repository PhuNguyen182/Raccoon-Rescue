using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Mainhome.Effects;
using BubbleShooter.Scripts.Feedbacks;
using TMPro;
using DG.Tweening;

namespace BubbleShooter.Scripts.Mainhome.UI.PopupBoxes
{
    public class LifePopup : BaseBox<LifePopup>
    {
        [SerializeField] private MovementUIObject heart;
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
            var flyHeart = SimplePool.Spawn(heart, UIEffectContainer.Transform
                                            , transform.position, Quaternion.identity);
            
            flyHeart.transform.localScale = Vector3.zero;
            await flyHeart.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            await UniTask.Delay(TimeSpan.FromSeconds(0.4f), cancellationToken: _token);
            Transform toPoint = MainhomeController.Instance.HeartBox.Heart;

            await flyHeart.MoveSeparatedWithScale(toPoint.position, 0.75f);
            SimplePool.Despawn(flyHeart.gameObject);
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
