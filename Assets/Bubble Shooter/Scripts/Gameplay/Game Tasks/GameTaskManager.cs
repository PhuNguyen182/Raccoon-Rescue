using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks;
using BubbleShooter.Scripts.Gameplay.GameHandlers;

namespace BubbleShooter.Scripts.Gameplay.GameTasks 
{
    public class GameTaskManager : IDisposable
    {
        private readonly InputProcessor _inputProcessor;
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;
        private readonly MatchBallHandler _matchBallHandler;
        private readonly CheckBallClusterTask _checkBallClusterTask;
        private readonly BoosterHandleTask _boosterHandleTask;
        private readonly InGamePowerupControlTask _powerupControlTask;
        private readonly IDisposable _disposable;

        public GameTaskManager(GridCellManager gridCellManager, InputHandler inputHandler)
        {
            DisposableBuilder builder = Disposable.CreateBuilder();

            _gridCellManager = gridCellManager;
            _gridCellManager.AddTo(ref builder);

            _inputProcessor = new(inputHandler, _gridCellManager);
            _inputProcessor.AddTo(ref builder);

            _powerupControlTask = new();
            _powerupControlTask.AddTo(ref builder);

            _breakGridTask = new(_gridCellManager);
            _checkBallClusterTask = new(_gridCellManager, _breakGridTask);
            _boosterHandleTask = new(_breakGridTask, _gridCellManager, _checkBallClusterTask);

            _breakGridTask.SetBoosterHandleTask(_boosterHandleTask);
            _boosterHandleTask.AddTo(ref builder);

            _matchBallHandler = new(_gridCellManager, _breakGridTask, _checkBallClusterTask);
            _matchBallHandler.AddTo(ref builder);

            _disposable = builder.Build();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    } 
}
