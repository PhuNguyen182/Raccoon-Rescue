using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.GameBoard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameHandlers
{
    public class AimingLine : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private LineDrawer lineDrawer;

        [Header("Hit Layers")]
        [SerializeField] private LayerMask ceilMask;
        [SerializeField] private LayerMask ballMask;
        [SerializeField] private LayerMask gridMask;
        [SerializeField] private LayerMask reflectMask;

        private RaycastHit2D _ceilHit;
        private RaycastHit2D _ballHit;
        private RaycastHit2D _reflectHit;

        private Vector2 _direction;
        private Vector3[] _linePoints = new Vector3[3];

        private const float CheckGridOffset = 0.35f;

        public void DrawAimingLine(bool isDraw)
        {
            if(!isDraw)
            {
                lineDrawer.HidePath();
                return;
            }

            _direction = inputHandler.InputPosition - spawnPoint.position;
            _ceilHit = Physics2D.Raycast(spawnPoint.position, _direction, 25, ceilMask);

            if (_ceilHit)
                _linePoints = new Vector3[] { spawnPoint.position, _ceilHit.point };

            else
            {
                _ballHit = Physics2D.Raycast(spawnPoint.position, _direction, 25, ballMask);

                if (_ballHit)
                    _linePoints = new Vector3[] { spawnPoint.position, _ballHit.point };

                else DrawReflectLine();
            }

            lineDrawer.ShowPath(_linePoints);
        }

        private void DrawReflectLine()
        {
            _reflectHit = Physics2D.Raycast(spawnPoint.position, _direction, 25, reflectMask);

            if (_reflectHit)
            {
                Vector3 hitPoint = _reflectHit.point;
                Vector2 reflectDir = Vector2.Reflect(hitPoint - spawnPoint.position, _reflectHit.normal);

                Ray2D reflectRay = new(hitPoint, reflectDir);
                _ceilHit = Physics2D.Raycast(hitPoint, reflectDir, 25, ceilMask);

                if (_ceilHit)
                {
                    _linePoints = new Vector3[] { spawnPoint.position, _reflectHit.point, _ceilHit.point };
                }

                else
                {
                    _ballHit = Physics2D.Raycast(hitPoint, reflectDir, 25, ballMask);

                    _linePoints = _ballHit ? new Vector3[] { spawnPoint.position, _reflectHit.point, _ballHit.point }
                                          : new Vector3[] { spawnPoint.position, _reflectHit.point, reflectRay.GetPoint(25f) };
                }
            }
        }

        public GridCellHolder ReportGridCell(Vector3 position, Vector3 dir)
        {
            Vector3 checkPosition = position - CheckGridOffset * dir.normalized;
            Collider2D cellCollider = Physics2D.OverlapPoint(checkPosition, gridMask);
            return cellCollider.TryGetComponent(out GridCellHolder holder) ? holder : null;
        }
    }
}
