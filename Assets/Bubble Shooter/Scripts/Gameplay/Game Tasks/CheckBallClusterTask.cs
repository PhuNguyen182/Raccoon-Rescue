using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Constants;

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
            if (!IsValidCell(position))
                return;

            _gridCellManager.SetAsVisited(position);
            IGridCell gridCell = _gridCellManager.Get(position);
            clusterModel.Cluster.Add(gridCell);

            if (gridCell.IsCeil)
                clusterModel.IsCeilAttached = true;

            for (int i = 0; i < CommonProperties.MaxNeighborCount; i++)
            {
                Vector3Int neighborOffset = position.y % 2 == 0 
                                            ? CommonProperties.EvenYNeighborOffsets[i]
                                            : CommonProperties.OddYNeighborOffsets[i];
                FindCluster(position + neighborOffset, clusterModel);
            }
        }

        private bool IsValidCell(Vector3Int position)
        {
            IGridCell gridCell = _gridCellManager.Get(position);
            
            if (gridCell == null)
                return false;

            bool hasVisited = _gridCellManager.GetIsVisited(position);
            bool containBall = gridCell.ContainsBall;
            return !hasVisited && containBall;
        }

        private void ExecuteCluster(BallClusterModel clusterModel)
        {
            if (clusterModel.IsCeilAttached)
                return;

            for (int i = 0; i < clusterModel.Cluster.Count; i++)
            {
                IBallEntity ball = clusterModel.Cluster[i].BallEntity;

                if(ball is ITargetBall target)
                {
                    target.FreeTarget();
                    _gridCellManager.DestroyAt(clusterModel.Cluster[i].GridPosition);
                }

                else if (ball is IBallPhysics ballPhysics)
                {
                    if (ball is IBallGraphics ballGraphics)
                        ballGraphics.ChangeLayer(BallConstants.HigherLayer);

                    ballPhysics.SetBodyActive(true);
                    BallAddForce(ballPhysics);
                    _gridCellManager.Remove(clusterModel.Cluster[i].GridPosition);
                }
            }
        }

        private void BallAddForce(IBallPhysics ballPhysics)
        {
            float x = Random.Range(-0.25f, 0.25f);
            float y = Random.Range(0.5f, 1f);

            Vector2 forceUnit = new(x,y);
            forceUnit.Normalize();

            float forceMagnitude = Random.Range(BallConstants.MinForce, BallConstants.MaxForce);
            ballPhysics.AddForce(forceMagnitude * forceUnit, ForceMode2D.Impulse);
        }
    }
}
