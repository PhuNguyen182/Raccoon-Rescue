using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Constants;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks
{
    public class LeafBallBoosterTask : IDisposable, IBoosterTask
    {
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;

        public LeafBallBoosterTask(GridCellManager gridCellManager, BreakGridTask breakGridTask)
        {
            _gridCellManager = gridCellManager;
            _breakGridTask = breakGridTask;
        }

        public async UniTask Execute(Vector3Int position)
        {
            IGridCell boosterCell = _gridCellManager.Get(position);
            if (boosterCell.BallEntity is IBallBooster booster)
                await booster.Activate();

            _gridCellManager.DestroyAt(position);
            using (var listPool = ListPool<UniTask>.Get(out var breakTasks))
            {
                var column = GetVerticalLine(position);

                for (int i = 0; i < column.Count; i++)
                {
                    if (column[i] == null)
                        continue;

                    if (position == column[i].GridPosition)
                        continue;

                    if (!column[i].ContainsBall)
                        continue;

                    if (column[i].BallEntity is IBallBooster ballBooster)
                        if (ballBooster.IsIgnored)
                            continue;

                    breakTasks.Add(_breakGridTask.Break(column[i]));
                }

                await UniTask.WhenAll(breakTasks);
                column.Clear();
            }
        }

        private List<IGridCell> GetVerticalLine(Vector3Int startPosition)
        {
            List<IGridCell> column = new();

            for (int i = 1; i < BoosterConstants.LeafBoosterAttackRange - 1; i++)
            {
                IGridCell gridCell = _gridCellManager.Get(startPosition + Vector3Int.up * i);
                column.Add(gridCell);
            }

            return column;
        }

        public void Dispose()
        {
            
        }
    }
}
