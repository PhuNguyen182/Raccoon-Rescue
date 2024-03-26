using BubbleShooter.Scripts.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class BreakGridTask
    {
        private readonly GridCellManager _gridCellManager;

        public BreakGridTask(GridCellManager gridCellManager)
        {
            _gridCellManager = gridCellManager;
        }

        public bool Break(Vector3Int position)
        {
            IGridCell gridCell = _gridCellManager.Get(position);
            
            if (gridCell == null)
                return false;

            IBallEntity ballEntity = gridCell.BallEntity;

            if (ballEntity == null)
                return false;

            if (ballEntity is IBreakable breakable)
                return breakable.Break();

            return false;
        }

        public bool Break(IGridCell gridCell)
        {
            if (gridCell == null)
                return false;

            IBallEntity ballEntity = gridCell.BallEntity;

            if (ballEntity == null)
                return false;

            if (ballEntity is IBreakable breakable)
                return breakable.Break();

            return false;
        }
    }
}
