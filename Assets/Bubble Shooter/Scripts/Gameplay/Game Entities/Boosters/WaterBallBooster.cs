using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.Boosters
{
    public class WaterBallBooster : BaseEntity, IBallBooster
    {
        public override EntityType EntityType => EntityType.WaterBall;

        public override bool IsMatchable => false;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public override void Destroy()
        {

        }

        public UniTask Explode()
        {
            return UniTask.CompletedTask;
        }

        public override void InitMessages()
        {

        }

        public override void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
