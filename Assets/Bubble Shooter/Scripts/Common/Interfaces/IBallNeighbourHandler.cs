using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IBallNeighbourHandler
    {
        public bool CheckNeighbours(out List<Vector3Int> neighbourPositions);
    }
}
