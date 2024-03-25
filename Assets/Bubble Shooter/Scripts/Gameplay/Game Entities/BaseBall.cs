using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public abstract class BaseBall : BaseEntities, IBallEntity, IBreakable
    {
        public abstract EntityType EntityType { get; }

        public Vector3Int GridPosition { get; set; }

        public Vector3 WorldPosition => transform.position;

        public bool IsFixedOnStart { get; set; }

        public abstract UniTask Blast();

        public abstract bool Break();

        public abstract void Destroy();

        public virtual void ResetBall() 
        {
            
        }
    }
}
