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
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private TrajactoryLine trajactoryLine;
        
        [Space(10)]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Vector2 normalizePosition;
        [SerializeField] private LayerMask leftReflect;
        [SerializeField] private LayerMask rightReflect;

        private bool _isReflect = false;
        private RaycastHit2D _hitWall1;
        private RaycastHit2D _hitWall2;

        private Vector2 _direction;
        private Vector3[] _linePoints = new Vector3[3];

        private void Awake()
        {
            Vector3 startPosition = mainCamera.ViewportToWorldPoint(normalizePosition);
            startPosition.z = 0;
            transform.position = startPosition;
        }

        private void Update()
        {
            GetMouseDirection();

            if (inputHandler.IsMouseUp)
            {
                SpawnABall();
            }

            DrawLine(inputHandler.IsMouseHold);
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

        private void DrawLine(bool isDraw)
        {
            if (isDraw)
            {
                _linePoints[0] = spawnPoint.position;
                Ray2D ray1 = new(spawnPoint.position, _direction);
                _hitWall1 = Physics2D.Raycast(spawnPoint.position, _direction, 25);

                if (_hitWall1)
                {
                    _linePoints[1] = _hitWall1.point;
                    Vector2 reflectDir = Vector2.Reflect(_linePoints[1] - _linePoints[0], _hitWall1.normal);
                    Ray2D ray2 = new(_linePoints[1], reflectDir);
                    
                    LayerMask secondMask = (leftReflect.value & (1 << _hitWall1.collider.gameObject.layer)) > 0 ? rightReflect : leftReflect;
                    _hitWall2 = Physics2D.Raycast(_linePoints[1], reflectDir, 25, secondMask);
                    
                    if (_hitWall2)
                        _linePoints[2] = _hitWall2.point;
                    else
                        _linePoints[2] = ray2.GetPoint(5);
                }

                else
                {
                    _linePoints[1] = ray1.GetPoint(25);
                    _linePoints[2] = _linePoints[1];
                }

                trajactoryLine.ShowPath(_linePoints);
            }

            else
                trajactoryLine.HidePath();
        }
    }
}
