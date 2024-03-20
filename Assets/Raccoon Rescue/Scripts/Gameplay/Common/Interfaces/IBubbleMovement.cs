using UnityEngine;
using Cysharp.Threading.Tasks;

namespace RaccoonRescue.Scripts.Gameplay.Common.Interfaces
{
    public interface IBubbleMovement
    {
        public UniTask SnapTo(Vector3 position);
    }
}
