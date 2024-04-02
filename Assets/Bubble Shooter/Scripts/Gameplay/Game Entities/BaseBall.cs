using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public abstract class BaseBall : BubbleEntity, IBallEntity, IBreakable
    {
        public bool IsFixedOnStart { get; set; }

        public Vector3Int GridPosition { get; set; }

        public Vector3 WorldPosition => transform.position;

        public abstract EntityType EntityType { get; }

        public abstract bool IsMatchable { get; }

        public abstract UniTask Blast();

        public abstract bool Break();

        public abstract void Destroy();

        public void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
