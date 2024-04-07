using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameHandlers
{
    public class BallShooter : MonoBehaviour
    {
        [SerializeField] private CommonBall prefab;

        [Space(10)]
        [SerializeField] private Transform pointer;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform ballContainer;

        [Space(10)]
        [SerializeField] private LineDrawer lineDrawer;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private MainCharacter mainCharacter;
        
        [Space(10)]
        [Header("Start Position Setting")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Vector2 normalizePosition;

        private Vector2 _direction;
        private Vector3 _startPosition;
        private float _limitAngleSine = 0;

        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        private void Update()
        {
            GetInputDirection();
            RotatePointer();

            if (inputHandler.IsMouseUp && _limitAngleSine > 0.15f)
            {
                SpawnABall().Forget();
            }

            if (lineDrawer.isActiveAndEnabled)
            {
                lineDrawer.DrawLine(inputHandler.IsMouseHold);
            }
        }

        public void SetStartPosition()
        {
            _startPosition = mainCamera.ViewportToWorldPoint(normalizePosition);
            _startPosition.z = 0;
            transform.position = _startPosition;
        }

        private void GetInputDirection()
        {
            _direction = inputHandler.MousePosition - spawnPoint.position;
        }

        private async UniTaskVoid SpawnABall()
        {
            mainCharacter.Shoot();
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.167f), cancellationToken: _token);
            if (_token.IsCancellationRequested)
                return;

            CommonBall ball = SimplePool.Spawn(prefab, ballContainer
                                               , spawnPoint.position
                                               , Quaternion.identity);
            ball.CanMove = false;
            ball.MovementState = BallMovementState.Ready;
            ball.ChangelayerMask(false);

            ball.SetBodyActive(false);
            ball.SetMoveDirection(_direction);
            ball.MovementState = BallMovementState.Moving;
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
