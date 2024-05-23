using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Feedbacks;
using BubbleShooter.Scripts.Effects.Tweens;
using Cysharp.Threading.Tasks;
using TMPro;

namespace BubbleShooter.Scripts.Mainhome.UI.TopComponents
{
    public class CoinHolder : MonoBehaviour, IReactiveComponent<CoinHolder>
    {
        [SerializeField] private string elementID;
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private Button shopButton;
        [SerializeField] private TweenValueEffect coinTween;

        private UniTaskCompletionSource _tcs;
        private ReactiveProperty<int> _coinReactive = new();

        public CoinHolder Receiver => this;

        public Vector3 Position => transform.position;

        public string InstanceID => elementID;

        private void Awake()
        {
            shopButton.onClick.AddListener(OpenShop);
            _coinReactive.Value = GameData.Instance.GetCoins();
            coinTween.BindInt(_coinReactive, SetCoinText);
        }

        private void Start()
        {
            Emittable.Default.Subscribe(this);
        }

        private void OpenShop()
        {
            MainhomeController.Instance.ShowShopPanel();
        }

        private void SetCoinText(int coin)
        {
            coinText.text = $"{coin}";
        }

        private void UpdateCoin(int coin)
        {
            _coinReactive.Value = coin;
        }

        public async UniTask OnReactive(UniTask task)
        {
            if (_tcs != null)
            {
                await task;
                _tcs.TrySetResult();
                UpdateCoin(GameData.Instance.GetCoins());
            }
        }

        public void SetCompletionSource(UniTaskCompletionSource tcs)
        {
            _tcs = tcs;
            OnReactive(DoSomething()).Forget();
        }

        private UniTask DoSomething()
        {
            return UniTask.CompletedTask;
        }

        private void OnDestroy()
        {
            Emittable.Default.Unsubscribe(this);
        }
    }
}
