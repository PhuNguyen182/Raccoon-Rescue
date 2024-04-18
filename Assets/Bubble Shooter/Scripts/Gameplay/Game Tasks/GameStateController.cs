using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stateless;
using BubbleShooter.Scripts.GameUI.Screens;

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
                             .OnEntryFrom(Trigger.BuyMove, ContinuePlay)
                             .Permit(_endGameTrigger.Trigger, State.EndGame);

            _gameStateMachine.Configure(State.EndGame)
                             .OnEntryFrom(_endGameTrigger, value => OnEndGame(value))
                             .Permit(Trigger.BuyMove, State.Playing)
                             .Permit(Trigger.Quit, State.Quit);

            _gameStateMachine.Configure(State.Quit)
                             .OnEntry(QuitGame);

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

        private void OnEndGame(bool isWin)
        {
            if (isWin)
            {
                // To do: Execute win game logic here
            }

            else
            {
                // To do: Execute lose game logic here
            }
        }

        private void ContinuePlay()
        {

        }

        private void QuitGame()
        {

        }

        public void Dispose()
        {
            _gameStateMachine.Deactivate();
        }
    }
}
