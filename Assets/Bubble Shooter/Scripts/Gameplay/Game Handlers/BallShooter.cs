using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameEntities.CustomEntities;

namespace BubbleShooter.Scripts.Gameplay.GameHandlers
{
    public class BallShooter : MonoBehaviour
    {
        [SerializeField] private CommonBall prefab;

        [Space(10)]
        [SerializeField] private Transform pointer;
        [SerializeField] private Transform spawnPoint;

        [Space(10)]
        [SerializeField] private LineDrawer lineDrawer;
        [SerializeField] private InputHandler inputHandler;
        
        [Space(10)]
        [Header("Start Position Setting")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Vector2 normalizePosition;

        private Vector2 _direction;
        private Vector3 _startPosition;

        private void Awake()
        {
            SetStartPosition();
        }

        private void Update()
        {
            GetMouseDirection();

            if (inputHandler.IsMouseUp)
            {
                SpawnABall();
            }

            lineDrawer.DrawLine(inputHandler.IsMouseHold);
            RotatePointer();
        }

        private void SetStartPosition()
        {
            _startPosition = mainCamera.ViewportToWorldPoint(normalizePosition);
            _startPosition.z = 0;
            transform.position = _startPosition;
        }

        private void GetMouseDirection()
        {
            _direction = inputHandler.MousePosition - spawnPoint.position;
        }

        private void SpawnABall()
        {
            CommonBall ball = SimplePool.Spawn(prefab, EntityContainer.Transform
                                        , spawnPoint.position, Quaternion.identity);

            ball.BallMovement.CanMove = false;
            ball.BallMovement.SetBodyActive(false);
            ball.BallMovement.SetMoveDirection(_direction);
            ball.BallMovement.CanMove = true;
        }

        private void RotatePointer()
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
            pointer.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
