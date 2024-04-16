using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.GameUI.IngamePowerup;
using BubbleShooter.Scripts.GameUI.Notifications;

namespace BubbleShooter.Scripts.GameUI.Screens
{
    public class MainScreenManager : MonoBehaviour
    {
        [SerializeField] private IngamePowerupPanel powerupPanel;
        [SerializeField] private NotificationPanel notificationPanel;
        [SerializeField] private EndGameScreen endGameScreen;

        public IngamePowerupPanel IngamePowerupPanel => powerupPanel;
        public NotificationPanel NotificationPanel => notificationPanel;
        public EndGameScreen EndGameScreen => endGameScreen;
    }
}
