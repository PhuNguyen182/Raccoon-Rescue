using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameManagers;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class StartingGameTask : IDisposable
    {
        private readonly InputProcessor _inputProcessor;
        private readonly MoveGameViewTask _moveGameViewTask;
        private readonly GameplayTutorialManager _gameplayTutorial;

        private CancellationToken _token;
        private CancellationTokenSource _cts;

        public StartingGameTask(InputProcessor inputProcessor, MoveGameViewTask moveGameViewTask, GameplayTutorialManager tutorialManager)
        {
            _inputProcessor = inputProcessor;
            _moveGameViewTask = moveGameViewTask;
            _gameplayTutorial = tutorialManager;

            _cts = new();
            _token = _cts.Token;
        }

        public async UniTask OnStartGame(int level)
        {
            _inputProcessor.IsActive = false;
            _moveGameViewTask.CalculateFirstItemHeight();
            var tutorial = _gameplayTutorial.GetTutorial(level);
            
            if (tutorial != null)
                SimplePool.PoolPreLoad(tutorial.gameObject, 1);

            await _moveGameViewTask.MoveViewOnStart();
            
            if (tutorial != null)
                SimplePool.Spawn(tutorial);

            _inputProcessor.IsActive = true;
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}