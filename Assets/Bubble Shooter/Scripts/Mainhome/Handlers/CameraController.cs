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

        private Vector2 _inputOffset;
        private Vector2 _inputPosition;

        private void Update()
        {
            if (!input.IsDragging)
                return;

            if (input.IsPointerOverlapUI())
                return;

            _inputPosition = input.PointerPosition;
            _inputOffset = input.PointerPosition - _inputPosition;
            
            _inputOffset.Normalize();
            lookCamera.transform.Translate(Vector3.up * _inputOffset.y * translateSpeed * Time.deltaTime);
            _inputPosition = input.PointerPosition;
        }
    }
}
