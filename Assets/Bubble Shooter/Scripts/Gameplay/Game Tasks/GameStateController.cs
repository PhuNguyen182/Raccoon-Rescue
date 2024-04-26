using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.GameUI.Screens;
using BubbleShooter.Scripts.Gameplay.Miscs;
using Cysharp.Threading.Tasks;
using Stateless;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class GameStateController : IDisposable
    {
        private enum State
        {
            Ready,
            Playing,
            EndGame,
            Quit
        }

        private enum Trigger
        {
            Play,
            EndGame,
            BuyMove,
            Quit
        }

        private readonly EndGameTask _endGameTask;
        private readonly EndGameScreen _endGameScreen;
        private readonly CheckTargetTask _checkTargetTask;
        private readonly CheckScoreTask _checkScoreTask;
        private readonly GameDecorator _gameDecorator;

        private StateMachine<State, Trigger> _gameStateMachine;
        private StateMachine<State, Trigger>.TriggerWithParameters<bool> _endGameTrigger;

        public GameStateController(EndGameScreen endGameScreen, EndGameTask endGameTask
            , CheckTargetTask checkTargetTask, CheckScoreTask checkScoreTask, GameDecorator gameDecorator)
        {
            _endGameTask = endGameTask;
            _endGameScreen = endGameScreen;
            _checkTargetTask = checkTargetTask;
            _checkScoreTask = checkScoreTask;
            _gameDecorator = gameDecorator;

            _checkTargetTask.OnEndGame = EndGame;
            CreateGameStateMachine();
        }

        private void CreateGameStateMachine()
        {
            _gameStateMachine = new StateMachine<State, Trigger>(State.Ready);
            _endGameTrigger = _gameStateMachine.SetTriggerParameters<bool>(Trigger.EndGame);

            _gameStateMachine.Configure(State.Ready)
                             .OnActivate(StartGame)
                             .Permit(Trigger.Play, State.Playing);

            _gameStateMachine.Configure(State.Playing)
                             .OnEntryFrom(Trigger.Play, PlayGame)
                             .OnEntryFrom(Trigger.BuyMove, PlayGame)
                             .Permit(_endGameTrigger.Trigger, State.EndGame);

            _gameStateMachine.Configure(State.EndGame)
                             .OnEntryFrom(_endGameTrigger, isWin => OnEndGame(isWin).Forget())
                             .Permit(Trigger.BuyMove, State.Playing)
                             .Permit(Trigger.Quit, State.Quit);

            _gameStateMachine.Configure(State.Quit)
                             .OnEntry(OnQuitGame);

            _gameStateMachine.Activate();
        }

        private void StartGame()
        {
            if (_gameStateMachine.CanFire(Trigger.Play))
            {
                _gameStateMachine.Fire(Trigger.Play);
            }
        }

        private void PlayGame()
        {
            _gameDecorator.Character.ResetCryState();
            _gameDecorator.Character.Continue();
        }

        private void EndGame(bool isWin)
        {
            if (_gameStateMachine.CanFire(_endGameTrigger.Trigger))
            {
                _gameStateMachine.Fire(_endGameTrigger, isWin);
            }
        }

        private async UniTask OnEndGame(bool isWin)
        {
            if (isWin)
            {
                await _endGameTask.OnWinGame();
                _endGameScreen.SetGameResult(_checkScoreTask.Tier, _checkScoreTask.Score);
                _endGameScreen.ShowWinPanel();
            }

            else
            {
                _gameDecorator.Character.ResetPlayState();
                _gameDecorator.Character.Cry();

                await _endGameTask.OnLoseGame();
                bool canContinue = await _endGameScreen.ShowLosePanel();

                if (canContinue)
                {
                    // Add 5 move to continue play game
                    BuyMove();
                }

                else
                {
                    QuitGame();
                }
            }
        }

        private void BuyMove()
        {
            _checkTargetTask.AddMove(5);

            if (_gameStateMachine.CanFire(Trigger.BuyMove))
            {
                _gameStateMachine.Fire(Trigger.BuyMove);
            }
        }

        private void OnQuitGame()
        {
            Debug.Log("Quit game");
        }

        private void QuitGame()
        {
            if (_gameStateMachine.CanFire(Trigger.Quit))
            {
                _gameStateMachine.Fire(Trigger.Quit);
            }
        }

        public void Dispose()
        {
            _gameStateMachine.Deactivate();
        }
    }
}
