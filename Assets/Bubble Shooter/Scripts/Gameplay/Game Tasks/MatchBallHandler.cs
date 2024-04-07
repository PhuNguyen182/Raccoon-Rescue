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
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;

        private List<IGridCell> _matchCluster = new();

        public MatchBallHandler(GridCellManager gridCellManager, BreakGridTask breakGridTask)
        {
            _gridCellManager = gridCellManager;
            _breakGridTask = breakGridTask;
        }

        public void Match(Vector3Int position)
        {
            MatchAsync(position).Forget();
        }

        private async UniTask MatchAsync(Vector3Int position)
        {
            _matchCluster.Clear();
            CheckMatch(position);
            await ExecuteCluster(_matchCluster);
            _gridCellManager.ClearVisitedPositions();
        }

        private void CheckMatch(Vector3Int position)
        {
            IGridCell gridCell = _gridCellManager.Get(position);
            IBallEntity ballEntity = gridCell.BallEntity;

            var neighbours = _gridCellManager.GetNeighbourGrids(position);
            
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i] == null)
                    continue;

                if (!neighbours[i].ContainsBall)
                    continue;

                if (neighbours[i].BallEntity.EntityType != ballEntity.EntityType)
                    continue;

                if (!_gridCellManager.IsVisited(neighbours[i].GridPosition))
                {
                    _matchCluster.Add(neighbours[i]);
                    _gridCellManager.MarkAsVisited(neighbours[i].GridPosition);
                    CheckMatch(neighbours[i].GridPosition);
                }
            }
        }

        private async UniTask ExecuteCluster(List<IGridCell> cluster)
        {
            using (var listPool = ListPool<UniTask>.Get(out var breakTask))
            {
                for (int i = 0; i < cluster.Count; i++)
                {
                    breakTask.Add(_breakGridTask.Break(cluster[i]));
                }

                await UniTask.WhenAll(breakTask);
            }
        }
    }
}
