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
        private readonly InputProcessor _inputProcessor;
        private readonly BallRippleTask _ballRippleTask;
        private readonly MoveGameViewTask _moveGameViewTask;

        private readonly FireBallBoosterTask _fireBallBoosterTask;
        private readonly LeafBallBoosterTask _leafBallBoosterTask;
        private readonly SunBallBoosterTask _sunBallBoosterTask;
        private readonly WaterBallBoosterTask _waterBallBoosterTask;
        private readonly ISubscriber<ActiveBoosterMessage> _boosterSubscriber;

        private IDisposable _disposable;

        public BoosterHandleTask(BreakGridTask breakGridTask, GridCellManager gridCellManager
            , CheckBallClusterTask checkBallClusterTask, InputProcessor inputProcessor
            , BallRippleTask ballRippleTask, MoveGameViewTask moveGameViewTask)
        {
            _breakGridTask = breakGridTask;
            _checkBallClusterTask = checkBallClusterTask;
            _gridCellManager = gridCellManager;
            _inputProcessor = inputProcessor;
            _ballRippleTask = ballRippleTask;
            _moveGameViewTask = moveGameViewTask;

            _fireBallBoosterTask = new(_gridCellManager, _breakGridTask);
            _leafBallBoosterTask = new(_gridCellManager, _breakGridTask);
            _sunBallBoosterTask = new(_gridCellManager, _breakGridTask);
            _waterBallBoosterTask = new(_gridCellManager, _breakGridTask);

            DisposableBagBuilder builder = DisposableBag.CreateBuilder();

            _boosterSubscriber = GlobalMessagePipe.GetSubscriber<ActiveBoosterMessage>();
            _boosterSubscriber.Subscribe(message => ActivateBooster(message.Position, message.IsFlyBooster).Forget()).AddTo(builder);

            _disposable = builder.Build();
        }

        public async UniTask ActivateBooster(Vector3Int position, bool isFlyBooster)
        {
            IGridCell gridCell = _gridCellManager.Get(position);
            
            if (gridCell == null)
                return;

            if (!gridCell.ContainsBall)
                return;

            _inputProcessor.IsActive = false;
            switch (gridCell.BallEntity.EntityType)
            {
                case EntityType.FireBall:
                    if(isFlyBooster)
                        _ballRippleTask.RippleAt(position, 5).Forget();
                    
                    await _fireBallBoosterTask.Execute(position);
                    break;
                case EntityType.LeafBall:
                    if (isFlyBooster)
                        _ballRippleTask.RippleAt(position, 3).Forget();

                    await _leafBallBoosterTask.Execute(position);
                    break;
                case EntityType.WaterBall:
                    if (isFlyBooster)
                        _ballRippleTask.RippleAt(position, 4).Forget();

                    await _waterBallBoosterTask.Execute(position);
                    break;
                case EntityType.SunBall:
                    if (isFlyBooster)
                        _ballRippleTask.RippleAt(position, 3).Forget();

                    await _sunBallBoosterTask.Execute(position);
                    break;
            }

            _ballRippleTask.ResetRippleIgnore();
            _checkBallClusterTask.CheckFreeCluster();
            await _moveGameViewTask.Check();
            _inputProcessor.IsActive = true;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
