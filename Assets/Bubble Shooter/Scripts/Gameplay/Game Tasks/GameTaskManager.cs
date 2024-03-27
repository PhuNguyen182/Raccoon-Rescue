using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks;
using R3;

namespace BubbleShooter.Scripts.Gameplay.GameTasks 
{
    public class GameTaskManager : IDisposable
    {
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;
        private readonly MatchBallHandler _matchBallHandler;
        private readonly BoosterHandleTask _boosterHandleTask;
        private readonly IDisposable _disposable;

        public GameTaskManager(GridCellManager gridCellManager)
        {
            var d = Disposable.CreateBuilder();

            _gridCellManager = gridCellManager;
            _gridCellManager.AddTo(ref d);

            _breakGridTask = new(_gridCellManager);
            _matchBallHandler = new(_breakGridTask);
            
            _boosterHandleTask = new(_breakGridTask, _gridCellManager);
            _boosterHandleTask.AddTo(ref d);

            _disposable = d.Build();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    } 
}
