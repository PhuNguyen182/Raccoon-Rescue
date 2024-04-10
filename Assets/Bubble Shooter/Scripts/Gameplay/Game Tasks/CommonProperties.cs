using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public static class CommonProperties
    {
        public const int MaxNeighborCount = 6;

        public static Vector3Int[] NeighborOffsets => new Vector3Int[]
        {
            new Vector3Int(0, 1),
            new Vector3Int(1, 0),
            new Vector3Int(0, -1),
            new Vector3Int(-1, -1),
            new Vector3Int(-1, 0),
            new Vector3Int(-1, 1),
        };
    }
}
