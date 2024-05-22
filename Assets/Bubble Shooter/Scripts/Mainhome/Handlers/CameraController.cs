using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Mainhome.Inputs;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace BubbleShooter.Scripts.Mainhome.Handlers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera lookCamera;
        [SerializeField] private ScreenBoundsHandler screenBounds;
        [SerializeField] private MainhomeInput input;
        
        [Space(10)]
        [SerializeField] private float translateSpeed = 3f;
        [SerializeField] private float smoothSpeed = 25f;

        private Vector2 _inputDelta;
        private CancellationToken _token;
        private Bounds _lookCameraBounds;

        private const float DefaultCameraSize = 6.75f;
        private const float DefaultCameraRatio = 9f / 16f;

        public bool IsDraggable { get; set; }

        private void Awake()
        {
            CalculateCameraView();
            _token = this.GetCancellationTokenOnDestroy();
            _lookCameraBounds = screenBounds.ScreenBounds;
            CalculateCameraBounds();
        }

        private void Start()
        {
            IsDraggable = true;
        }

        private void Update()
        {
            DragCamera();
            RestrictCamera();
        }

        public void SetCameraBounds(ScreenBoundsHandler screenBounds)
        {
            this.screenBounds = screenBounds;
        }

        public void TranslateTo(Vector3 position)
        {
            IsDraggable = false;
            Vector3 toPosition = new(position.x, position.y, -10);
            lookCamera.transform.position = toPosition;
            IsDraggable = true;
        }

        public async UniTask MoveTo(Vector3 position, float duration, Ease ease)
        {
            IsDraggable = false;
            Vector3 toPosition = new(position.x, position.y, -10);
            await lookCamera.transform.DOMove(toPosition, duration).SetEase(ease);

            lookCamera.transform.DOKill();
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: _token);
            IsDraggable = true;
        }

        private void DragCamera()
        {
            if (IsDraggable)
            {
                if (input.IsDragging)
                    _inputDelta = !input.IsPointerOverlapUI() ? -input.Delta
                                  : Vector2.Lerp(_inputDelta, Vector2.zero, smoothSpeed * Time.deltaTime);

                else _inputDelta = Vector2.Lerp(_inputDelta, Vector2.zero, smoothSpeed * Time.deltaTime);

                lookCamera.transform.Translate(Vector3.up * _inputDelta.y * translateSpeed * Time.deltaTime);
            }
        }

        private void CalculateCameraView()
        {
            float cameraRatio = lookCamera.aspect;
            float newSize = DefaultCameraSize * DefaultCameraRatio / cameraRatio;
            lookCamera.orthographicSize = newSize;
        }

        private void CalculateCameraBounds()
        {
            float height = lookCamera.orthographicSize;
            float width = height * lookCamera.aspect;
            float minX = screenBounds.ScreenBounds.min.x + width;
            float maxX = screenBounds.ScreenBounds.max.x - width;
            float minY = screenBounds.ScreenBounds.min.y + height;
            float maxY = screenBounds.ScreenBounds.max.y - height;

            _lookCameraBounds = new Bounds
            {
                min = new(minX, minY),
                max = new(maxX, maxY)
            };
        }

        private void RestrictCamera()
        {
            lookCamera.transform.position = GetCameraBounds();
        }

        private Vector3 GetCameraBounds()
        {
            return new Vector3
                (
                    Mathf.Clamp(lookCamera.transform.position.x, _lookCameraBounds.min.x, _lookCameraBounds.max.x),
                    Mathf.Clamp(lookCamera.transform.position.y, _lookCameraBounds.min.y, _lookCameraBounds.max.y),
                    lookCamera.transform.position.z
                );
        }
    }
}
