using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.Boosters
{
    public class SunBallBooster : BaseEntities, IBallBooster
    {
        public EntityType BoosterType => EntityType.SunBall;

        public UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Explode()
        {
            return UniTask.CompletedTask;
        }
    }
}
