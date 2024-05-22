using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        }
    }
}
