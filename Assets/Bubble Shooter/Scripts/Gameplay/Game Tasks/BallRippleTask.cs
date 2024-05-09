using System;
using System.Threading;
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

        private readonly CancellationToken _token;
        private readonly CancellationTokenSource _cts;

        private HashSet<Vector3Int> _level1 = new();
        private HashSet<Vector3Int> _level2 = new();
        private HashSet<Vector3Int> _level3 = new();

        private const float RippleMagnitude = 0.075f;

        public BallRippleTask(GridCellManager gridCellManager)
        {
            _gridCellManager = gridCellManager;

            _cts = new();
            _token = _cts.Token;
        }

        public async UniTask RippleAt(Vector3Int position, int level)
        {
            if (level <= 0)
                return;

            IGridCell currentCell = _gridCellManager.Get(position);
            IBallEntity currentBall = currentCell.BallEntity;
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
                    Vector3 dir = ballEntity.WorldPosition - currentBall.WorldPosition;

                    if (ballEntity is IBallMovement movement)
                        moveTasks.Add(movement.BounceMove(ballEntity.WorldPosition + dir.normalized * RippleMagnitude * Mathf.Log(level + 1, 2)));

                    RippleAt(neighbour[i], level - 1).Forget();
                }

                await UniTask.WhenAll(moveTasks);
            }
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

        public void Dispose()
        {
            _level1.Clear();
            _level2.Clear();
            _level3.Clear();
            _cts.Dispose();
        }
    }
}
