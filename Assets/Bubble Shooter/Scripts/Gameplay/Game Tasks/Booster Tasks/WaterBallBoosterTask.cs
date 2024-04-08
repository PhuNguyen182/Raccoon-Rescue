using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks
{
    public class WaterBallBoosterTask : IDisposable, IBoosterTask
    {
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;

        public WaterBallBoosterTask(GridCellManager gridCellManager, BreakGridTask breakGridTask)
        {
            _gridCellManager = gridCellManager;
            _breakGridTask = breakGridTask;
        }

        // To do: Destroy a horizontal line of balls
        public async UniTask Execute(Vector3Int position)
        {
            IGridCell boosterCell = _gridCellManager.Get(position);
            if (boosterCell.BallEntity is IBallBooster booster)
                await booster.Activate();

            using (var listPool = ListPool<UniTask>.Get(out var breakTasks))
            {
                _gridCellManager.GetRow(position, out List<IGridCell> row);

                for (int i = 0; i < row.Count; i++)
                {
                    if (position == row[i].GridPosition)
                        continue;

                    if (row[i].ContainsBall)
                        breakTasks.Add(_breakGridTask.Break(row[i]));
                }

                await UniTask.WhenAll(breakTasks);
            }

            _gridCellManager.DestroyAt(position);
        }

        public void Dispose()
        {
            
        }
    }
}
