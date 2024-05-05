using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.GameUI.IngameBooster;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks
{
    public class ColorfullBoosterTask
    {
        private readonly BallShooter _ballShooter;
        private readonly BoosterPanel _boosterPanel;
        private readonly InputProcessor _inputProcessor;

        private bool _canExecute;

        public ColorfullBoosterTask(BoosterPanel boosterPanel, BallShooter ballShooter, InputProcessor inputProcessor)
        {
            _canExecute = true;
            _boosterPanel = boosterPanel;
            _ballShooter = ballShooter;
            _inputProcessor = inputProcessor;
        }

        public async UniTask Execute()
        {
            if (!_canExecute)
                return;

            _canExecute = false;
            _inputProcessor.IsActive = false;
            _ballShooter.SetColorModel(new BallShootModel(), false);
            await _boosterPanel.SpawnColorful(_ballShooter.ShotPoint.position);

            _ballShooter.SetColorModel(new BallShootModel
            {
                BallCount = 1,
                BallColor = EntityType.ColorfulBall,
                IsPowerup = true
            }, true);

            _inputProcessor.IsActive = true;
            _canExecute = true;
        }
    }
}
