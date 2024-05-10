using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BubbleShooter.Scripts.Gameplay.Inputs
{
    public class InputController : MonoBehaviour
    {
        private GameplayInput _inputController;

        public bool Release { get; private set; }
        public Vector2 Position { get; private set; }

        private void Awake()
        {
            _inputController = new();

            _inputController.Gameplay.Move.started += OnMovePerform;
            _inputController.Gameplay.Move.performed += OnMovePerform;
            _inputController.Gameplay.Move.canceled += OnMovePerform;

            _inputController.Gameplay.Release.started += OnReleasePerform;
            _inputController.Gameplay.Release.performed += OnReleasePerform;
            _inputController.Gameplay.Release.canceled += OnReleasePerform;
        }

        private void OnReleasePerform(InputAction.CallbackContext context)
        {
            Release = context.ReadValue<bool>();
        }

        private void OnMovePerform(InputAction.CallbackContext context)
        {
            Position = context.ReadValue<Vector2>();
        }

        private void OnEnable()
        {
            _inputController.Enable();
        }

        private void OnDisable()
        {
            _inputController.Disable();
        }

        private void OnDestroy()
        {
            _inputController.Dispose();
        }
    }
}
