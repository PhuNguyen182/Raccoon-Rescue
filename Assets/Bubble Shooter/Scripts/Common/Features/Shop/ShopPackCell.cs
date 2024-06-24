using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Mainhome;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Feedbacks;

namespace BubbleShooter.Scripts.Common.Features.Shop
{
    public class ShopPackCell : MonoBehaviour
    {
        [SerializeField] private AudioClip coinClip;
        [SerializeField] private string productID;
        [SerializeField] private Button purchaseButton;

        private CancellationToken _token;

        private void Awake()
        {
            purchaseButton.onClick.AddListener(OnPurchaseClick);
        }

        private void OnPurchaseClick()
        {
            MusicManager.Instance.PlaySoundEffect(coinClip, 0.6f);
            GameData.Instance.ShopProfiler.BuyProduct(productID);
            DoShopEffect().Forget();
        }

        private async UniTask DoShopEffect()
        {
            if (MainhomeController.Instance != null)
            {
                MainhomeController.Instance.SetInteractive(false);
                MainhomeController.Instance.CloseShopPanel();
                await UniTask.Delay(TimeSpan.FromSeconds(0.667f), cancellationToken: _token);
                await MainhomeController.Instance.UIEffectManager.SpawnFlyCoin();
                MainhomeController.Instance.SetInteractive(true);
            }

            else
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.167f), cancellationToken: _token);
                await Emittable.Default.Emit("CoinHolder");
            }
        }
    }
}
