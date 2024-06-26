using R3;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using BubbleShooter.Scripts.GameUI.IngameBooster;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.GameUI.Boxes;
using BubbleShooter.Scripts.Common.Configs;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks
{
    public class IngameBoosterHandler : IDisposable
    {
        private readonly InputProcessor _inputProcessor;
        private readonly PreciseAimerBoosterTask _aimBoosterTask;
        private readonly ColorfullBoosterTask _colorfullBoosterTask;
        private readonly ChangeBallTask _changeBallTask;
        private readonly IngameBoosterPanel _boosterPanel;
        private readonly ISubscriber<AddIngameBoosterMessage> _boosterSubscriber;

        private bool _hasUsedBooster;
        private Dictionary<IngameBoosterType, ReactiveProperty<int>> _boosters;
        private IDisposable _messageDisposable;
        private IDisposable _boosterDisposable;

        private const string AimBoosterBoxPath = "Gameplay/Boxes/Ingame Boosters/Aim Booster Popup";
        private const string ColorfulBoxPath = "Gameplay/Boxes/Ingame Boosters/Colorful Booster Popup";
        private const string ExtraBallBoxPath = "Gameplay/Boxes/Ingame Boosters/Change Ball Booster Popup";

        public IngameBoosterHandler(IngameBoosterPanel boosterPanel, BallProvider ballProvider, BallShooter ballShooter, InputProcessor inputProcessor)
        {
            _aimBoosterTask = new(ballShooter);
            _colorfullBoosterTask = new(boosterPanel, ballShooter, inputProcessor);
            _changeBallTask = new(boosterPanel, ballProvider, ballShooter, inputProcessor);
            _inputProcessor = inputProcessor;
            _boosterPanel = boosterPanel;

            DisposableBagBuilder builder = MessagePipe.DisposableBag.CreateBuilder();
            _boosterSubscriber = GlobalMessagePipe.GetSubscriber<AddIngameBoosterMessage>();
            _boosterSubscriber.Subscribe(AddBooster).AddTo(builder);
            _messageDisposable = builder.Build();
        }

        public void InitBooster(List<IngameBoosterModel> boosterModels)
        {
            boosterModels ??= new();

            if (!boosterModels.Exists(x => x.BoosterType == IngameBoosterType.Colorful))
                boosterModels.Add(new IngameBoosterModel() { BoosterType = IngameBoosterType.Colorful });

            if (!boosterModels.Exists(x => x.BoosterType == IngameBoosterType.PreciseAimer))
                boosterModels.Add(new IngameBoosterModel() { BoosterType = IngameBoosterType.PreciseAimer });
            
            if (!boosterModels.Exists(x => x.BoosterType == IngameBoosterType.ChangeBall))
                boosterModels.Add(new IngameBoosterModel() { BoosterType = IngameBoosterType.ChangeBall });

            using (var disposableList = ListPool<IDisposable>.Get(out var boosterDisposables))
            {
                _boosters = boosterModels.ToDictionary(booster => booster.BoosterType, booster =>
                {
                    ReactiveProperty<int> boosterAmount = new(0);
                    BoosterButton boosterButton = _boosterPanel.GetButtonByBooster(booster.BoosterType);

                    boosterAmount.Subscribe(value => boosterButton.SetBoosterCount(value));
                    IDisposable d1 = boosterButton.OnClickObserver.Where(value => (boosterAmount.Value > 0 || value.IsFree) && !value.IsActive && _inputProcessor.IsActive)
                                                  .Subscribe(value =>
                                                  {
                                                      ExecuteBoosterAsync(booster.BoosterType).Forget();
                                                      boosterButton.ShowInvincible().Forget();
                                                  });

                    IDisposable d2 = boosterButton.OnClickObserver.Where(value => boosterAmount.Value <= 0 && !value.IsFree && !value.IsActive && _inputProcessor.IsActive)
                                                  .Subscribe(value => ShowBuyBooster(booster.BoosterType));

                    boosterDisposables.Add(d1);
                    boosterDisposables.Add(d2);
                    boosterAmount.Value = booster.Amount;
                    return boosterAmount;
                });

                _boosterDisposable = Disposable.Combine(boosterDisposables.ToArray());
            }

            PreloadBoosterPopups();
        }

        private void PreloadBoosterPopups()
        {
            IngameBoosterPopup.Preload(ColorfulBoxPath);
            IngameBoosterPopup.Preload(AimBoosterBoxPath);
            IngameBoosterPopup.Preload(ExtraBallBoxPath);
        }

        private void AddBooster(AddIngameBoosterMessage message)
        {
            _boosters[message.BoosterType].Value += message.Amount; 
        }

        private async UniTask ExecuteBoosterAsync(IngameBoosterType booster)
        {
            if (_hasUsedBooster)
                return;

            switch (booster)
            {
                case IngameBoosterType.Colorful:
                    await _colorfullBoosterTask.Execute();
                    break;
                case IngameBoosterType.PreciseAimer:
                    _aimBoosterTask.Execute();
                    break;
                case IngameBoosterType.ChangeBall:
                    await _changeBallTask.Execute();
                    break;
            }

            _hasUsedBooster = true;
            BoosterButton button = _boosterPanel.GetButtonByBooster(booster);
            button.SetBoosterActive(true);
            _boosters[booster].Value--;
            
            if(!PlayConfig.Current.IsTest)
                GameData.Instance.UseBooster(booster, 1);
        }

        public void AfterUseBooster()
        {
            if (!_hasUsedBooster)
                return;

            for (int i = 0; i < _boosterPanel.Boosters.Count; i++)
            {
                _boosterPanel.Boosters[i].SetBoosterActive(false);
                _boosterPanel.Boosters[i].SetFreeState(false);
            }

            _hasUsedBooster = false;
        }

        private void ShowBuyBooster(IngameBoosterType boosterType)
        {
            switch (boosterType)
            {
                case IngameBoosterType.Colorful:
                    IngameBoosterPopup.Create(ColorfulBoxPath);
                    break;
                case IngameBoosterType.PreciseAimer:
                    IngameBoosterPopup.Create(AimBoosterBoxPath);
                    break;
                case IngameBoosterType.ChangeBall:
                    IngameBoosterPopup.Create(ExtraBallBoxPath);
                    break;
            }
        }

        public void Dispose()
        {
            _messageDisposable?.Dispose();
            _boosterDisposable?.Dispose();
        }
    }
}
