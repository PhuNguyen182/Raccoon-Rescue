using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.Common.Messages;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class CheckTargetTask : IDisposable
    {
        private readonly IDisposable _disposable;
        private readonly ISubscriber<CheckTargetMessage> _checkTargetSubscriber;

        private int _targetCount;

        public CheckTargetTask()
        {
            DisposableBagBuilder builder = DisposableBag.CreateBuilder();
            _checkTargetSubscriber = GlobalMessagePipe.GetSubscriber<CheckTargetMessage>();
            _checkTargetSubscriber.Subscribe(message => CheckTarget()).AddTo(builder);
            _disposable = builder.Build();
        }

        public void SetTargetCount(LevelModel levelModel)
        {
            _targetCount = levelModel.TargetCount;
        }

        private void CheckTarget()
        {
            _targetCount = _targetCount - 1;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
