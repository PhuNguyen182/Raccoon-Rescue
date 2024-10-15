using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Mainhome.Inputs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace BubbleShooter.Scripts.Mainhome.Handlers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera lookCamera;
        [SerializeField] private RectTransform mainCanvasSafeArea;
        [SerializeField] private ScreenBoundsHandler screenBounds;
        [SerializeField] private MainhomeInput input;
        
        [Space(10)]
        [SerializeField] private float translateSpeed = 3f;
        [SerializeField] private float smoothSpeed = 25f;

        private float _topCanvasOffset = 0;
        private float _bottomCanvasOffset = 0;

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
        }

        private void Start()
        {
            IsDraggable = true;
            CalculateCameraBounds();
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

        public float GetDragSpeed()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
            return translateSpeed;
#elif UNITY_ANDROID || UNITY_IOS
            return translateSpeed * 0.45f;
#endif
        }

        private void DragCamera()
        {
            if (IsDraggable)
            {
                float dragSpeed = GetDragSpeed();
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
            #region Calculate original map bounds
            float height = lookCamera.orthographicSize;
            float width = height * lookCamera.aspect;
            float minX = screenBounds.ScreenBounds.min.x + width;
            float maxX = screenBounds.ScreenBounds.max.x - width;
            float minY = screenBounds.ScreenBounds.min.y + height;
            float maxY = screenBounds.ScreenBounds.max.y - height;
            #endregion

            #region Calculate safe area offset amount
            Vector3 topPoint = lookCamera.ViewportToWorldPoint(new Vector3(1, 1));
            Vector3 bottomPoint = lookCamera.ViewportToWorldPoint(new Vector3(1, 0));
            Vector3 safeTopPoint = lookCamera.ViewportToWorldPoint(new Vector3(1, mainCanvasSafeArea.anchorMax.y));
            Vector3 safeBottomPoint = lookCamera.ViewportToWorldPoint(new Vector3(1, mainCanvasSafeArea.anchorMin.y));

            _topCanvasOffset = topPoint.y - safeTopPoint.y;
            _bottomCanvasOffset = safeBottomPoint.y - bottomPoint.y;
            #endregion

            _lookCameraBounds = new Bounds
            {
                min = new(minX, minY - _bottomCanvasOffset),
                max = new(maxX, maxY + _topCanvasOffset)
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
