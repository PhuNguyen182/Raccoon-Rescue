using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Effects.Tweens;
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
        [SerializeField] private TweenValueEffect tweenValueEffect;

        private bool _canActive = false;
        private IPublisher<PowerupMessage> _powerupPublisher;
        private ReactiveProperty<float> _reactiveFillAmount = new();

        public EntityType PowerUpType => powerUpType;

        private void Awake()
        {
            button.onClick.AddListener(ActivePowerup);
            tweenValueEffect.BindFloat(_reactiveFillAmount, ShowFillAmount);
            _reactiveFillAmount.Value = 0;
        }

        private void Start()
        {
            _powerupPublisher = GlobalMessagePipe.GetPublisher<PowerupMessage>();
        }

        private void ActivePowerup()
        {
            if (_canActive)
            {
                // When free booster counter, it means use that booster
                _powerupPublisher.Publish(new PowerupMessage
                {
                    PowerupColor = powerUpType,
                    Command = ReactiveValueCommand.Reset
                });
            }
        }

        private void SetGlowActive(bool active)
        {
            glow.SetActive(active);
        }

        public void SetFillAmount(float fillAmount)
        {
            _reactiveFillAmount.Value = fillAmount;
            SetGlowActive(fillAmount >= 1);
            _canActive = fillAmount >= 1;
        }

        private void ShowFillAmount(float fillAmount)
        {
            fill.fillAmount = fillAmount;
        }
    }
}
