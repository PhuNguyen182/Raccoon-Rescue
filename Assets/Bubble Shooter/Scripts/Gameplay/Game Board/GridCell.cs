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

        public Vector3Int GridPosition { get; set; }

        public GridCell(Vector3Int position, IBallEntity ballEntity)
        {
            _ballEntity = ballEntity;
            GridPosition = position;
        }

        public void Destroy()
        {
            if(_ballEntity != null)
            {
                _ballEntity.Destroy();
                _ballEntity = null;
            }
        }

        public void SetBall(IBallEntity ball)
        {
            _ballEntity = ball;
        }
    }
}
