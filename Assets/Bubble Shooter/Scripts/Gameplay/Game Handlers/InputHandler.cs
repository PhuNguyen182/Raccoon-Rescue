using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;
using UnityEngine.EventSystems;

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
            MousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (!IsUiOverlap(MousePosition))
            {
                IsMousePress = Input.GetMouseButtonDown(0);
                IsMouseHold = Input.GetMouseButton(0);
                IsMouseUp = Input.GetMouseButtonUp(0);

                if (IsMousePress)
                    OnClicked?.Invoke();

                if (IsMouseUp)
                    OnRelease?.Invoke();
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

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}
