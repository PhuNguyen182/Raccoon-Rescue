using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BubbleShooter.Scripts.Mainhome.Handlers
{
    public class ScreenBoundsHandler : MonoBehaviour
    {
        [SerializeField] private Bounds bounds;
        [SerializeField] private SpriteRenderer minBackground;
        [SerializeField] private SpriteRenderer maxBackground;

        public Bounds ScreenBounds => bounds;

        [Button]
        public void CalculateBounds()
        {
            bounds = new Bounds
            {
                min = minBackground.bounds.min,
                max = maxBackground.bounds.max
            };
        }
    }
}
