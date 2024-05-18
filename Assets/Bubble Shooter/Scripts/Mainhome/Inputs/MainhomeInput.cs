using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace BubbleShooter.Scripts.Mainhome.Inputs
{
    public class MainhomeInput : MonoBehaviour
    {
        [SerializeField] private Camera inputObserverCamera;

        private Vector2 _delta, _deltaTemp;
        private Vector2 _pointerPosition;
        private GameplayInput _inputActions;

        #region Cached UI Overlap Checking Variables
        private List<RaycastResult> _results = new();
        private PointerEventData _eventDataCurrentPosition;
        #endregion

        public bool IsDragging { get; private set; }
        public Vector2 PointerPosition { get; private set; }
        public Vector2 Delta => _deltaTemp;

        private void Awake()
        {
            _inputActions = new();

            _inputActions.Mainhome.Pointer.started += OnPointerPosition;
            _inputActions.Mainhome.Pointer.performed += OnPointerPosition;
            _inputActions.Mainhome.Pointer.canceled += OnPointerPosition;

            _inputActions.Mainhome.Drag.started += OnPointerDrag;
            _inputActions.Mainhome.Drag.performed += OnPointerDrag;
            _inputActions.Mainhome.Drag.canceled += OnPointerDrag;

            _inputActions.Mainhome.Delta.started += OnPointerDelta;
            _inputActions.Mainhome.Delta.performed += OnPointerDelta;
            _inputActions.Mainhome.Delta.canceled += OnPointerDelta;
        }

        private void OnPointerDelta(InputAction.CallbackContext context)
        {
            _delta = context.ReadValue<Vector2>();
            _deltaTemp.x = _delta.x / Screen.width;
            _deltaTemp.y = _delta.y / Screen.height;
        }

        private void OnPointerDrag(InputAction.CallbackContext context)
        {
            IsDragging = context.ReadValueAsButton();
        }

        private void OnPointerPosition(InputAction.CallbackContext context)
        {
            _pointerPosition = context.ReadValue<Vector2>();
            PointerPosition = inputObserverCamera.ScreenToWorldPoint(_pointerPosition);
        }

        public bool IsPointerOverlapUI()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
            return EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
            return IsPointerOverUIObject();
#endif
        }

        public bool IsPointerOverUIObject()
        {
            _results.Clear();
            _eventDataCurrentPosition = new(EventSystem.current);
            _eventDataCurrentPosition.position = new(PointerPosition.x, PointerPosition.y);
            EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
            return _results.Count > 0;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void OnDestroy()
        {
            _inputActions.Dispose();
        }
    }
}
