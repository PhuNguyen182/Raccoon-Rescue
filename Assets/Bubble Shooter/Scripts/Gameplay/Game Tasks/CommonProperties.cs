using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public static class CommonProperties
    {
        public const int MaxNeighborCount = 6;

        public static Vector3Int[] EvenYNeighborOffsets => new Vector3Int[]
        {
            new(0, 1), new(1, 0), new(0, -1),
            new(-1, -1), new(-1, 0), new(-1, 1),
        };

        public static Vector3Int[] OddYNeighborOffsets => new Vector3Int[]
        {
            new(0, 1), new(1, 1), new(1, 0),
            new(1, -1), new(0, -1), new(-1, 0),
        };
    }
}
