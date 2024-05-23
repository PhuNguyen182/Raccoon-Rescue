using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Feedbacks;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Common.Features.Shop
{
    public class ShopPackCell : MonoBehaviour
    {
        [SerializeField] private string productID;
        [SerializeField] private Button purchaseButton;

        private void Awake()
        {
            purchaseButton.onClick.AddListener(OnPurchaseClick);
        }

        private void OnPurchaseClick()
        {
            GameData.Instance.ShopProfiler.BuyProduct(productID);
            Emittable.Default.Emit("CoinHolder").Forget();
        }
    }
}
