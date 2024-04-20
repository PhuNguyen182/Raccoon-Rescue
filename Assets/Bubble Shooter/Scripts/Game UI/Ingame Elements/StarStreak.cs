using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.GameUI.IngameElements
{
    public class StarStreak : MonoBehaviour
    {
        [SerializeField] private GameObject star;
        [SerializeField] private RectTransform rect;

        public void SetStarActive(bool active)
        {
            star.SetActive(active);
        }

        public void SetHorizontalPosition(float x)
        {
            // This rect transform is anchored left side
            // it means anchor is (0, 0.5f)
            rect.anchoredPosition = new Vector2(x, 2.5f);
        }
    }
}
