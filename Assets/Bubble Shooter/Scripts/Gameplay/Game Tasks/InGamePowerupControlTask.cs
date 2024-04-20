using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.GameUI.IngamePowerup;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Common.Constants;
using BubbleShooter.Scripts.Common.Enums;
using MessagePipe;
using BubbleShooter.Scripts.Gameplay.Models;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class InGamePowerupControlTask : IDisposable
    {
        private readonly BallShooter _ballShooter;
        private readonly IDisposable _messageDisposable;
        private readonly IngamePowerupPanel _ingamePowerupPanel;
        private readonly ISubscriber<PowerupMessage> _powerupSubscriber;

        private int _redBallCount;
        private int _greenBallCount;
        private int _yellowBallCount;
        private int _blueBallCount;

        public InGamePowerupControlTask(IngamePowerupPanel ingamePowerupPanel, BallShooter ballShooter)
        {
            _ingamePowerupPanel = ingamePowerupPanel;
            _redBallCount = _greenBallCount = _yellowBallCount = _blueBallCount = 0;
            _ballShooter = ballShooter;

            DisposableBagBuilder messageBuilder = DisposableBag.CreateBuilder();

            _powerupSubscriber = GlobalMessagePipe.GetSubscriber<PowerupMessage>();
            _powerupSubscriber.Subscribe(ProcessPowerup).AddTo(messageBuilder);


            _messageDisposable = messageBuilder.Build();
            CheckPowerup();
        }

        private void CheckPowerup()
        {
            CheckFireball();
            CheckLeafball();
            CheckSunball();
            CheckWaterball();
        }

        private void ProcessPowerup(PowerupMessage message)
        {
            switch (message.Command)
            {
                case ReactiveValueCommand.Changing:
                    PumbPowerup(message);
                    break;
                case ReactiveValueCommand.Remaining:
                    break;
                case ReactiveValueCommand.Reset:
                    FreePowerup(message);
                    break;
            }
        }

        private void PumbPowerup(PowerupMessage message)
        {
            switch (message.PowerupColor)
            {
                case EntityType.Red:
                    AddRedBall(message.Amount);
                    break;
                case EntityType.Yellow:
                    AddYellowBall(message.Amount);
                    break;
                case EntityType.Green:
                    AddGreenBall(message.Amount);
                    break;
                case EntityType.Blue:
                    AddBlueBall(message.Amount);
                    break;
            }
        }

        private void FreePowerup(PowerupMessage message)
        {
            switch (message.PowerupColor)
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

        private void AddRedBall(int amount)
        {
            _redBallCount = _redBallCount + amount;
            _redBallCount = Mathf.Clamp(_redBallCount, 0, BoosterConstants.FireballThreashold);
            CheckFireball();
        }

        private void AddYellowBall(int amount)
        {
            _yellowBallCount = _yellowBallCount + amount;
            _yellowBallCount = Mathf.Clamp(_yellowBallCount, 0, BoosterConstants.SunballThreashold);
            CheckSunball();
        }

        private void AddGreenBall(int amount)
        {
            _greenBallCount = _greenBallCount + amount;
            _greenBallCount = Mathf.Clamp(_greenBallCount, 0, BoosterConstants.LeafballThreashold);
            CheckLeafball();
        }

        private void AddBlueBall(int amount)
        {
            _blueBallCount = _blueBallCount + amount;
            _blueBallCount = Mathf.Clamp(_blueBallCount, 0, BoosterConstants.WaterballThreashold);
            CheckWaterball();
        }

        private void FreeRedBall()
        {
            _redBallCount = 0;
            CheckFireball();

            // If free ball, use this power up
            _ballShooter.SetColorModel(new BallShootModel
            {
                BallCount = 1,
                BallColor = EntityType.FireBall,
                IsPowerup = true
            }, true);
        }

        private void FreeYellowBall()
        {
            _yellowBallCount = 0;
            CheckSunball();

            // If free ball, use this power up
            _ballShooter.SetColorModel(new BallShootModel
            {
                BallCount = 3,
                BallColor = EntityType.SunBall,
                IsPowerup = true
            }, true);
        }

        private void FreeGreenBall()
        {
            _greenBallCount = 0;
            CheckLeafball();

            // If free ball, use this power up
            _ballShooter.SetColorModel(new BallShootModel
            {
                BallCount = 1,
                BallColor = EntityType.LeafBall,
                IsPowerup = true
            }, true);
        }

        private void FreeBlueBall()
        {
            _blueBallCount = 0;
            CheckWaterball();

            // If free ball, use this power up
            _ballShooter.SetColorModel(new BallShootModel
            {
                BallCount = 1,
                BallColor = EntityType.WaterBall,
                IsPowerup = true
            }, true);
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
            _messageDisposable.Dispose();
        }
    }
}
