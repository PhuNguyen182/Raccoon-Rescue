using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Gameplay.GameHandlers;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class BallPot : MonoBehaviour
    {
        [SerializeField] private Button potButton;
        [SerializeField] private Button arrowButton;
        [SerializeField] private BallShooter ballShooter;

        private void Awake()
        {
            potButton.onClick.AddListener(ChangeBall);
            arrowButton.onClick.AddListener(ChangeBall);
        }

        private void ChangeBall()
        {
            if (ballShooter.IsIngamePowerupHolding())
                return;

            if (arrowButton.gameObject.activeInHierarchy)
                arrowButton.gameObject.SetActive(false);

            ballShooter.SwitchBall();
        }
    }
}
