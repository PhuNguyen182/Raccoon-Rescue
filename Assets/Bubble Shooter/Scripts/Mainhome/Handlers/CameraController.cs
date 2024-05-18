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

        private void Update()
        {
            if (input.IsDragging)
            {
                if (!input.IsPointerOverlapUI())
                {
                    _inputDelta = -input.Delta.normalized;
                }
            }

            else
                _inputDelta = Vector2.Lerp(_inputDelta, Vector2.zero, smoothSpeed * Time.deltaTime);
            
            lookCamera.transform.Translate(Vector3.up * _inputDelta.y * translateSpeed * Time.deltaTime);
        }
    }
}
