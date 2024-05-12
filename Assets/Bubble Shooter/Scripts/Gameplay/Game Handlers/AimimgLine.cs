using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Constants;
using BubbleShooter.Scripts.Gameplay.GameBoard;
using BubbleShooter.Scripts.Gameplay.Inputs;

namespace BubbleShooter.Scripts.Gameplay.GameHandlers
{
    public class AimingLine : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private InputController inputHandler;
        [SerializeField] private LineDrawer mainLineDrawer;

        private const float RaycastLength = 25f;
        private const float NormalAimLength = 1.5f;
        private const float PremierAimLength = 25f;

        private float _angle = 0;

        private LayerMask _ceilMask;
        private LayerMask _ballMask;
        private LayerMask _gridMask;
        private LayerMask _reflectMask;

        private RaycastHit2D _ceilHit;
        private RaycastHit2D _ballHit;
        private RaycastHit2D _reflectHit;

        private Vector2 _direction, _originalDir;
        private Vector3[] _linePoints = new Vector3[3];

        public bool IsPremier { get; set; }

        private void Awake()
        {
            _ceilMask = LayerMask.GetMask(BallConstants.CeilLayerName);
            _ballMask = LayerMask.GetMask(BallConstants.BallLayerName);
            _gridMask = LayerMask.GetMask(BallConstants.GridLayerName);
            _reflectMask = LayerMask.GetMask(BallConstants.ReflectLayerName);
        }

        public void SetAngle(float angle)
        {
            _angle = angle;
        }

        public void DrawAimingLine(bool isDraw, Color lineColor)
        {
            if(!isDraw)
            {
                _angle = 0;
                mainLineDrawer.HidePath();
                return;
            }

            mainLineDrawer.SetColor(lineColor);
            _originalDir = inputHandler.Pointer - spawnPoint.position;
            _direction = Quaternion.AngleAxis(_angle, Vector3.forward) * _originalDir;
            _ceilHit = Physics2D.Raycast(spawnPoint.position, _direction, RaycastLength, _ceilMask);

            if (_ceilHit)
                _linePoints = new Vector3[] { spawnPoint.position, _ceilHit.point };

            else
            {
                _ballHit = Physics2D.Raycast(spawnPoint.position, _direction, RaycastLength, _ballMask);

                if (_ballHit)
                    _linePoints = new Vector3[] { spawnPoint.position, _ballHit.point };

                else DrawReflectLine();
            }

            mainLineDrawer.ShowPath(_linePoints);
        }

        private void DrawReflectLine()
        {
            _reflectHit = Physics2D.Raycast(spawnPoint.position, _direction, RaycastLength, _reflectMask);

            if (_reflectHit)
            {
                Vector3 hitPoint = _reflectHit.point;
                Vector2 reflectDir = Vector2.Reflect(hitPoint - spawnPoint.position, _reflectHit.normal);

                Ray2D reflectRay = new(hitPoint, reflectDir);
                _ceilHit = Physics2D.Raycast(hitPoint, reflectDir, RaycastLength, _ceilMask);

                if (_ceilHit)
                {
                    _linePoints = new Vector3[] { spawnPoint.position, _reflectHit.point, _ceilHit.point };
                }

                else
                {
                    float extendLength = IsPremier ? PremierAimLength : NormalAimLength;
                    _ballHit = Physics2D.Raycast(hitPoint, reflectDir, extendLength, _ballMask);

                    _linePoints = _ballHit ? new Vector3[] { spawnPoint.position, _reflectHit.point, _ballHit.point }
                                          : new Vector3[] { spawnPoint.position, _reflectHit.point, reflectRay.GetPoint(extendLength) };
                }
            }
        }

        public GridCellHolder ReportGridCell()
        {
            Vector3 dir, position;
            if (_linePoints.Length == 3)
            {
                position = _linePoints[2];
                dir = _linePoints[1] - _linePoints[2];
            }

            else if (_linePoints.Length == 2)
            {
                position = _linePoints[1];
                dir = _linePoints[0] - _linePoints[1];
            }

            else return null;

            Vector3 checkPosition = position + BallConstants.CheckGridOffset * dir.normalized;
            Collider2D cellCollider = Physics2D.OverlapPoint(checkPosition, _gridMask);
            
            if (cellCollider == null)
                return null;

            return cellCollider.TryGetComponent(out GridCellHolder holder) ? holder : null;
        }
    }
}
