using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.GameUI.Screens;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class CheckScoreTask : IDisposable
    {
        private readonly IDisposable _disposable;
        private readonly InGamePanel _inGamePanel;
        private readonly ISubscriber<AddScoreMessage> _addScoreSubscriber;

        private int _tier = 0;
        private int _tier1Score = 0;
        private int _tier2Score = 0;
        private int _tier3Score = 0;
        private int _maxScore = 0;
        private int _score = 0;

        public CheckScoreTask(InGamePanel inGamePanel)
        {
            _score = 0;
            _inGamePanel = inGamePanel;

            DisposableBagBuilder builder = DisposableBag.CreateBuilder();
            _addScoreSubscriber = GlobalMessagePipe.GetSubscriber<AddScoreMessage>();
            _addScoreSubscriber.Subscribe(message => AddScore(message.Score)).AddTo(builder);
            _disposable = builder.Build();
        }

        public void SetScores(LevelModel levelModel)
        {
            _tier1Score = levelModel.TierOneScore;
            _tier2Score = levelModel.TierTwoScore;
            _tier3Score = levelModel.TierThreeScore;
            _maxScore = levelModel.MaxScore;

            _inGamePanel.SetScore(_score, _maxScore);
            _inGamePanel.SetScoreStreak(_tier1Score, _tier2Score, _tier3Score);
        }

        private void AddScore(int score)
        {
            _score = _score + score;

            if(_score >= _tier3Score) _tier = 3;
            else if(_score >= _tier2Score) _tier = 2;
            else if(_score >= _tier1Score) _tier = 1;
            else _tier = 0;

            UpdateScore();
        }

        private void UpdateScore()
        {
            _inGamePanel.SetScore(_score, _maxScore);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
