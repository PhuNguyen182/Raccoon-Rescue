using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class BallRippleTask : IDisposable
    {
        private readonly GridCellManager _gridCellManager;

        private const float RippleMagnitude = 0.3f;

        private List<IBallEntity> _affectedBalls;

        public BallRippleTask(GridCellManager gridCellManager)
        {
            _affectedBalls = new();
            _gridCellManager = gridCellManager;
        }

        public async UniTask RippleAt(Vector3Int position, int level)
        {
            if (level <= 0)
                return;

            IGridCell currentCell = _gridCellManager.Get(position);
            IBallEntity currentBall = currentCell.BallEntity;

            if (currentBall == null)
                return;

            currentBall.RippleIgnore = true;
            _affectedBalls.Add(currentBall);

            List<Vector3Int> neighbour = GetNeighbourPositions(position);

            using (PooledObject<List<UniTask>> pool = ListPool<UniTask>.Get(out List<UniTask> moveTasks))
            {
                for (int i = 0; i < neighbour.Count; i++)
                {
                    IGridCell gridCell = _gridCellManager.Get(neighbour[i]);

                    if (gridCell == null)
                        continue;
                    
                    if (!gridCell.ContainsBall)
                        continue;

                    IBallEntity ballEntity = gridCell.BallEntity;
                    
                    if (ballEntity.RippleIgnore)
                        continue;

                    ballEntity.RippleIgnore = true;
                    Vector3 currentPosition = GetBallPosition(position);
                    Vector3 nextPosition = GetBallPosition(ballEntity.GridPosition);
                    Vector3 dir = (nextPosition - currentPosition).normalized;

                    if (ballEntity is IBallMovement movement)
                    {
                        _affectedBalls.Add(ballEntity);
                        float rippleAmount = RippleMagnitude * Mathf.Log(level + 1, 10);
                        moveTasks.Add(movement.BounceMove(nextPosition + dir * rippleAmount));
                    }
                }

                for (int i = 0; i < neighbour.Count; i++)
                {
                    IGridCell gridCell = _gridCellManager.Get(neighbour[i]);

                    if (gridCell == null)
                        continue;

                    if (!gridCell.ContainsBall)
                        continue;

                    RippleAt(neighbour[i], level - 1).Forget();
                }

                await UniTask.WhenAll(moveTasks);
            }
        }

        public void ResetRippleIgnore()
        {
            for (int i = 0; i < _affectedBalls.Count; i++)
            {
                _affectedBalls[i].RippleIgnore = false;
            }

            _affectedBalls.Clear();
        }

        private List<Vector3Int> GetNeighbourPositions(Vector3Int checkPosition)
        {
            List<Vector3Int> chainPositions = new();

            for (int i = 0; i < CommonProperties.MaxNeighborCount; i++)
            {
                Vector3Int neighborOffset = checkPosition.y % 2 == 0
                                            ? CommonProperties.EvenYNeighborOffsets[i]
                                            : CommonProperties.OddYNeighborOffsets[i];
                chainPositions.Add(checkPosition + neighborOffset);
            }

            return chainPositions;
        }

        private Vector3 GetBallPosition(Vector3Int gridPosition)
        {
            return _gridCellManager.ConvertGridToWorldFunction(gridPosition);
        }

        public void Dispose()
        {
            _affectedBalls.Clear();
        }
    }
}
