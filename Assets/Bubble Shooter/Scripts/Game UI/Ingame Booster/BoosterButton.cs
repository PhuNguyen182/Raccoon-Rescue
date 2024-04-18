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

        public IngameBoosterType Booster => booster;

        private void Awake()
        {
            boosterButton.onClick.AddListener(ActivateBooster);
        }

        private void ActivateBooster()
        {

        }

        public void SetBoosterCount(int count)
        {
            boosterCount.text = $"{count}";
        }
    }
}
