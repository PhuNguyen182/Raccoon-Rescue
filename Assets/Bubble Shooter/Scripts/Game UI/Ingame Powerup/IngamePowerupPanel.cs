using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.GameUI.IngamePowerup
{
    public class IngamePowerupPanel : MonoBehaviour
    {
        [SerializeField] private PowerupButton fireballButton;
        [SerializeField] private PowerupButton leafballButton;
        [SerializeField] private PowerupButton sunballButton;
        [SerializeField] private PowerupButton waterballButton;

        public void ControlPowerupButtons(float fillAmount, EntityType powerupType)
        {
            switch (powerupType)
            {
                case EntityType.FireBall:
                    fireballButton.SetFillAmount(fillAmount);
                    break;
                case EntityType.LeafBall:
                    leafballButton.SetFillAmount(fillAmount);
                    break;
                case EntityType.IceBall:
                    sunballButton.SetFillAmount(fillAmount);
                    break;
                case EntityType.WaterBall:
                    waterballButton.SetFillAmount(fillAmount);
                    break;
                case EntityType.SunBall:
                    break;
            }
        }
    }
}
