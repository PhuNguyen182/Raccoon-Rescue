using BubbleShooter.Scripts.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class MatchBallHandler
    {
        private readonly BreakGridTask _breakGridTask;

        public MatchBallHandler(BreakGridTask breakGridTask)
        {
            _breakGridTask = breakGridTask;
        }

        public bool CheckCluster(IBallEntity ball, List<IGridCell> cluster)
        {
            if (cluster.Count < 3)
                return false;

            List<Vector3Int> balls = new();

            for (int i = 0; i < cluster.Count; i++)
            {
                if (cluster[i].BallEntity.EntityType == ball.EntityType)
                    balls.Add(cluster[i].GridPosition);
            }

            if (balls.Count < 3)
                return false;

            ExecuteCluster(balls);
            return true;
        }

        private void ExecuteCluster(List<Vector3Int> clusterPositions)
        {
            for (int i = 0; i < clusterPositions.Count; i++)
            {
                _breakGridTask.Break(clusterPositions[i]);
            }
        }
    }
}
