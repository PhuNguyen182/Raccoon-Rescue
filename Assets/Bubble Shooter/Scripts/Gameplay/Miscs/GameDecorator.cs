using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class GameDecorator : MonoBehaviour
    {
        [SerializeField] private Transform topCeil;
        [SerializeField] private BoxCollider2D groundingArea;

        public void SetTopCeilPosition(Vector3 position)
        {
            topCeil.position = position;
        }

        public Vector3 GetGroundingPoint()
        {
            float x = Random.Range(groundingArea.bounds.min.x, groundingArea.bounds.max.x);
            float y = Random.Range(groundingArea.bounds.min.y, groundingArea.bounds.max.y);
            return new(x, y);
        }
    }
}
