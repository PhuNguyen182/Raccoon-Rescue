using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.GameUI.IngameElements
{
    public class StarStreak : MonoBehaviour
    {
        [SerializeField] private GameObject star;

        public void SetStarActive(bool active)
        {
            star.SetActive(active);
        }
    }
}
