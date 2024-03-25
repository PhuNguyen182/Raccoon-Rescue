using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Interfaces;

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

        // To do: Destroy a big area of ball (3x3 perhaps ?!!)
        public async UniTask Execute(Vector3Int position)
        {
            await UniTask.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}
