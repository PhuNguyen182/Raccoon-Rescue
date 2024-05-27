using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.GameUI.Screens;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.Common.Configs;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using Scripts.SceneUtils;
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
        private readonly MainScreenManager _mainScreen;
        private readonly EndGameScreen _endGameScreen;
        private readonly CheckTargetTask _checkTargetTask;
        private readonly CheckScoreTask _checkScoreTask;
        private readonly GameDecorator _gameDecorator;
        private readonly InputProcessor _inputProcessor;

        private StateMachine<State, Trigger> _gameStateMachine;
        private StateMachine<State, Trigger>.TriggerWithParameters<bool> _endGameTrigger;

        public bool IsEndGame { get; private set; }

        public GameStateController(EndGameTask endGameTask, MainScreenManager mainScreen, CheckTargetTask checkTargetTask
            , CheckScoreTask checkScoreTask, GameDecorator gameDecorator, InputProcessor inputProcessor)
        {
            _mainScreen = mainScreen;
            _endGameTask = endGameTask;
            _endGameScreen = mainScreen.EndGameScreen;
            _checkTargetTask = checkTargetTask;
            _checkScoreTask = checkScoreTask;
            _gameDecorator = gameDecorator;
            _inputProcessor = inputProcessor;

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
                             .OnEntryFrom(Trigger.BuyMove, Continue)
                             .Permit(_endGameTrigger.Trigger, State.EndGame);

            _gameStateMachine.Configure(State.EndGame)
                             .OnEntryFrom(_endGameTrigger, isWin => OnEndGame(isWin).Forget())
                             .Permit(Trigger.BuyMove, State.Playing)
                             .Permit(Trigger.Quit, State.Quit);

            _gameStateMachine.Configure(State.Quit)
                             .OnEntry(() => OnQuitGame().Forget());

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
            IsEndGame = false;
            _endGameTask.ResetBallColor();
            _gameDecorator.Character.ResetCryState();
            _gameDecorator.Character.Continue();
        }

        private void Continue()
        {
            IsEndGame = false;
            _endGameTask.ResetBallColor();
            _gameDecorator.Character.ResetCryState();
            _gameDecorator.Character.Continue();
            _endGameTask.ContinueSpawnBall();
            _mainScreen.ShowMainPanel(true);
            _inputProcessor.IsActive = true;
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
            IsEndGame = true;
            _inputProcessor.IsActive = false;
            _mainScreen.ShowMainPanel(false);

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

        private async UniTask OnQuitGame()
        {
            if (!PlayConfig.Current.IsTest)
            {
                TransitionConfig.Current = new TransitionConfig
                {
                    SceneName = SceneName.Mainhome
                };

                BackHomeConfig.Current = new BackHomeConfig
                {
                    IsWin = false,
                    Level = PlayConfig.Current.Level,
                    Star = _checkScoreTask.Tier
                };

                PlayConfig.Current = null;
                await SceneLoader.LoadScene(SceneConstants.Transition, LoadSceneMode.Single);
            }

            QuitMessage();
        }

        private void QuitGame()
        {
            if (_gameStateMachine.CanFire(Trigger.Quit))
            {
                _gameStateMachine.Fire(Trigger.Quit);
            }
        }

        private void QuitMessage()
        {
#if UNITY_EDITOR
            Debug.Log("On Back To Main Home");
#endif
        }

        public void Dispose()
        {
            _gameStateMachine.Deactivate();
        }
    }
}
