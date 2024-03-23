using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Gameplay.Confiner
{
    public class SideWall : MonoBehaviour
    {
        [SerializeField] private WallSide wallSide;
        [SerializeField] private Camera mainCamera;

        private void Awake()
        {
            RestrictPosition();
        }

        private void RestrictPosition()
        {
            Vector3 position = wallSide == WallSide.Left ? mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f))
                                                         : mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f));
            position.z = 0;
            transform.position = position;
        }
    }
}
