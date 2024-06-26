using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.Models;

namespace BubbleShooter.Scripts.Common.Configs
{
    public class PlayConfig
    {
        public static PlayConfig Current;

        public int Level;
        public bool IsTest;
        public LevelModel LevelModel;

        public bool UseColorful;
        public bool UseAiming;
        public bool UseExtraBall;
    }
}
