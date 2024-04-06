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
        private readonly BoosterHandleTask _boosterHandleTask;

        public BreakGridTask(GridCellManager gridCellManager, BoosterHandleTask boosterHandleTask)
        {
            _gridCellManager = gridCellManager;
            _boosterHandleTask = boosterHandleTask;
        }

        public async UniTask Break(Vector3Int position)
        {
            IGridCell gridCell = _gridCellManager.Get(position);
            
            if (gridCell == null)
                await UniTask.CompletedTask;

            IBallEntity ballEntity = gridCell.BallEntity;

            if (ballEntity == null)
                await UniTask.CompletedTask;

            if(ballEntity is IBallBooster)
                _boosterHandleTask.ActiveBooster(position).Forget();

            if (ballEntity is IBreakable breakable)
            {
                if (breakable.Break())
                {
                    await ballEntity.Blast();
                    _gridCellManager.DestroyAt(position);
                }
            }
        }

        public async UniTask Break(IGridCell gridCell)
        {
            if (gridCell == null)
                await UniTask.CompletedTask;

            IBallEntity ballEntity = gridCell.BallEntity;

            if (ballEntity == null)
                await UniTask.CompletedTask;

            if (ballEntity is IBallBooster)
                _boosterHandleTask.ActiveBooster(gridCell.GridPosition).Forget();

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
