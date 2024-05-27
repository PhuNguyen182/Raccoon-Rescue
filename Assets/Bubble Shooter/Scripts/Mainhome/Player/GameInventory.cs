using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Mainhome.Player
{
    public class GameInventory
    {
        private const int MaxHeart = 5;
        private const int ReviveLifeCoin = 600;

        public int GetReviveLifeCoins()
        {
            return ReviveLifeCoin;
        }

        public int GetMaxHeart()
        {
            return MaxHeart;
        }

        public int GetIngameBoosterPrice(IngameBoosterType boosterType)
        {
            int price = boosterType switch
            {
                IngameBoosterType.Colorful => 600,
                IngameBoosterType.PreciseAimer => 800,
                IngameBoosterType.ChangeBall => 400,
                _ => 0
            };

            return price;
        }
    }
}
