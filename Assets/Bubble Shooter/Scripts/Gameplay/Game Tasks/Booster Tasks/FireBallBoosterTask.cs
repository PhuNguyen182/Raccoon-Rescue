using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Utils.BoundsUtils;
using System.Linq;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks
{
    public class FireBallBoosterTask : IDisposable, IBoosterTask
    {
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;

        public FireBallBoosterTask(GridCellManager gridCellManager, BreakGridTask breakGridTask)
        {
            _gridCellManager = gridCellManager;
            _breakGridTask = breakGridTask;
        }

        // To do: Destroy a big area of ball (5x5 perhaps ?!!)
        public async UniTask Execute(Vector3Int position)
        {
            Vector3Int[] gridPosition = GetBoosterRange(position);

            for (int i = 0; i < gridPosition.Length; i++)
            {
                IGridCell cell = _gridCellManager.Get(gridPosition[i]);
                _breakGridTask.Break(cell);
            }

            await UniTask.CompletedTask;
        }

        private Vector3Int[] GetBoosterRange(Vector3Int position)
        {
            BoundsInt boosterRange = position.GetBounds2D(5);
            return boosterRange.Iterator().ToArray();
        }

        public void Dispose()
        {

        }
    }
}
