using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Enums;
using MessagePipe;

namespace BubbleShooter.Scripts.GameUI.IngamePowerup
{
    public class PowerupButton : MonoBehaviour
    {
        [SerializeField] private Image fill;
        [SerializeField] private GameObject glow;
        [SerializeField] private Button button;
        [SerializeField] private EntityType powerUpType;

        private bool _canActive = false;
        private IPublisher<PowerupMessage> _powerupPublisher;

        public EntityType PowerUpType => powerUpType;

        private void Awake()
        {
            button.onClick.AddListener(ActivePowerup);
        }

        private void Start()
        {
            _powerupPublisher = GlobalMessagePipe.GetPublisher<PowerupMessage>();
        }

        private void ActivePowerup()
        {
            if (_canActive)
            {
                _powerupPublisher.Publish(new PowerupMessage
                {
                    IsAdd = false,
                    PowerupColor = powerUpType
                });
                // To do: fire a booster ball relative to powerup type
            }
        }

        private void SetGlowActive(bool active)
        {
            glow.SetActive(active);
        }

        public void SetFillAmount(float fillAmount)
        {
            fill.fillAmount = fillAmount;
            SetGlowActive(fillAmount >= 1);
            _canActive = fillAmount >= 1;
        }
    }
}
