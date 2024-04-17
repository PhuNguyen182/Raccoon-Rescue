using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.GameBoard;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class InputProcessor : IDisposable
    {
        private readonly InputHandler _inputHandler;

        public bool IsActive
        {
            get => _inputHandler.IsActive;
            set => _inputHandler.IsActive = value;
        }

        public InputProcessor(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        public void Dispose()
        {

        }
    }
}
