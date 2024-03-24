using BubbleShooter.Scripts.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class EntityManager
    {
        // To do: write a methof creating a collection of a ball cluster

        public bool CheckCluster(IBallEntity ball, List<IGridCell> cluster)
        {
            if (cluster.Count < 3)
                return false;

            List<IBallEntity> balls = new();

            for (int i = 0; i < cluster.Count; i++)
            {
                if (cluster[i].BallEntity.EntityType == ball.EntityType)
                    balls.Add(cluster[i].BallEntity);
            }

            if (balls.Count < 3)
                return false;

            ExecuteCluster(balls);
            return true;
        }

        private void ExecuteCluster(List<IBallEntity> cluster)
        {

        }
    }
}
