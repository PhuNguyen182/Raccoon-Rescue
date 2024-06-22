using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.GameUI.IngamePowerup;
using BubbleShooter.Scripts.GameUI.Notifications;
using BubbleShooter.Scripts.GameUI.IngameBooster;

namespace BubbleShooter.Scripts.GameUI.Screens
{
    public class MainScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject invincible;
        [SerializeField] private Animator mainPanelAnimator;

        [Header("UI Panels")]
        [SerializeField] private InGamePanel inGamePanel;
        [SerializeField] private IngamePowerupPanel powerupPanel;
        [SerializeField] private NotificationPanel notificationPanel;
        [SerializeField] private IngameBoosterPanel boosterPanel;
        [SerializeField] private EndGameScreen endGameScreen;

        private readonly int _appearHash = Animator.StringToHash("Appear");
        private readonly int _disappearHash = Animator.StringToHash("Disappear");

        public InGamePanel InGamePanel => inGamePanel;
        public IngamePowerupPanel IngamePowerupPanel => powerupPanel;
        public NotificationPanel NotificationPanel => notificationPanel;
        public EndGameScreen EndGameScreen => endGameScreen;
        public IngameBoosterPanel BoosterPanel => boosterPanel;

        public void SetInvincibleObjectActive(bool isActive)
        {
            invincible.SetActive(isActive);
        }

        public void ShowMainPanel(bool active)
        {
            mainPanelAnimator.SetTrigger(active ? _appearHash : _disappearHash);
        }
    }
}
