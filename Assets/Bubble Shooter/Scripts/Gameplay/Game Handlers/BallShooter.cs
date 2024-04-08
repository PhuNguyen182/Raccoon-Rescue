using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls;
using BubbleShooter.Scripts.Common.Factories;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.GameDatas;
using BubbleShooter.Scripts.Gameplay.GameEntities;

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
        private EntityType _ballColor;
        private float _limitAngleSine = 0;

        private CancellationToken _token;
        private EntityFactory _entityFactory;

        private void Awake()
        {
            SetColor(EntityType.FireBall);
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

        public void SetBallFactory(EntityFactory factory)
        {
            _entityFactory = factory;
        }

        public void SetStartPosition()
        {
            _startPosition = mainCamera.ViewportToWorldPoint(normalizePosition);
            _startPosition.z = 0;
            transform.position = _startPosition;
        }

        public void SetColor(EntityType ballColor)
        {
            _ballColor = ballColor;
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

            BaseEntity newBall = _entityFactory.Create(new EntityMapData 
            {
                HP = 1,
                EntityType = _ballColor 
            });

            newBall.IsFixedOnStart = false;
            newBall.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.identity);
            
            if (newBall.TryGetComponent(out IBallMovement ballMovement) && newBall.TryGetComponent(out IBallPhysics ballPhysics))
            {
                ballMovement.CanMove = false;
                ballMovement.MovementState = BallMovementState.Ready;
                ballPhysics.ChangeLayerMask(false);

                ballPhysics.SetBodyActive(false);
                ballMovement.SetMoveDirection(_direction);
                ballMovement.MovementState = BallMovementState.Moving;
                ballMovement.CanMove = true;
            }
        }

        private void RotatePointer()
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
            pointer.rotation = Quaternion.Euler(0, 0, angle);
            _limitAngleSine = Mathf.Sin((angle + 90f) / Mathf.Rad2Deg);
        }
    }
}
