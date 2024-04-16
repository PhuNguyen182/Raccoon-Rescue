using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.GameUI.IngamePowerup;
using BubbleShooter.Scripts.Common.Constants;
using BubbleShooter.Scripts.Common.Enums;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class InGamePowerupControlTask : IDisposable
    {
        private readonly IngamePowerupPanel _ingamePowerupPanel;
        private readonly IDisposable _disposable;
        private readonly ISubscriber<PowerupMessage> _powerupSubscriber;

        private int _redBallCount;
        private int _greenBallCount;
        private int _yellowBallCount;
        private int _blueBallCount;

        public InGamePowerupControlTask(IngamePowerupPanel ingamePowerupPanel)
        {
            _ingamePowerupPanel = ingamePowerupPanel;
            _redBallCount = _greenBallCount = _yellowBallCount = _blueBallCount = 0;

            DisposableBagBuilder builder = DisposableBag.CreateBuilder();
            _powerupSubscriber = GlobalMessagePipe.GetSubscriber<PowerupMessage>();
            _powerupSubscriber.Subscribe(ProcessPowerup).AddTo(builder);
            _disposable = builder.Build();

            CheckPowerup();
        }

        private void CheckPowerup()
        {
            CheckFireball();
            CheckSunball();
            CheckLeafball();
            CheckWaterball();
        }

        private void ProcessPowerup(PowerupMessage message)
        {
            if (message.IsAdd)
                PumbPowerup(message.PowerupColor);
            else
                FreePowerup(message.PowerupColor);
        }

        private void PumbPowerup(EntityType powerupColor)
        {
            switch (powerupColor)
            {
                case EntityType.Red:
                    AddRedBall();
                    break;
                case EntityType.Yellow:
                    AddYellowBall();
                    break;
                case EntityType.Green:
                    AddGreenBall();
                    break;
                case EntityType.Blue:
                    AddBlueBall();
                    break;
            }
        }

        private void FreePowerup(EntityType powerupColor)
        {
            switch (powerupColor)
            {
                case EntityType.Red:
                    FreeRedBall();
                    break;
                case EntityType.Yellow:
                    FreeYellowBall();
                    break;
                case EntityType.Green:
                    FreeGreenBall();
                    break;
                case EntityType.Blue:
                    FreeBlueBall();
                    break;
            }
        }

        private void AddRedBall()
        {
            _redBallCount = _redBallCount + 1;
            
            if(_redBallCount >= BoosterConstants.FireballThreashold)
            {
                _redBallCount = BoosterConstants.FireballThreashold;
            }

            CheckFireball();
        }

        private void AddYellowBall()
        {
            _yellowBallCount = _yellowBallCount + 1;

            if(_yellowBallCount >= BoosterConstants.SunballThreashold)
            {
                _yellowBallCount = BoosterConstants.SunballThreashold;
            }

            CheckSunball();
        }

        private void AddGreenBall()
        {
            _greenBallCount = _greenBallCount + 1;

            if (_greenBallCount >= BoosterConstants.LeafballThreashold)
            {
                _greenBallCount = BoosterConstants.LeafballThreashold;
            }

            CheckLeafball();
        }

        private void AddBlueBall()
        {
            _blueBallCount = _blueBallCount + 1;

            if (_blueBallCount >= BoosterConstants.WaterballThreashold)
            {
                _blueBallCount = BoosterConstants.WaterballThreashold;
            }

            CheckWaterball();
        }

        private void FreeRedBall()
        {
            _redBallCount = 0;
            CheckFireball();
        }

        private void FreeYellowBall()
        {
            _yellowBallCount = 0;
            CheckSunball();
        }

        private void FreeGreenBall()
        {
            _greenBallCount = 0;
            CheckLeafball();
        }

        private void FreeBlueBall()
        {
            _blueBallCount = 0;
            CheckWaterball();
        }

        private void CheckFireball()
        {
            _ingamePowerupPanel.ControlPowerupButtons((float)_redBallCount / BoosterConstants.FireballThreashold, EntityType.FireBall);
        }

        private void CheckSunball()
        {
            _ingamePowerupPanel.ControlPowerupButtons((float)_yellowBallCount / BoosterConstants.SunballThreashold, EntityType.SunBall);
        }

        private void CheckLeafball()
        {
            _ingamePowerupPanel.ControlPowerupButtons((float)_greenBallCount / BoosterConstants.LeafballThreashold, EntityType.LeafBall);
        }

        private void CheckWaterball()
        {
            _ingamePowerupPanel.ControlPowerupButtons((float)_blueBallCount / BoosterConstants.WaterballThreashold, EntityType.WaterBall);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
