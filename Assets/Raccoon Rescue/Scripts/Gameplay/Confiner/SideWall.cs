using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RaccoonRescue.Scripts.Gameplay.Common.Enums;

namespace RaccoonRescue.Scripts.Gameplay.Confiner
{
    public class SideWall : MonoBehaviour
    {
        [SerializeField] private WallSide wallSide;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            RestrictPosition();
        }

        private void RestrictPosition()
        {
            Vector3 position = wallSide == WallSide.Left ? _mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f))
                                                         : _mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f));
            position.z = 0;
            transform.position = position;
        }
    }
}
