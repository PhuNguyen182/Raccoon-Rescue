using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IBallEntity
    {
        public bool IsFixedOnStart { get; set; }
        public Vector3 WorldPosition { get; }
        public Vector3Int GridPosition { get; set; }
        public EntityType EntityType { get; set; }

        public void Blast();
        public void Destroy();
    }
}
