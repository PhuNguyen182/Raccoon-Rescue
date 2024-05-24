using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
