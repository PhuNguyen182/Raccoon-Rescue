using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

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
            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            
        }
    }
}
