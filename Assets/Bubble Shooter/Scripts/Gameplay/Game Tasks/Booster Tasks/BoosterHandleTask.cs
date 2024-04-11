using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks
{
    public class BoosterHandleTask : IDisposable
    {
        private readonly BreakGridTask _breakGridTask;
        private readonly GridCellManager _gridCellManager;
        private readonly CheckBallClusterTask _checkBallClusterTask;

        private readonly FireBallBoosterTask _fireBallBoosterTask;
        private readonly LeafBallBoosterTask _leafBallBoosterTask;
        private readonly SunBallBoosterTask _sunBallBoosterTask;
        private readonly WaterBallBoosterTask _waterBallBoosterTask;
        private readonly ISubscriber<ActiveBoosterMessage> _boosterSubscriber;

        private IDisposable _disposable;

        public BoosterHandleTask(BreakGridTask breakGridTask, GridCellManager gridCellManager, CheckBallClusterTask checkBallClusterTask)
        {
            _breakGridTask = breakGridTask;
            _checkBallClusterTask = checkBallClusterTask;
            _gridCellManager = gridCellManager;

            _fireBallBoosterTask = new(_gridCellManager, _breakGridTask);
            _leafBallBoosterTask = new(_gridCellManager, _breakGridTask);
            _sunBallBoosterTask = new(_gridCellManager, _breakGridTask);
            _waterBallBoosterTask = new(_gridCellManager, _breakGridTask);

            DisposableBagBuilder builder = DisposableBag.CreateBuilder();

            _boosterSubscriber = GlobalMessagePipe.GetSubscriber<ActiveBoosterMessage>();
            _boosterSubscriber.Subscribe(message => ActiveBooster(message.Position).Forget()).AddTo(builder);

            _disposable = builder.Build();
        }

        public async UniTask ActiveBooster(Vector3Int position)
        {
            IGridCell gridCell = _gridCellManager.Get(position);
            
            if (gridCell == null)
                return;

            if (!gridCell.ContainsBall)
                return;

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

            _checkBallClusterTask.CheckCluster();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
