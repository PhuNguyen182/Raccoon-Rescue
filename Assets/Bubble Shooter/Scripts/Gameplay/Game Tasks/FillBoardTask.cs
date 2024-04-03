using BubbleShooter.Scripts.Gameplay.Strategies;
using System.Collections;
using System.Collections.Generic;
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

        }
    }
}
