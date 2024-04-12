using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class GameDecorator : MonoBehaviour
    {
        [SerializeField] private Transform topCeil;

        public void SetTopCeilPosition(Vector3 position)
        {
            topCeil.position = position;
        }
    }
}
