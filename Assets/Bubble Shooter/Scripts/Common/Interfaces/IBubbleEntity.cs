using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IBubbleEntity
    {
        public Vector3 WorldPosition { get; }
        public Vector3Int GridPosition { get; set; }
        public EntityType EntityType { get; set; }

        public void Blast();
        public void Clear();
    }
}
