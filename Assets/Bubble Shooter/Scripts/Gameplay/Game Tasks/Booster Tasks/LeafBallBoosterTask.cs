using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BubbleShooter.Scripts.Common.Interfaces;
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

        // To do: Destroy a vertical line of ball
        public async UniTask Execute(Vector3Int position)
        {
            using (var listPool = ListPool<UniTask>.Get(out var breakTasks))
            {
                _gridCellManager.GetColumn(position, out List<IGridCell> column);

                for (int i = 0; i < column.Count; i++)
                {
                    if (column[i].ContainsBall)
                        breakTasks.Add(_breakGridTask.Break(column[i]));
                }

                await UniTask.WhenAll(breakTasks);
            }
        }

        public void Dispose()
        {
            
        }
    }
}
