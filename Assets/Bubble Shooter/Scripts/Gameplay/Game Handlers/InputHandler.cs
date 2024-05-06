using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BubbleShooter.Scripts.Gameplay.GameHandlers
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        public Vector3 InputPosition { get; private set; }
        public bool IsPressed { get; private set; }
        public bool IsHolden { get; private set; }
        public bool IsReleased { get; private set; }

        public bool IsActive { get; set; }

        private Touch _touch;

        private void Awake()
        {
            IsActive = true;
        }

        private void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (IsActive)
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                StandaloneInput();
#elif UNITY_ANDROID || UNITY_IOS
                MobileInput();
#endif
            }
        }

        private void StandaloneInput()
        {
            InputPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (!IsUiOverlap(InputPosition))
            {
                IsPressed = Input.GetMouseButtonDown(0);
                IsHolden = Input.GetMouseButton(0);
                IsReleased = Input.GetMouseButtonUp(0);
            }
            else
            {
                IsPressed = false;
                IsHolden = false;
                IsReleased = false;
            }
        }

        private void MobileInput()
        {
            if(Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);
                InputPosition = mainCamera.ScreenToWorldPoint(_touch.position);
                
                if (!IsUiOverlap(InputPosition))
                {
                    switch (_touch.phase)
                    {
                        case TouchPhase.Began:
                            IsPressed = true;
                            IsHolden = false;
                            IsReleased = false;
                            break;
                        case TouchPhase.Moved:
                            IsPressed = false;
                            IsHolden = true;
                            IsReleased = false;
                            break;
                        case TouchPhase.Stationary:
                            IsPressed = false;
                            IsHolden = true;
                            IsReleased = false;
                            break;
                        case TouchPhase.Ended:
                            IsPressed = false;
                            IsHolden = false;
                            IsReleased = true;
                            break;
                    }
                }
                else
                {
                    IsPressed = false;
                    IsHolden = false;
                    IsReleased = false;
                }
            }
        }

        private bool IsUiOverlap(Vector3 position)
        {
#if UNITY_EDITOR
            return EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
            return IsPointerOverUIObject(position);
#endif
        }

        private bool IsPointerOverUIObject(Vector3 position)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(position.x, position.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}
