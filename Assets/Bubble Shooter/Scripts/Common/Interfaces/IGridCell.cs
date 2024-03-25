using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IGridCell
    {
        public EntityType EntityType { get; }
        public IBallEntity BallEntity { get; }
        public Vector3Int GridPosition { get; set; }

        public void SetBall(IBallEntity ball);
        public bool Destroy();
    }
}
