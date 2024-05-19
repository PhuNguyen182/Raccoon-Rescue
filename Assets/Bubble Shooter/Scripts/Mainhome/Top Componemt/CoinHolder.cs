using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Effects.Tweens;
using TMPro;

namespace BubbleShooter.Scripts.Mainhome.TopComponents
{
    public class CoinHolder : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private Button shopButton;
        [SerializeField] private TweenValueEffect coinTween;

        private ReactiveProperty<int> _coinReactive = new();

        private void Awake()
        {
            shopButton.onClick.AddListener(OpenShop);
            coinTween.BindInt(_coinReactive, SetCoinText);
        }

        private void OpenShop()
        {

        }

        private void SetCoinText(int coin)
        {
            coinText.text = $"{coin}";
        }

        public void UpdateCoin(int coin)
        {
            _coinReactive.Value = coin;
        }
    }
}
