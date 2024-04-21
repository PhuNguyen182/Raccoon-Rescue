using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.GameUI.Screens;
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

        private readonly EndGameScreen _endGameScreen;
        private readonly CheckTargetTask _checkTargetTask;

        private StateMachine<State, Trigger> _gameStateMachine;
        private StateMachine<State, Trigger>.TriggerWithParameters<bool> _endGameTrigger;

        public GameStateController(EndGameScreen endGameScreen, CheckTargetTask checkTargetTask)
        {
            CreateGameStateMachine();

            _endGameScreen = endGameScreen;
            _checkTargetTask = checkTargetTask;

            _checkTargetTask.OnEndGame = EndGame;
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
                _endGameScreen.ShowWinPanel();
            }

            else
            {
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
