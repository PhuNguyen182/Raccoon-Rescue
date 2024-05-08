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
        [SerializeField] private InGamePanel inGamePanel;
        [SerializeField] private IngamePowerupPanel powerupPanel;
        [SerializeField] private NotificationPanel notificationPanel;
        [SerializeField] private EndGameScreen endGameScreen;
        [SerializeField] private BoosterPanel boosterPanel;

        public InGamePanel InGamePanel => inGamePanel;
        public IngamePowerupPanel IngamePowerupPanel => powerupPanel;
        public NotificationPanel NotificationPanel => notificationPanel;
        public EndGameScreen EndGameScreen => endGameScreen;
        public BoosterPanel BoosterPanel => boosterPanel;

        public void SetInvincibleObjectActive(bool isActive)
        {
            invincible.SetActive(isActive);
        }
    }
}
