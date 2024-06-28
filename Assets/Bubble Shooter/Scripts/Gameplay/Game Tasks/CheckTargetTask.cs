using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.GameUI.Screens;
using BubbleShooter.Scripts.Common.PlayDatas;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class CheckTargetTask : IDisposable
    {
        private readonly InGamePanel _inGamePanel;
        private readonly IDisposable _disposable;
        private readonly ISubscriber<AddTargetMessage> _addTargetSubscriber;
        private readonly ISubscriber<MoveToTargetMessage> _moveTargetSubscriber;
        private readonly ISubscriber<DecreaseMoveMessage> _decreaseMoveSubscriber;

        private bool _isTargetAdded;
        private bool _isOutOfTarget;

        private int _moveCount;
        private int _targetCount;
        private int _maxTarget;
        private int _targetCountTemp;

        public Action<bool> OnEndGame;
        public int MoveCount => _moveCount;
        public int TargetCount => _maxTarget;
        public bool IsTargetAdded => _isTargetAdded;
        public bool IsOutOfTarget => _isOutOfTarget;

        public CheckTargetTask(InGamePanel inGamePanel)
        {
            _inGamePanel = inGamePanel;
            DisposableBagBuilder builder = DisposableBag.CreateBuilder();

            _moveTargetSubscriber = GlobalMessagePipe.GetSubscriber<MoveToTargetMessage>();
            _addTargetSubscriber = GlobalMessagePipe.GetSubscriber<AddTargetMessage>();
            _decreaseMoveSubscriber = GlobalMessagePipe.GetSubscriber<DecreaseMoveMessage>();

            _moveTargetSubscriber.Subscribe(SetTargetInfo).AddTo(builder);
            _addTargetSubscriber.Subscribe(message => AddTarget(message)).AddTo(builder);
            _decreaseMoveSubscriber.Subscribe(DecreaseMove).AddTo(builder);

            _disposable = builder.Build();
        }

        public void SetTargetCount(LevelModel levelModel)
        {
            _targetCount = 0;
            _targetCountTemp = 0;
            _isTargetAdded = false;
            _isOutOfTarget = false;
            _isOutOfTarget = false;

            _moveCount = levelModel.MoveCount;
            _maxTarget = levelModel.TargetCount;

            UpdateMove();
            UpdateTarget();
        }

        public void AddMove(int move)
        {
            _moveCount = _moveCount + move;

            UpdateMove();
            CheckTarget();
        }

        private void SetTargetInfo(MoveToTargetMessage message)
        {
            _isTargetAdded = true;
            Vector3 position = _inGamePanel.TargetPoint.position;
            position.z = 0;

            message.Source.TrySetResult(new MoveTargetData
            {
                Destination = position
            });
        }

        private void UpdateMove()
        {
            _inGamePanel.SetMoveCount(_moveCount);
        }

        private void UpdateTarget()
        {
            _inGamePanel.UpdateTarget(_targetCount, _maxTarget);
        }

        private void DecreaseMove(DecreaseMoveMessage message)
        {
            if (message.CanDecreaseMove)
            {
                _moveCount = _moveCount - 1;
                UpdateMove();
            }
        }

        private void AddTarget(AddTargetMessage message)
        {
            if (!message.IsImmediately)
            {
                _targetCount = _targetCount + 1;
                _inGamePanel.TargetHolder.PlayTargetAnimation();
                _isTargetAdded = false;

                UpdateTarget();
                CheckTarget();
            }

            else
            {
                _targetCountTemp += 1;

                if (_targetCountTemp >= _maxTarget)
                    _isOutOfTarget = true;
            }
        }

        public void CheckTarget()
        {
            if (_moveCount == 0)
            {
                bool allTargetCollected = _targetCount >= _maxTarget;
                OnEndGame?.Invoke(allTargetCollected);
            }

            else
            {
                if (_targetCount >= _maxTarget)
                {
                    OnEndGame?.Invoke(true);
                }
            }
        }

        public void Dispose()
        {
            OnEndGame = null;
            _disposable.Dispose();
        }
    }
}
