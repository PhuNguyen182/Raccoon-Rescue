using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            
        }
    }
}
