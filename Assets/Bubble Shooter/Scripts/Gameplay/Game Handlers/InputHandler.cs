using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameHandlers
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        public Vector3 MousePosition { get; private set; }
        public bool IsMousePress { get; private set; }
        public bool IsMouseHold { get; private set; }

        public Action OnHold;
        public Action OnClicked;
        public Action OnRelease;

        private void Update()
        {
            IsMousePress = Input.GetMouseButtonDown(0);
            IsMouseHold = Input.GetMouseButton(0);
            MousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
