using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.Strategies;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class FillBoardTask
    {
        private readonly GridCellManager _gridCellManager;
        private readonly MetaBallManager _metaBallManager;

        public FillBoardTask(GridCellManager gridCellManager, MetaBallManager metaBallManager)
        {
            _gridCellManager = gridCellManager;
            _metaBallManager = metaBallManager;
        }

        public void Fill()
        {
            var positions = _metaBallManager.Iterator().ToList();
            
            for (int i = 0; i < positions.Count; i++)
            {
                IGridCell gridCell = _gridCellManager.Get(positions[i]);
                IBallEntity ballEntity = _metaBallManager.Get(positions[i]);

                ballEntity.CheckCeilAttach();
                gridCell.SetBall(ballEntity);
            }

            _gridCellManager.Encapsulate();
        }
    }
}
