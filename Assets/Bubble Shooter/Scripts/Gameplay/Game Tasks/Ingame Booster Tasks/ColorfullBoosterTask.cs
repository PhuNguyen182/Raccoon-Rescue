using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks
{
    public class ColorfullBoosterTask
    {
        private readonly BallShooter _ballShooter;

        public ColorfullBoosterTask(BallShooter ballShooter)
        {
            _ballShooter = ballShooter;
        }

        public void Execute()
        {
            _ballShooter.SetColorModel(new BallShootModel
            {
                BallCount = 1,
                BallColor = EntityType.ColorfulBall,
                IsPowerup = true
            }, true);
        }
    }
}
