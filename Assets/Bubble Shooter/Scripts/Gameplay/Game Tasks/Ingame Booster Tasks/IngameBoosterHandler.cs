using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.GameUI.IngameBooster;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Gameplay.Miscs;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks
{
    public class IngameBoosterHandler : IDisposable
    {
        private readonly ColorfullBoosterTask _colorfullBoosterTask;
        private readonly AimBoosterTask _aimBoosterTask;
        private readonly ChangeBallTask _changeBallTask;

        public IngameBoosterHandler(BoosterPanel boosterPanel, BallProvider ballProvider, BallShooter ballShooter, InputProcessor inputProcessor)
        {
            _aimBoosterTask = new(ballShooter);
            _colorfullBoosterTask = new(boosterPanel, ballShooter, inputProcessor);
            _changeBallTask = new(boosterPanel, ballProvider, ballShooter, inputProcessor);
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

        public void Dispose()
        {
            
        }
    }
}
