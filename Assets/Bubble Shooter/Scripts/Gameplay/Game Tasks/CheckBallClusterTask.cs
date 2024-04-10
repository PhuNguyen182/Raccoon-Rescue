using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.Models;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class CheckBallClusterTask
    {
        private readonly GridCellManager _gridCellManager;
        
        public CheckBallClusterTask(GridCellManager gridCellManager)
        {
            _gridCellManager = gridCellManager;
        }

        public void CheckCluster()
        {
            for (int i = 0; i < _gridCellManager.GridPositions.Count; i++)
            {
                Vector3Int position = _gridCellManager.GridPositions[i];

                if (IsValidCell(position))
                {
                    BallClusterModel clusterModel = new();
                    FindCluster(position, clusterModel);
                    ExecuteCluster(clusterModel);
                    clusterModel.Dispose();
                }
            }

            _gridCellManager.ClearVisitedPositions();
        }

        private void FindCluster(Vector3Int position, BallClusterModel clusterModel)
        {
            if (IsValidCell(position))
                return;

            _gridCellManager.SetAsVisited(position);
            IGridCell gridCell = _gridCellManager.Get(position);
            clusterModel.Cluster.Add(gridCell);

            if (gridCell.BallEntity.IsCeilAttached)
                clusterModel.IsCeilAttached = true;

            for (int i = 0; i < CommonProperties.MaxNeighborCount; i++)
            {
                Vector3Int neighborOffset = CommonProperties.NeighborOffsets[i];
                FindCluster(position + neighborOffset, clusterModel);
            }
        }

        private bool IsValidCell(Vector3Int position)
        {
            bool hasVisited = _gridCellManager.GetIsVisited(position);
            bool containBall = _gridCellManager.Get(position).ContainsBall;
            return !hasVisited && containBall;
        }

        private void ExecuteCluster(BallClusterModel clusterModel)
        {
            if (clusterModel.IsCeilAttached)
                return;

            for (int i = 0; i < clusterModel.Cluster.Count; i++)
            {
                IBallEntity ball = clusterModel.Cluster[i].BallEntity;

                if (ball is IBallPhysics ballPhysics)
                {
                    ballPhysics.SetBodyActive(true);
                    _gridCellManager.Remove(clusterModel.Cluster[i].GridPosition);
                }
            }
        }
    }
}
