using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IBallMovement
    {
        public bool CanMove { get; set; }

        public void SetBodyActive(bool active);
        public void SetMoveDirection(Vector2 direction);
        public UniTask SnapTo(Vector3 position);
    }
}
