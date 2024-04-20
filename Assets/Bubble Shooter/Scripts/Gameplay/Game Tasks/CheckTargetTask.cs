using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.GameUI.Screens;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class CheckTargetTask : IDisposable
    {
        private readonly IDisposable _disposable;
        private readonly InGamePanel _inGamePanel;
        private readonly ISubscriber<CheckTargetMessage> _checkTargetSubscriber;
        private readonly ISubscriber<DecreaseMoveMessage> _decreaseMoveSubscriber;

        private int _moveCount;
        private int _targetCount;
        private int _maxTarget;

        public Action<bool> OnEndGame;

        public CheckTargetTask(InGamePanel inGamePanel)
        {
            _inGamePanel = inGamePanel;
            DisposableBagBuilder builder = DisposableBag.CreateBuilder();

            _checkTargetSubscriber = GlobalMessagePipe.GetSubscriber<CheckTargetMessage>();
            _decreaseMoveSubscriber = GlobalMessagePipe.GetSubscriber<DecreaseMoveMessage>();

            _checkTargetSubscriber.Subscribe(message => DecreaseTarget()).AddTo(builder);
            _decreaseMoveSubscriber.Subscribe(message => DecreaseMove()).AddTo(builder);

            _disposable = builder.Build();
        }

        public void SetTargetCount(LevelModel levelModel)
        {
            _moveCount = levelModel.MoveCount;
            _maxTarget = levelModel.TargetCount;
            _targetCount = 0;
            CheckTarget();
        }

        public void AddMove(int move)
        {
            _moveCount = _moveCount + move;
            CheckTarget();
        }

        private void UpdateTarget()
        {
            _inGamePanel.SetMoveCount(_moveCount);
            _inGamePanel.UpdateTarget(_targetCount, _maxTarget);
        }

        private void DecreaseMove()
        {
            _moveCount = _moveCount - 1;
            CheckTarget();
        }

        private void DecreaseTarget()
        {
            _targetCount = _targetCount + 1;
            CheckTarget();
        }

        private void CheckTarget()
        {
            if (_moveCount <= 0)
            {
                if (_targetCount < _maxTarget)
                    OnEndGame?.Invoke(false);
            }

            else if(_moveCount >= 0)
            {
                if (_targetCount >= _maxTarget)
                {
                    OnEndGame?.Invoke(true);
                }
            }

            UpdateTarget();
        }

        public void Dispose()
        {
            OnEndGame = null;
            _disposable.Dispose();
        }
    }
}
