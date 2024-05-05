using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Common.Enums;
using TMPro;

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
        public IngameBoosterType Booster => booster;

        public Observable<bool> OnClickObserver
        {
            get => boosterButton.OnClickAsObservable().Where(_ => !_isLocked).Select(_ => !_isActive);
        }

        private void Awake()
        {
            boosterButton.onClick.AddListener(ActivateBooster);
        }

        private void ActivateBooster()
        {
            if(_count > 0)
            {

            }
            else
            {

            }
        }

        public void SetBoosterCount(int count)
        {
            _count = count;
            boosterCount.text = $"{count}";
        }
    }
}
