using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomEntities
{
    public class CommonBall : BaseBall, IFixedUpdateHandler, IBallMovement
    {
        [SerializeField] private BallMovement ballMovement;

        public bool CanMove 
        { 
            get => ballMovement.CanMove; 
            set => ballMovement.CanMove = value; 
        }

        private void OnEnable()
        {
            UpdateHandlerManager.Instance.AddFixedUpdateBehaviour(this);
        }

        public void OnFixedUpdate()
        {
            if(CanMove && !IsFixedOnStart)
            {
                ballMovement.Move();
            }
        }

        public void SetBodyActive(bool active)
        {
            ballMovement.SetBodyActive(active);
        }

        public void SetMoveDirection(Vector2 direction)
        {
            ballMovement.SetMoveDirection(direction);
        }

        public UniTask SnapTo(Vector3 position)
        {
            return ballMovement.SnapTo(position);
        }

        public override void Blast()
        {
            
        }

        public override bool Break()
        {
            return true;
        }

        public override void Destroy()
        {
            SimplePool.Despawn(this.gameObject);
        }

        private void OnDisable()
        {
            UpdateHandlerManager.Instance.RemoveFixedUpdateBehaviour(this);
        }
    }
}
