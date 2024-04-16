using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.GameUI.IngameElements;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.Screens
{
    public class InGamePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private StarStreak starStreak;
        [SerializeField] private TargetHolder targetHolder;
    }
}
