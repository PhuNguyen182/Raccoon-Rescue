using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class CameraController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private Ease moveEase = Ease.InOutSine;

        [Space(10)]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private BallShooter ballShooter;

        private const float DefaultCameraSize = 6.75f;
        private const float DefaultBackgroundScale = 0.7f;
        private const float DefaultScreenRatio = 16f / 9f;

        private Tweener _moveTween;
        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

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

        public UniTask MoveTo(Vector3 toPosition)
        {
            _moveTween ??= CreateMoveTween(toPosition);
            _moveTween.ChangeStartValue(transform.position);
            _moveTween.ChangeEndValue(toPosition);

            _moveTween.Rewind();
            _moveTween.Play();

            return UniTask.Delay(TimeSpan.FromSeconds(_moveTween.Duration()), cancellationToken: _token);
        }

        private Tweener CreateMoveTween(Vector3 toPosition)
        {
            return transform.DOMove(toPosition, moveDuration).SetEase(moveEase).SetAutoKill(false);
        }

        private void OnDestroy()
        {
            _moveTween?.Kill();
        }
    }
}
