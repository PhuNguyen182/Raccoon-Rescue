using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using R3;

namespace BubbleShooter.Scripts.Gameplay.GameTasks 
{
    public class GameTaskManager : IDisposable
    {
        private readonly InputProcessor _inputProcessor;
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;
        private readonly MatchBallHandler _matchBallHandler;
        private readonly BoosterHandleTask _boosterHandleTask;
        private readonly IDisposable _disposable;

        public GameTaskManager(GridCellManager gridCellManager, InputHandler inputHandler)
        {
            var d = Disposable.CreateBuilder();

            _gridCellManager = gridCellManager;
            _gridCellManager.AddTo(ref d);

            _inputProcessor = new(inputHandler, _gridCellManager);
            _inputProcessor.AddTo(ref d);

            
            _boosterHandleTask = new(_breakGridTask, _gridCellManager);
            _boosterHandleTask.AddTo(ref d);

            _matchBallHandler = new(_breakGridTask);
            _breakGridTask = new(_gridCellManager, _boosterHandleTask);

            _disposable = d.Build();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    } 
}
