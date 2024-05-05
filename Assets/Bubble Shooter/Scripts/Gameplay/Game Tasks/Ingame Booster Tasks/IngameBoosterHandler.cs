using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.GameUI.IngameBooster;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.Common.Messages;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks
{
    public class IngameBoosterHandler : IDisposable
    {
        private readonly ISubscriber<IngameBoosterMessage> _boosterSubscriber;
        private readonly ColorfullBoosterTask _colorfullBoosterTask;
        private readonly AimBoosterTask _aimBoosterTask;
        private readonly ChangeBallTask _changeBallTask;
        private readonly BoosterPanel _boosterPanel;

        private IDisposable _disposable;

        public IngameBoosterHandler(BoosterPanel boosterPanel, BallProvider ballProvider, BallShooter ballShooter, InputProcessor inputProcessor)
        {
            _aimBoosterTask = new(ballShooter);
            _colorfullBoosterTask = new(boosterPanel, ballShooter, inputProcessor);
            _changeBallTask = new(boosterPanel, ballProvider, ballShooter, inputProcessor);

            DisposableBagBuilder builder = DisposableBag.CreateBuilder();
            _boosterSubscriber = GlobalMessagePipe.GetSubscriber<IngameBoosterMessage>();
            _boosterSubscriber.Subscribe(message => ExecuteBooster(message.BoosterType).Forget()).AddTo(builder);
            _disposable = builder.Build();
        }

        public async UniTask ExecuteBooster(IngameBoosterType booster)
        {
            switch (booster)
            {
                case IngameBoosterType.Colorful:
                    await _colorfullBoosterTask.Execute();
                    break;
                case IngameBoosterType.Aim:
                    _aimBoosterTask.Execute();
                    break;
                case IngameBoosterType.ChangeBall:
                    await _changeBallTask.Execute();
                    break;
            }
        }

        private void UpdateBooster()
        {
            _boosterPanel.Colorful.SetBoosterCount(GameData.Instance.InGameBoosterData.ColorfulCount);
            _boosterPanel.Aiming.SetBoosterCount(GameData.Instance.InGameBoosterData.TargetAimCount);
            _boosterPanel.ChangeBall.SetBoosterCount(GameData.Instance.InGameBoosterData.RandomBallCount);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
