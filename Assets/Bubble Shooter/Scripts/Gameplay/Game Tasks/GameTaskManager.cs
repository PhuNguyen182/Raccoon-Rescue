using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks;
using BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.GameUI.Screens;

namespace BubbleShooter.Scripts.Gameplay.GameTasks 
{
    public class GameTaskManager : IDisposable
    {
        private readonly MainScreenManager _mainScreenManager;
        private readonly InputProcessor _inputProcessor;
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;
        private readonly MatchBallHandler _matchBallHandler;
        private readonly CheckBallClusterTask _checkBallClusterTask;
        private readonly BoosterHandleTask _boosterHandleTask;
        private readonly InGamePowerupControlTask _powerupControlTask;
        private readonly IngameBoosterHandler _ingameBoosterHandler;
        private readonly GameStateController _gameStateController;
        private readonly IDisposable _disposable;

        public GameTaskManager(GridCellManager gridCellManager, InputHandler inputHandler, MainScreenManager mainScreenManager,
            CheckTargetTask checkTargetTask, BallShooter ballShooter)
        {
            DisposableBuilder builder = Disposable.CreateBuilder();

            _gridCellManager = gridCellManager;
            _gridCellManager.AddTo(ref builder);

            _inputProcessor = new(inputHandler);
            _inputProcessor.AddTo(ref builder);

            _mainScreenManager = mainScreenManager;
            _powerupControlTask = new(_mainScreenManager.IngamePowerupPanel, ballShooter);
            _powerupControlTask.AddTo(ref builder);

            _breakGridTask = new(_gridCellManager);
            _checkBallClusterTask = new(_gridCellManager, _breakGridTask, _inputProcessor);
            _boosterHandleTask = new(_breakGridTask, _gridCellManager, _checkBallClusterTask, _inputProcessor);

            _ingameBoosterHandler = new(ballShooter);
            _breakGridTask.SetBoosterHandleTask(_boosterHandleTask);
            _boosterHandleTask.AddTo(ref builder);

            _matchBallHandler = new(_gridCellManager, _breakGridTask, _checkBallClusterTask, _inputProcessor);
            _matchBallHandler.AddTo(ref builder);

            _gameStateController = new(_mainScreenManager.EndGameScreen, checkTargetTask);
            _gameStateController.AddTo(ref builder);

            _disposable = builder.Build();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    } 
}
