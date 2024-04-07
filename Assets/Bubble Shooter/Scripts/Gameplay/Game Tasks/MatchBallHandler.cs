using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class MatchBallHandler
    {
        private readonly BreakGridTask _breakGridTask;

        public MatchBallHandler(BreakGridTask breakGridTask)
        {
            _breakGridTask = breakGridTask;
        }

        public async UniTask<bool> CheckCluster(IBallEntity ball, List<IGridCell> cluster)
        {
            if (cluster.Count < 3)
                return false;

            using (var ballCluster = ListPool<Vector3Int>.Get(out var balls))
            {
                for (int i = 0; i < cluster.Count; i++)
                {
                    if (cluster[i].BallEntity.EntityType == ball.EntityType)
                        balls.Add(cluster[i].GridPosition);
                }

                if (balls.Count < 3)
                    return false;

                await ExecuteCluster(balls);
                return true;
            }
        }

        private async UniTask ExecuteCluster(List<Vector3Int> clusterPositions)
        {
            using (var cluster = ListPool<UniTask>.Get(out var listPool))
            {
                for (int i = 0; i < clusterPositions.Count; i++)
                {
                    listPool.Add(_breakGridTask.Break(clusterPositions[i]));
                }

                await UniTask.WhenAll(listPool);
            }
        }
    }
}
