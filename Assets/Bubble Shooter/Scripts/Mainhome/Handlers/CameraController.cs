using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Mainhome.Inputs;

namespace BubbleShooter.Scripts.Mainhome.Handlers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera lookCamera;
        [SerializeField] private MainhomeInput input;
        [SerializeField] private float translateSpeed = 3f;
        [SerializeField] private float smoothSpeed = 25f;

        private Vector2 _inputDelta;

        public bool IsDraggable { get; set; }

        private void Awake()
        {
            IsDraggable = true;
        }

        private void Update()
        {
            if (IsDraggable)
            {
                if (input.IsDragging)
                    _inputDelta = !input.IsPointerOverlapUI() ? -input.Delta
                                  : Vector2.Lerp(_inputDelta, Vector2.zero, smoothSpeed * Time.deltaTime);

                else _inputDelta = Vector2.Lerp(_inputDelta, Vector2.zero, smoothSpeed * Time.deltaTime);

                lookCamera.transform.Translate(Vector3.up * _inputDelta.y * translateSpeed * Time.deltaTime);
            }
        }

        public void MoveTo(Vector3 position)
        {
            lookCamera.transform.position = position;
        }
    }
}
