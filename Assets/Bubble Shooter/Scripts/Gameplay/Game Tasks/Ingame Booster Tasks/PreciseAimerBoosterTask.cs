using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks
{
    public class PreciseAimerBoosterTask
    {
        private readonly BallShooter _ballShooter;

        public PreciseAimerBoosterTask(BallShooter ballShooter)
        {
            _ballShooter = ballShooter;
        }

        public void Execute()
        {
            _ballShooter.SetPremierState(true);
        }
    }
}
