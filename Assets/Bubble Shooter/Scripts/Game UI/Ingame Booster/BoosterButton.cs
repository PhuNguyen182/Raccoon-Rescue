using R3;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TMPro;
using BubbleShooter.Scripts.Gameplay.GameManagers;

namespace BubbleShooter.Scripts.GameUI.IngameBooster
{
    public class BoosterButton : MonoBehaviour
    {
        [SerializeField] private IngameBoosterType booster;
        [SerializeField] private Button boosterButton;
        [SerializeField] private TMP_Text boosterCount;

        private int _count;
        private bool _isActive;
        private bool _isLocked;

        private CancellationToken _token;
        private IPublisher<IngameBoosterMessage> _boosterPublisher;

        public IngameBoosterType Booster => booster;

        public Observable<bool> OnClickObserver
        {
            get => boosterButton.OnClickAsObservable().Where(_ => !_isLocked).Select(_ => !_isActive);
        }

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
            boosterButton.onClick.AddListener(ActivateBooster);
        }

        private void Start()
        {
            _boosterPublisher = GlobalMessagePipe.GetPublisher<IngameBoosterMessage>();
        }

        private void ActivateBooster()
        {
            if(_count >= 0)
            {
                _boosterPublisher.Publish(new IngameBoosterMessage
                {
                    BoosterType = booster
                });

                ShowInvincible().Forget();
            }

            else
            {

            }
        }

        // This function is used to block input by UI to prevent unexpected aiming line being displayed
        private async UniTask ShowInvincible()
        {
            GameController.Instance.MainScreenManager.SetInvincibleObjectActive(true);
            await UniTask.DelayFrame(75, PlayerLoopTiming.Update, _token);
            GameController.Instance.MainScreenManager.SetInvincibleObjectActive(false);
        }

        public void SetBoosterCount(int count)
        {
            _count = count;
            boosterCount.text = $"{count}";
        }
    }
}
