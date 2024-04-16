using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.Notifications
{
    public class NotificationBar : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text message;
        [SerializeField] private Animator panelAnimator;

        [Header("Animation Clips")]
        [SerializeField] private AnimationClip moveIn;
        [SerializeField] private AnimationClip moveOut;

        private static readonly int _moveHash = Animator.StringToHash("Move");

        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        public void SetNotification(Sprite sprite, string content)
        {
            icon.sprite = sprite;
            message.text = content;
        }

        public async UniTask ShowNotification()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(moveIn.length + 1.25f), cancellationToken: _token);
            panelAnimator.SetBool(_moveHash, true);
            await UniTask.Delay(TimeSpan.FromSeconds(moveOut.length + 0.5f), cancellationToken: _token);
        }
    }
}
