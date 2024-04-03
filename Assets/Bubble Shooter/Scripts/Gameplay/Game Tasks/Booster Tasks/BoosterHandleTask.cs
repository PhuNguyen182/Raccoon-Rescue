using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks
{
    public class BoosterHandleTask : IDisposable
    {
        private readonly BreakGridTask _breakGridTask;
        private readonly GridCellManager _gridCellManager;

        private readonly FireBallBoosterTask _fireBallBoosterTask;
        private readonly LeafBallBoosterTask _leafBallBoosterTask;
        private readonly SunBallBoosterTask _sunBallBoosterTask;
        private readonly WaterBallBoosterTask _waterBallBoosterTask;

        public BoosterHandleTask(BreakGridTask breakGridTask, GridCellManager gridCellManager)
        {
            _breakGridTask = breakGridTask;
            _gridCellManager = gridCellManager;

            _fireBallBoosterTask = new(_gridCellManager, _breakGridTask);
            _leafBallBoosterTask = new(_gridCellManager, _breakGridTask);
            _sunBallBoosterTask = new(_gridCellManager, _breakGridTask);
            _waterBallBoosterTask = new(_gridCellManager, _breakGridTask);
        }

        public async UniTask ActiveBooster(Vector3Int position)
        {
            IGridCell gridCell = _gridCellManager.Get(position);

            switch (gridCell.BallEntity.EntityType)
            {
                case EntityType.FireBall:
                    await _fireBallBoosterTask.Execute(position);
                    break;
                case EntityType.LeafBall:
                    await _leafBallBoosterTask.Execute(position);
                    break;
                case EntityType.WaterBall:
                    await _waterBallBoosterTask.Execute(position);
                    break;
                case EntityType.SunBall:
                    await _sunBallBoosterTask.Execute(position);
                    break;
            }
        }

        public void Dispose()
        {
            
        }
    }
}
