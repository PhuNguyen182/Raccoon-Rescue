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

        private int _moveCount;
        private int _targetCount;
        private int _maxTarget;

        public Action<bool> OnEndGame;

        public CheckTargetTask(InGamePanel inGamePanel)
        {
            _inGamePanel = inGamePanel;
            DisposableBagBuilder builder = DisposableBag.CreateBuilder();

            _moveTargetSubscriber = GlobalMessagePipe.GetSubscriber<MoveToTargetMessage>();
            _addTargetSubscriber = GlobalMessagePipe.GetSubscriber<AddTargetMessage>();
            _decreaseMoveSubscriber = GlobalMessagePipe.GetSubscriber<DecreaseMoveMessage>();

            _moveTargetSubscriber.Subscribe(SetTargetInfo).AddTo(builder);
            _addTargetSubscriber.Subscribe(message => AddTarget()).AddTo(builder);
            _decreaseMoveSubscriber.Subscribe(message => DecreaseMove()).AddTo(builder);

            _disposable = builder.Build();
        }

        public void SetTargetCount(LevelModel levelModel)
        {
            _moveCount = levelModel.MoveCount;
            _maxTarget = levelModel.TargetCount;
            _targetCount = 0;
            
            UpdateMove();
            UpdateTarget();
        }

        public void AddMove(int move)
        {
            _moveCount = _moveCount + move;
            CheckTarget();
        }

        private void SetTargetInfo(MoveToTargetMessage message)
        {
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

        private void DecreaseMove()
        {
            _moveCount = _moveCount - 1;
            UpdateMove();
        }

        private void AddTarget()
        {
            _targetCount = _targetCount + 1;
            _inGamePanel.TargetHolder.PlayTargetAnimation();

            UpdateTarget();
            CheckTarget();
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
