using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class BreakGridTask
    {
        private readonly GridCellManager _gridCellManager;
        private BoosterHandleTask _boosterHandleTask;

        public BreakGridTask(GridCellManager gridCellManager)
        {
            _gridCellManager = gridCellManager;
        }

        public void SetBoosterHandleTask(BoosterHandleTask boosterHandleTask)
        {
            _boosterHandleTask = boosterHandleTask;
        }

        public async UniTask Break(IGridCell gridCell)
        {
            if (gridCell == null)
                return;

            IBallEntity ballEntity = gridCell.BallEntity;

            if (ballEntity == null)
                return;

            if (ballEntity is IBallBooster booster)
            {
                await booster.Explode();
                _boosterHandleTask.ActiveBooster(gridCell.GridPosition).Forget();
            }

            if (ballEntity is IBreakable breakable)
            {
                if (breakable.Break())
                {
                    await ballEntity.Blast();
                    _gridCellManager.DestroyAt(gridCell.GridPosition);
                }
            }
        }
    }
}
