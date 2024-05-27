using R3;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Feedbacks;
using BubbleShooter.Scripts.Effects.Tweens;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

namespace BubbleShooter.Scripts.Mainhome.UI.TopComponents
{
    public class CoinHolder : MonoBehaviour, IReactiveComponent<CoinHolder>
    {
        [SerializeField] private string elementID;
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private Button shopButton;
        [SerializeField] private TweenValueEffect coinTween;
        [SerializeField] private Transform coinIcon;

        private CancellationToken _token;
        private UniTaskCompletionSource _tcs;
        private ReactiveProperty<int> _coinReactive = new();

        public CoinHolder Receiver => this;

        public Vector3 Position => coinIcon.position;

        public string InstanceID => elementID;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();

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
                UpdateCoin(GameData.Instance.GetCoins());

                await task;
                _tcs.TrySetResult();
            }
        }

        public void SetCompletionSource(UniTaskCompletionSource tcs)
        {
            _tcs = tcs;
            OnReactive(DoSomething()).Forget();
        }

        private async UniTask DoSomething()
        {
            for (int i = 0; i < 5; i++)
            {
                await coinIcon.DOPunchScale(Vector3.one * 0.2f, 0.125f, 1, 1).SetEase(Ease.InOutSine);
            }
        }

        private void OnDestroy()
        {
            Emittable.Default.Unsubscribe(this);
        }
    }
}
