using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.Boosters
{
    public class BaseBooster : BaseEntity, IBallBooster
    {
        public override bool IsMatchable => false;

        public override bool IsFixedOnStart { get; set; }

        public override Vector3 WorldPosition => transform.position;

        public override Vector3Int GridPosition { get; set; }

        public override EntityType EntityType { get; }

        public virtual UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public override void Destroy()
        {
            SimplePool.Despawn(this.gameObject);
        }

        public virtual UniTask Explode()
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
