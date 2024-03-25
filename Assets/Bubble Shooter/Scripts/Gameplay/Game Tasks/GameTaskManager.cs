using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameTasks 
{
    public class GameTaskManager : IDisposable
    {
        private readonly GridCellManager _gridCellManager;

        public GameTaskManager()
        {
            _gridCellManager = new();
        }

        public void Dispose()
        {
            
        }
    } 
}
