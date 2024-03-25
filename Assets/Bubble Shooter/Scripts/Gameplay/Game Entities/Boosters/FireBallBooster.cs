using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.Boosters
{
    public class FireBallBooster : BaseEntities, IBallBooster
    {
        public EntityType BoosterType => EntityType.FireBall;

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
