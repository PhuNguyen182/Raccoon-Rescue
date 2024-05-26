using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using UnityEngine.UI;

namespace BubbleShooter.Scripts.Mainhome.UI.PopupBoxes.PlayGamePopup
{
    public class BoosterToggle : MonoBehaviour
    {
        [SerializeField] private IngameBoosterType boosterType;
        [SerializeField] private Toggle boosterToggle;
        [SerializeField] private GameObject lockState;

        public IngameBoosterType Booster => boosterType;

        private bool _isLocked = false;

        public Observable<bool> OnClickObservable => boosterToggle.OnValueChangedAsObservable().Where(_ => !_isLocked);

        public void SetLockState(bool isLocked)
        {
            _isLocked = isLocked;
            lockState.SetActive(_isLocked);
        }
    }
}
