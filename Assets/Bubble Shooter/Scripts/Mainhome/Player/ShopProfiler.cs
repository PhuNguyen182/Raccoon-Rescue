using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Common.InAppPurchase;
using Scripts.Service;
using UnityEngine;

namespace BubbleShooter.Scripts.Mainhome.Player
{
    public class ShopProfiler : IService
    {
        private Dictionary<string, bool> _buyBack = new();

        private Dictionary<string, Action> _onPackBuyCallback;

        public ShopProfiler()
        {
            Initialize();
        }

        public bool IsProductBought(string pruductID)
        {
            return _buyBack.ContainsKey(pruductID);
        }

        // Use for non-consumable products
        public void BuyBackProduct(string productID)
        {
            if (!_buyBack.ContainsKey(productID))
                _buyBack.Add(productID, true);
            
            _buyBack[productID] = true;
        }

        public void BuyProduct(string productID)
        {
            if (_onPackBuyCallback.ContainsKey(productID))
                _onPackBuyCallback[productID].Invoke();

            //InAppPurchaseController.Instance.BuyProduct(productID);
        }

        public void Initialize()
        {
            _onPackBuyCallback = new()
            {
                { "coin1", BuyCoin1 },
                { "coin2", BuyCoin2 },
                { "coin3", BuyCoin3 },
                { "coin4", BuyCoin4 }
            };
        }        

        private void BuyCoin1()
        {
            GameData.Instance.AddCoins(25);
        }

        private void BuyCoin2()
        {
            GameData.Instance.AddCoins(65);
        }

        private void BuyCoin3()
        {
            GameData.Instance.AddCoins(150);
        }

        private void BuyCoin4()
        {
            GameData.Instance.AddCoins(250);
        }
    }
}
