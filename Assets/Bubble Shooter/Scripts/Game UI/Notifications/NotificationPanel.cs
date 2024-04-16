using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.GameUI.Notifications
{
    public class NotificationPanel : MonoBehaviour
    {
        [SerializeField] private NotificationBar notificationBar;

        public void SetNotificationInfo(NotificationMessage message)
        {
            notificationBar.SetNotification(message.Icon, message.Content);
        }
    }
}
