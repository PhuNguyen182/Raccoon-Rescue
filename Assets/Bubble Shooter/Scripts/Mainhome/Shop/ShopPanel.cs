using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Mainhome.Shop
{
    public class ShopPanel : BaseBox<ShopPanel>
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Animator shopAnimator;

        private static readonly int _disappearHash = Animator.StringToHash("Disappear");

        private CancellationToken _token;

        protected override void OnAwake()
        {
            _token = this.GetCancellationTokenOnDestroy();
            closeButton.onClick.AddListener(Close);
        }

        protected override void DoClose()
        {
            OnClose().Forget();
        }

        private async UniTask OnClose()
        {
            shopAnimator.SetTrigger(_disappearHash);
            await UniTask.Delay(TimeSpan.FromSeconds(0.667f), cancellationToken: _token);

            shopAnimator.ResetTrigger(_disappearHash);
            base.DoClose();
        }
    }
}
