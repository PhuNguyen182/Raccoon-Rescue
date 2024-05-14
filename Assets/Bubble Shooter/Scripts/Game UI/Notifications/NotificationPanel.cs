using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.GameUI.Notifications
{
    public class NotificationPanel : MonoBehaviour
    {
        [SerializeField] private GameObject background;
        [SerializeField] private NotificationBar notificationBar;
        [SerializeField] private NotificationBar loseLifeBar;

        public async UniTask SetNotificationInfo(string message)
        {
            background.SetActive(true);
            notificationBar.SetNotification(message);
            await notificationBar.ShowNotification();
            background.SetActive(false);
        }

        public async UniTask ShowLosePanel()
        {
            background.SetActive(true);
            await loseLifeBar.ShowNotification();
            background.SetActive(false);
        }
    }
}
