using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public abstract class BaseBall : BaseEntities, IBallEntity
    {
        public EntityType EntityType { get; set; }

        public Vector3Int GridPosition { get; set; }

        public Vector3 WorldPosition => transform.position;

        public abstract void Blast();

        public abstract void Clear();
    }
}
