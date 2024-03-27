using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.Boosters
{
    public abstract class BaseBooster : BubbleEntity, IBallBooster
    {
        public abstract EntityType BoosterType { get; }

        public abstract UniTask Activate();

        public abstract UniTask Explode();
    }
}
