using BubbleShooter.Scripts.Gameplay.Inputs;
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

        public InputController InputController;
        public Vector3 InputPosition => InputController.Pointer;
        public bool IsPressed { get; private set; }
        public bool IsHolden => InputController.IsHolden;
        public bool IsReleased => InputController.IsRelease;

        public bool IsActive { get; set; }

        private Touch _touch;

        private void Awake()
        {
            IsActive = true;
        }

        //private void Update()
        //{
        //    ProcessInput();
        //}

        private void ProcessInput()
        {
            if (IsActive)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
                StandaloneInput();
#elif UNITY_ANDROID || UNITY_IOS
                MobileInput();
#endif
            }
        }

        private void StandaloneInput()
        {
            //InputPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            IsPressed = Input.GetMouseButtonDown(0);
            //IsHolden = Input.GetMouseButton(0);
            //IsReleased = Input.GetMouseButtonUp(0);
        }

        private void MobileInput()
        {
            if (Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);
                //InputPosition = mainCamera.ScreenToWorldPoint(_touch.position);

                switch (_touch.phase)
                {
                    case TouchPhase.Began:
                        IsPressed = true;
                        //IsHolden = false;
                        //IsReleased = false;
                        break;
                    case TouchPhase.Moved:
                        IsPressed = false;
                        //IsHolden = true;
                        //IsReleased = false;
                        break;
                    case TouchPhase.Stationary:
                        IsPressed = false;
                        //IsHolden = true;
                        //IsReleased = false;
                        break;
                    case TouchPhase.Ended:
                        IsPressed = false;
                        //IsHolden = false;
                        //IsReleased = true;
                        break;
                }
            }
        }

        public bool IsPointerOverlapUI()
        {
            return InputController.IsPointerOverlapUI();
        }

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(InputPosition.x, InputPosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}
