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

        private Vector2 _pointerPosition;
        private GameplayInput _inputActions;

        #region Cached UI Overlap Checking Variables
        private List<RaycastResult> _results = new();
        private PointerEventData _eventDataCurrentPosition;
        #endregion

        public bool IsDragging { get; private set; }
        public Vector2 PointerPosition { get; private set; }

        private void Awake()
        {
            _inputActions = new();

            _inputActions.Mainhome.Pointer.started += OnPointerPosition;
            _inputActions.Mainhome.Pointer.performed += OnPointerPosition;
            _inputActions.Mainhome.Pointer.canceled += OnPointerPosition;

            _inputActions.Mainhome.Drag.started += OnPointerDrag;
            _inputActions.Mainhome.Drag.performed += OnPointerDrag;
            _inputActions.Mainhome.Drag.canceled += OnPointerDrag;
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
