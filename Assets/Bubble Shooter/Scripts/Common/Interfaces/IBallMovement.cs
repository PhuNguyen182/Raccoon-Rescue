using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IBallMovement
    {
        public bool CanMove { get; set; }
        public Vector2 MoveDirection { get; }
        public BallMovementState MovementState { get; set; }

        public void SetMoveDirection(Vector2 direction);
        public UniTask SnapTo(Vector3 position);
        public UniTask BounceMove(Vector3 position);

        public Func<Vector3, Vector3Int> WorldToGridFunction { get; set; }
        public Func<Vector3Int, IGridCell> TakeGridCellFunction { get; set; }
    }
}
