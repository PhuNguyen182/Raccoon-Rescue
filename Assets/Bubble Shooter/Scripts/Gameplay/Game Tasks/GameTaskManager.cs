using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks;
using BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks;
using BubbleShooter.Scripts.Gameplay.Strategies;
using BubbleShooter.Scripts.GameUI.Screens;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.Common.Factories;

namespace BubbleShooter.Scripts.Gameplay.GameTasks 
{
    public class GameTaskManager : IDisposable
    {
        private readonly InputProcessor _inputProcessor;
        private readonly MainScreenManager _mainScreenManager;
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;
        private readonly MatchBallHandler _matchBallHandler;
        private readonly CheckBallClusterTask _checkBallClusterTask;
        private readonly BoosterHandleTask _boosterHandleTask;
        private readonly InGamePowerupControlTask _powerupControlTask;
        private readonly GameStateController _gameStateController;
        private readonly BallRippleTask _ballRippleTask;
        private readonly EndGameTask _endGameTask;
        private readonly IDisposable _disposable;

        public GameTaskManager(GridCellManager gridCellManager, MainScreenManager mainScreenManager, InputProcessor inputProcessor
            , CheckTargetTask checkTargetTask, CheckScoreTask checkScoreTask, BallProvider ballProvider, BallShooter ballShooter
            , MetaBallManager metaBallManager, GameDecorator gameDecorator, MoveGameViewTask moveGameViewTask
            , IngameBoosterHandler ingameBoosterHandler)
        {
            DisposableBuilder builder = Disposable.CreateBuilder();

            _inputProcessor = inputProcessor;
            _gridCellManager = gridCellManager;
            _gridCellManager.AddTo(ref builder);

            _mainScreenManager = mainScreenManager;
            _powerupControlTask = new(_mainScreenManager.IngamePowerupPanel, ballShooter, _inputProcessor);
            _powerupControlTask.AddTo(ref builder);
            ballShooter.SetIngamePowerup(_powerupControlTask);

            _ballRippleTask = new(_gridCellManager);
            _ballRippleTask.AddTo(ref builder);

            _breakGridTask = new(_gridCellManager);
            _checkBallClusterTask = new(_gridCellManager, _breakGridTask);
            _boosterHandleTask = new(_breakGridTask, _gridCellManager, _checkBallClusterTask, _inputProcessor);

            _breakGridTask.SetBoosterHandleTask(_boosterHandleTask);
            _boosterHandleTask.AddTo(ref builder);

            _matchBallHandler = new(_gridCellManager, _breakGridTask, _checkBallClusterTask, _inputProcessor
                                    , checkTargetTask, moveGameViewTask, _ballRippleTask, ingameBoosterHandler);
            _matchBallHandler.AddTo(ref builder);

            _endGameTask = new(metaBallManager, ballShooter, ballProvider, checkTargetTask, mainScreenManager.NotificationPanel);
            _endGameTask.AddTo(ref builder);

            _gameStateController = new(_endGameTask, _mainScreenManager, checkTargetTask
                                       , checkScoreTask, gameDecorator, _inputProcessor);
            _gameStateController.AddTo(ref builder);
            _matchBallHandler.SetGameStateController(_gameStateController);

            _disposable = builder.Build();
        }

        public void SetBallMaterialEndGame(Material material)
        {
            _endGameTask.SetMaterial(material);
            _endGameTask.ResetBallColor();
        }

        public void SetInputActive(bool active)
        {
            _inputProcessor.IsActive = active;
        }

        public void Dispose()
        {
            _endGameTask.ResetBallColor();
            _disposable.Dispose();
        }
    } 
}
