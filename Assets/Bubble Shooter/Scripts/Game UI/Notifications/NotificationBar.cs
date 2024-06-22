using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.Notifications
{
    public class NotificationBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text message;
        [SerializeField] private Animator panelAnimator;
        [SerializeField] private AudioClip swapClip;

        [Header("Animation Clips")]
        [SerializeField] private AnimationClip moveIn;
        [SerializeField] private AnimationClip moveOut;

        private readonly int _moveHash = Animator.StringToHash("Move");

        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        public void SetNotification(string content)
        {
            message.text = content;
        }

        public async UniTask ShowNotification()
        {
            MusicManager.Instance.PlaySoundEffect(swapClip, 0.6f);
            panelAnimator.SetBool(_moveHash, true);
            await UniTask.Delay(TimeSpan.FromSeconds(moveIn.length + 1.25f), cancellationToken: _token);

            panelAnimator.SetBool(_moveHash, false);
            MusicManager.Instance.PlaySoundEffect(swapClip, 0.6f);
            await UniTask.Delay(TimeSpan.FromSeconds(moveOut.length + 0.25f), cancellationToken: _token);
        }
    }
}
