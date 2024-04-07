using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private BallShooter ballShooter;

        private const float DefaultCameraSize = 6.75f;
        private const float DefaultBackgroundScale = 0.7f;
        private const float DefaultScreenRatio = 16f / 9f;

        private void Start()
        {
            GameScreenCalculate();
        }

        private void GameScreenCalculate()
        {
            float currentScreenRatio = 1f / mainCamera.aspect;
            
            if (currentScreenRatio > DefaultScreenRatio)
            {
                mainCamera.orthographicSize = DefaultCameraSize * currentScreenRatio / DefaultScreenRatio;
                background.transform.localScale = Vector3.one * DefaultBackgroundScale
                                                  * currentScreenRatio / DefaultScreenRatio;
            }

            ballShooter.SetStartPosition();
        }
    }
}
