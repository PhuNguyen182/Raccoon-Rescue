using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls;
using BubbleShooter.Scripts.Utils.BoundsUtils;
using System.Linq;

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
        private float _limitAngleSine = 0;

        private void Awake()
        {
            SetStartPosition();

            Vector3Int position = Vector3Int.zero;
            BoundsInt bounds = position.GetBounds2D(5);
            var positions = bounds.Iterator();
            BoundsInt newBounds = BoundsExtension.Encapsulate(positions.ToList());
            foreach (var item in newBounds.Iterator())
            {
                Debug.Log(item);
            }
        }

        private void Update()
        {
            GetMouseDirection();

            if (inputHandler.IsMouseUp && _limitAngleSine > 0.15f)
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

            ball.CanMove = false;
            ball.SetBodyActive(false);
            ball.SetMoveDirection(_direction);
            ball.CanMove = true;
        }

        private void RotatePointer()
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
            pointer.rotation = Quaternion.Euler(0, 0, angle);
            _limitAngleSine = Mathf.Sin((angle + 90f) / Mathf.Rad2Deg);
        }
    }
}
