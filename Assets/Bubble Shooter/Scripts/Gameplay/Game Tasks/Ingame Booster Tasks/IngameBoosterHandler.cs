using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks
{
    public class IngameBoosterHandler : IDisposable
    {
        private readonly ColorfullBoosterTask _colorfullBoosterTask;
        private readonly AimBoosterTask _aimBoosterTask;
        private readonly ChangeBallTask _changeBallTask;

        public IngameBoosterHandler(BallShooter ballShooter)
        {
            _colorfullBoosterTask = new(ballShooter);
            _aimBoosterTask = new(ballShooter);
            _changeBallTask = new(ballShooter);
        }

        public void ExecuteBooster(IngameBoosterType booster)
        {
            switch (booster)
            {
                case IngameBoosterType.Colorful:
                    _colorfullBoosterTask.Execute();
                    break;
                case IngameBoosterType.Aim:
                    _aimBoosterTask.Execute();
                    break;
                case IngameBoosterType.ChangeBall:
                    _changeBallTask.Execute();
                    break;
            }
        }

        public void Dispose()
        {
            
        }
    }
}
