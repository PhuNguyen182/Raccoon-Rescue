using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IGridCell
    {
        public bool IsCeil { get; set; }
        public bool IsBottom { get; set; }
        public bool ContainsBall { get; }
        public EntityType EntityType { get; }
        public IBallEntity BallEntity { get; }
        public Vector3Int GridPosition { get; set; }
        public Vector3 WorldPosition { get; set; }

        public void SetBall(IBallEntity ball);
        public bool Destroy();
    }
}
