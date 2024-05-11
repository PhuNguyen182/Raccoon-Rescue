using R3;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Gameplay.GameManagers;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.IngameBooster
{
    public class BoosterButton : MonoBehaviour
    {
        [SerializeField] private IngameBoosterType booster;
        [SerializeField] private Button boosterButton;
        [SerializeField] private TMP_Text boosterCount;
        [SerializeField] private TMP_Text boosterFree;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Material greyscale;

        private int _count;
        private bool _isFree;
        private bool _isActive;
        private bool _isLocked;

        private CancellationToken _token;

        public bool IsLocked => _isLocked;
        public IngameBoosterType Booster => booster;

        public Observable<(bool IsActive, bool IsFree)> OnClickObserver 
            => boosterButton.OnClickAsObservable()
                            .Where(_ => !_isLocked)
                            .Select(_ => (_isActive, _isFree));

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        // This function is used to block input by UI to prevent unexpected aiming line being displayed
        public async UniTask ShowInvincible()
        {
            GameController.Instance.MainScreenManager.SetInvincibleObjectActive(true);
            await UniTask.DelayFrame(75, PlayerLoopTiming.Update, _token);
            GameController.Instance.MainScreenManager.SetInvincibleObjectActive(false);
        }

        public void SetBoosterCount(int count)
        {
            _count = count;
            boosterCount.text = $"{_count}";
        }

        public void SetBoosterActive(bool active)
        {
            _isActive = active;
        }

        public void SetFreeState(bool free)
        {
            _isFree = free;
            boosterFree.gameObject.SetActive(free);
            boosterCount.gameObject.SetActive(!free);
        }

        public void SetLockState(bool isLocked)
        {
            _isLocked = isLocked;
            buttonImage.material = _isLocked ? null : greyscale;
        }
    }
}
