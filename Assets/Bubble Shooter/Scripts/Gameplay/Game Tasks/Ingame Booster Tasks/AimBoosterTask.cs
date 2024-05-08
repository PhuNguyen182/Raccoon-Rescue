using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks
{
    public class AimBoosterTask
    {
        private readonly BallShooter _ballShooter;

        public AimBoosterTask(BallShooter ballShooter)
        {
            _ballShooter = ballShooter;
        }

        public void Execute()
        {
            _ballShooter.SetPremierState(true);
        }
    }
}
