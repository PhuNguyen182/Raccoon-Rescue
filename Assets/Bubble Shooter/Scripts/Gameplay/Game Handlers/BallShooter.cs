using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameEntities.CustomEntities;

namespace BubbleShooter.Scripts.Gameplay.GameHandlers
{
    public class BallShooter : MonoBehaviour
    {
        [SerializeField] private CommonBall prefab;
        [SerializeField] private Transform pointer;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Vector2 normalizePosition;

        private Vector2 _direction;

        private void Awake()
        {
            Vector3 startPosition = mainCamera.ViewportToWorldPoint(normalizePosition);
            startPosition.z = 0;
            transform.position = startPosition;
        }

        private void Update()
        {
            GetMouseDirection();

            if (inputHandler.IsMousePress)
            {
                SpawnABall();
            }

            RotatePointer();
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
