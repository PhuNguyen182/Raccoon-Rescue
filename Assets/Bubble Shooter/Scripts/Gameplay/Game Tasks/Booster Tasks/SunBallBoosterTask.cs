using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine.Pool;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks
{
    public class SunBallBoosterTask : IDisposable, IBoosterTask
    {
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;

        public SunBallBoosterTask(GridCellManager gridCellManager, BreakGridTask breakGridTask)
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
                var gridPosition = GetHexagonClusterBall(position);
                for (int i = 0; i < gridPosition.Count; i++)
                {
                    if (position == gridPosition[i])
                        continue;

                    IGridCell cell = _gridCellManager.Get(gridPosition[i]);
                    if (cell == null)
                        continue;

                    if (!cell.ContainsBall)
                        continue;

                    if (cell.BallEntity is IBallBooster ballBooster)
                        if (ballBooster.IsIgnored)
                            continue;

                    breakTasks.Add(_breakGridTask.Break(cell));
                }

                await UniTask.WhenAll(breakTasks);
                gridPosition.Clear();
            }
        }

        private List<Vector3Int> GetHexagonClusterBall(Vector3Int position)
        {
            List<Vector3Int> triplePosition = new();

            for (int i = 0; i < CommonProperties.MaxNeighborCount; i++)
            {
                Vector3Int neighborOffset = position.y % 2 == 0 
                                            ? CommonProperties.EvenYNeighborOffsets[i]
                                            : CommonProperties.OddYNeighborOffsets[i];
                triplePosition.Add(position + neighborOffset);
            }

            return triplePosition;
        }

        public void Dispose()
        {
            
        }
    }
}
