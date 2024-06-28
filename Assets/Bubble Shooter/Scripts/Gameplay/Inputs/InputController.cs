using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using BubbleShooter.Scripts.Gameplay.Miscs;

namespace BubbleShooter.Scripts.Gameplay.Inputs
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameDecorator gameDecorator;

        private float _hold;
        private GameplayInput _inputController;

        #region Cached UI Overlap Checking Variables
        private List<RaycastResult> _results = new();
        private PointerEventData _eventDataCurrentPosition;
        #endregion

        public bool IsActive { get; set; }
        public bool IsHolden => _hold > 0.5f;
        public bool IsRelease => GetReleaseState();

        public Vector2 Position { get; private set; }
        public Vector3 Pointer { get; private set; }

        private void Awake()
        {
            IsActive = true;
            _inputController = new();

            _inputController.Gameplay.Move.started += OnMovePerform;
            _inputController.Gameplay.Move.performed += OnMovePerform;
            _inputController.Gameplay.Move.canceled += OnMovePerform;

            _inputController.Gameplay.Release.started += OnReleasePerform;
            _inputController.Gameplay.Release.performed += OnReleasePerform;
            _inputController.Gameplay.Release.canceled += OnReleasePerform;
        }

        private void OnEnable()
        {
            _inputController.Enable();
            EnhancedTouchSupport.Enable();
        }

        private void OnReleasePerform(InputAction.CallbackContext context)
        {
            _hold = context.ReadValue<float>();
            gameDecorator.ShowReflectWalls(IsHolden && IsActive);
        }

        private void OnMovePerform(InputAction.CallbackContext context)
        {
            Position = context.ReadValue<Vector2>();

            if (mainCamera != null)
                Pointer = mainCamera.ScreenToWorldPoint(Position);
        }

        private bool GetReleaseState()
        {
            return _inputController.Gameplay.Release.WasReleasedThisFrame();
        }

        public bool IsPointerOverlapUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private void OnDisable()
        {
            _inputController.Disable();
            EnhancedTouchSupport.Disable();
        }

        private void OnDestroy()
        {
            _inputController.Dispose();
        }
    }
}
