using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.Boosters
{
    public class WaterBallBooster : BaseBooster, IBallBooster
    {
        public override EntityType BoosterType => EntityType.WaterBall;

        public override UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask Explode()
        {
            return UniTask.CompletedTask;
        }

        public override void InitMessages()
        {
            
        }
    }
}
