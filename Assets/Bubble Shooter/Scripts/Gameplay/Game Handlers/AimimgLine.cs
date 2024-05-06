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

        private Vector2 _direction;
        private Vector3[] _linePoints = new Vector3[3];

        public void DrawAimingLine(bool isDraw)
        {
            if(!isDraw)
            {
                lineDrawer.HidePath();
                return;
            }

            _direction = inputHandler.InputPosition - spawnPoint.position;
            RaycastHit2D ceilHit = Physics2D.Raycast(spawnPoint.position, _direction, 25, ceilMask);

            if (ceilHit)
                _linePoints = new Vector3[] { spawnPoint.position, ceilHit.point };

            else
            {
                RaycastHit2D ballHit = Physics2D.Raycast(spawnPoint.position, _direction, 25, ballMask);

                if (ballHit)
                    _linePoints = new Vector3[] { spawnPoint.position, ballHit.point };

                else DrawReflectLine();
            }

            lineDrawer.ShowPath(_linePoints);
        }

        private void DrawReflectLine()
        {
            RaycastHit2D reflectHit = Physics2D.Raycast(spawnPoint.position, _direction, 25, reflectMask);

            if (reflectHit)
            {
                Vector3 hitPoint = reflectHit.point;
                Vector2 reflectDir = Vector2.Reflect(hitPoint - spawnPoint.position, reflectHit.normal);
                Ray2D reflectRay = new(hitPoint, reflectDir);
                RaycastHit2D ceilHit = Physics2D.Raycast(hitPoint, reflectDir, 25, ceilMask);

                if (ceilHit)
                {
                    _linePoints = new Vector3[] { spawnPoint.position, reflectHit.point, ceilHit.point };
                }

                else
                {
                    RaycastHit2D ballHit = Physics2D.Raycast(hitPoint, reflectDir, 25, ballMask);

                    _linePoints = ballHit ? new Vector3[] { spawnPoint.position, reflectHit.point, ballHit.point }
                                          : new Vector3[] { spawnPoint.position, reflectHit.point, reflectRay.GetPoint(25f) };
                }
            }
        }
    }
}
