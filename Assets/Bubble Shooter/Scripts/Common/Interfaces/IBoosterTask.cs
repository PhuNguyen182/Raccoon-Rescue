using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IBoosterTask
    {
        public UniTask Execute(Vector3Int position);
    }
}
