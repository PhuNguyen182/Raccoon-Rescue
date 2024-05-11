using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.Inputs;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class InputProcessor : IDisposable
    {
        private readonly InputController _inputHandler;

        public bool IsActive
        {
            get => _inputHandler.IsActive;
            set => _inputHandler.IsActive = value;
        }

        public InputProcessor(InputController inputHandler)
        {
            _inputHandler = inputHandler;
        }

        public void Dispose()
        {

        }
    }
}
