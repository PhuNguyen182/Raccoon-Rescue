using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameHandlers
{
    public class LineDrawer : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private TrajactoryLine trajactoryLine;

        [Header("Wall Layers")]
        [SerializeField] private LayerMask leftReflect;
        [SerializeField] private LayerMask rightReflect;

        private RaycastHit2D _hitWall1;
        private RaycastHit2D _hitWall2;

        private Vector2 _direction;
        private Vector3[] _linePoints = new Vector3[3];

        public void DrawLine(bool isDraw)
        {
            if (isDraw)
            {
                _linePoints[0] = spawnPoint.position;
                _direction = inputHandler.InputPosition - spawnPoint.position;
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
