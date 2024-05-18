using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Mainhome.PopupBoxes;

namespace BubbleShooter.Scripts.Mainhome
{
    public class MainhomeNavigator : MonoBehaviour
    {
        [SerializeField] private Button settingButton;

        private const string SettingPopupPath = "Popups/Setting Popup";

        public static MainhomeNavigator Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            RegisterButtons();
        }

        private void RegisterButtons()
        {
            settingButton.onClick.AddListener(ShowSettingPopup);
        }

        private void ShowSettingPopup()
        {
            SettingPopup.Create(SettingPopupPath);
        }
    }
}
