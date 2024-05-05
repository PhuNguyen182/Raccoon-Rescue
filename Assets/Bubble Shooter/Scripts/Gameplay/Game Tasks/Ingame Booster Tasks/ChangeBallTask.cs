using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.GameUI.IngameBooster;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.Gameplay.Models;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks
{
    public class ChangeBallTask
    {
        private readonly BoosterPanel _boosterPanel;
        private readonly BallProvider _ballProvider;
        private readonly BallShooter _ballShooter;
        private readonly InputProcessor _inputProcessor;

        private bool _canExecute;

        public ChangeBallTask(BoosterPanel boosterPanel, BallProvider ballProvider, BallShooter ballShooter, InputProcessor inputProcessor)
        {
            _canExecute = true;
            _boosterPanel = boosterPanel;
            _ballProvider = ballProvider;
            _ballShooter = ballShooter;
            _inputProcessor = inputProcessor;
        }

        public async UniTask Execute()
        {
            if (!_canExecute)
                return;

            _canExecute = false;
            _inputProcessor.IsActive = false;
            BallShootModel ballModel = _ballProvider.GetRandomHelperBall();
            _ballShooter.SetColorModel(new BallShootModel(), false);

            await _boosterPanel.SpawnColorBall(ballModel.BallColor, _ballShooter.ShotPoint.position);
            _ballShooter.SetColorModel(ballModel, true);
            _inputProcessor.IsActive = true;
            _canExecute = true;
        }
    }
}
