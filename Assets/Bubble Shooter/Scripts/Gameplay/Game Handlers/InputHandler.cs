using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

namespace BubbleShooter.Scripts.Gameplay.GameHandlers
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        public Vector3 MousePosition { get; private set; }
        public bool IsMousePress { get; private set; }
        public bool IsMouseHold { get; private set; }
        public bool IsMouseUp { get; private set; }

        public event Action OnClicked;
        public event Action OnRelease;

        private IDisposable _disposable;

        private void Awake()
        {
            var d = Disposable.CreateBuilder();
            
            Observable.EveryUpdate()
                      .Index()
                      .Subscribe(_ => ProcessInput())
                      .AddTo(ref d);
            
            _disposable = d.Build();
        }

        private void ProcessInput()
        {
            IsMousePress = Input.GetMouseButtonDown(0);
            IsMouseHold = Input.GetMouseButton(0);
            IsMouseUp = Input.GetMouseButtonUp(0);

            MousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (IsMousePress)
                OnClicked?.Invoke();

            if (IsMouseUp)
                OnRelease?.Invoke();
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}
