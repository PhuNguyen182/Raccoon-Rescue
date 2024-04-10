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

        // To do: Destroy a triple of ball
        public async UniTask Execute(Vector3Int position)
        {
            IGridCell boosterCell = _gridCellManager.Get(position);
            if (boosterCell.BallEntity is IBallBooster booster)
                await booster.Activate();
            
            using (var listPool = ListPool<UniTask>.Get(out var breakTasks))
            {
                List<Vector3Int> gridPosition = GetTripleBall(position);
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
            }
        }

        private List<Vector3Int> GetTripleBall(Vector3Int position)
        {
            List<Vector3Int> triplePosition = new();

            for (int i = 0; i < CommonProperties.MaxNeighborCount; i++)
            {
                Vector3Int neighborOffset = CommonProperties.NeighborOffsets[i];
                IGridCell gridCell = _gridCellManager.Get(position + neighborOffset);

                if (triplePosition.Count == 3)
                    break;

                if (gridCell == null)
                    continue;

                if (!gridCell.ContainsBall)
                    continue;

                triplePosition.Add(neighborOffset);
            }

            return triplePosition;
        }

        public void Dispose()
        {
            
        }
    }
}
