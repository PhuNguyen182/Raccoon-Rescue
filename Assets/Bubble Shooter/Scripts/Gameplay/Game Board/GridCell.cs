using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Gameplay.GameBoard 
{
    public class GridCell : IGridCell
    {
        private IBallEntity _ballEntity;

        public IBallEntity BallEntity => _ballEntity;

        public EntityType EntityType => _ballEntity.EntityType;

        public Vector3 WorldPosition { get; set; }
        public Vector3Int GridPosition { get; set; }

        public bool Destroy()
        {
            if(_ballEntity != null)
            {
                _ballEntity.Destroy();
                _ballEntity = null;
                return true;
            }

            return false;
        }

        public void SetBall(IBallEntity ball)
        {
            _ballEntity = ball;
            
            if(_ballEntity != null)
            {
                ball.GridPosition = GridPosition;
                ball.SetWorldPosition(WorldPosition);
            }
        }
    }
}
