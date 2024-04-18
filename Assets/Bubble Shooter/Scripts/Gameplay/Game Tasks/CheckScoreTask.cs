using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Gameplay.Models;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class CheckScoreTask : IDisposable
    {
        private readonly IDisposable _disposable;
        private readonly ISubscriber<AddScoreMessage> _addScoreSubscriber;

        private int _tierOneScore;
        private int _tierTwoScore;
        private int _tierThreeScore;
        private int _score = 0;

        public CheckScoreTask()
        {
            _score = 0;

            DisposableBagBuilder builder = DisposableBag.CreateBuilder();
            _addScoreSubscriber = GlobalMessagePipe.GetSubscriber<AddScoreMessage>();
            _addScoreSubscriber.Subscribe(message => AddScore(message.Score)).AddTo(builder);
            _disposable = builder.Build();
        }

        public void SetScores(LevelModel levelModel)
        {
            _tierOneScore = levelModel.TierOneScore;
            _tierTwoScore = levelModel.TierTwoScore;
            _tierThreeScore = levelModel.TierThreeScore;
        }

        private void AddScore(int score)
        {
            _score = _score + score;

            // Must check score in decending order to check star score
            if(_score >= _tierThreeScore)
            {
                // Mark a star 3 of score
            }

            else if(_score >= _tierTwoScore)
            {
                // Mark a star 2 of score
            }

            else if(_score >= _tierOneScore)
            {
                // Mark a star 1 of score
            }

            UpdateScore();
        }

        private void UpdateScore()
        {
            // To do: Update score UI here
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
