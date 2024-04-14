using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Messages;
using Cysharp.Threading.Tasks;
using MessagePipe;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class MatchBallHandler : IDisposable
    {
        private readonly BreakGridTask _breakGridTask;
        private readonly CheckBallClusterTask _checkBallClusterTask;
        private readonly GridCellManager _gridCellManager;

        private List<IGridCell> _matchCluster = new();
        private ISubscriber<CheckMatchMessage> _checkMatchSubscriber;
        private IDisposable _disposable;

        public MatchBallHandler(GridCellManager gridCellManager, BreakGridTask breakGridTask, CheckBallClusterTask checkBallClusterTask)
        {
            _gridCellManager = gridCellManager;
            _checkBallClusterTask = checkBallClusterTask;
            _breakGridTask = breakGridTask;

            var builder = DisposableBag.CreateBuilder();

            _checkMatchSubscriber = GlobalMessagePipe.GetSubscriber<CheckMatchMessage>();
            _checkMatchSubscriber.Subscribe(Match).AddTo(builder);

            _disposable = builder.Build();
        }

        public void Match(CheckMatchMessage message)
        {
            MatchAsync(message.Position).Forget();
        }

        private async UniTask MatchAsync(Vector3Int position)
        {
            _matchCluster.Clear();
            CheckMatch(position);
            
            _gridCellManager.ClearVisitedPositions();
            bool isCluster = await ExecuteCluster(_matchCluster);
            
            if(isCluster)
                _checkBallClusterTask.CheckCluster();
        }

        private void CheckMatch(Vector3Int position)
        {
            IGridCell gridCell = _gridCellManager.Get(position);
            if (gridCell == null)
                return;

            IBallEntity ballEntity = gridCell.BallEntity;
            if (ballEntity == null)
                return;

            var neighbours = _gridCellManager.GetNeighbourGrids(position);

            if (ballEntity.EntityType != EntityType.ColorfulBall)
            {
                for (int i = 0; i < neighbours.Count; i++)
                {
                    if (neighbours[i] == null)
                        continue;

                    if (_gridCellManager.GetIsVisited(neighbours[i].GridPosition))
                        continue;

                    if (!neighbours[i].ContainsBall)
                        continue;

                    // XOR operator: if the ball is not matchable and is booster or is matchable and is not booster
                    // Can use != instead of ^
                    if (!(neighbours[i].BallEntity.IsMatchable ^ neighbours[i].BallEntity is IBallBooster))
                        continue;

                    if(neighbours[i].EntityType != EntityType.ColorfulBall)
                        if (neighbours[i].EntityType != ballEntity.EntityType)
                            continue;

                    _matchCluster.Add(neighbours[i]);
                    _gridCellManager.SetAsVisited(neighbours[i].GridPosition);
                    CheckMatch(neighbours[i].GridPosition);
                }
            }

            else
            {
                for (int i = 0; i < neighbours.Count; i++)
                {
                    if (neighbours[i] == null)
                        continue;

                    if (!neighbours[i].ContainsBall)
                        continue;

                    if (!neighbours[i].BallEntity.IsMatchable)
                        continue;

                    if (neighbours[i].EntityType == EntityType.ColorfulBall)
                        continue;

                    CheckMatch(neighbours[i].GridPosition);
                }
            }
        }

        private async UniTask<bool> ExecuteCluster(List<IGridCell> cluster)
        {
            if (cluster.Count < 3)
                return false;

            using (var listPool = ListPool<UniTask>.Get(out var breakTask))
            {
                for (int i = 0; i < cluster.Count; i++)
                {
                    breakTask.Add(_breakGridTask.Break(cluster[i]));
                }

                await UniTask.WhenAll(breakTask);
            }

            return true;
        }

        public void Dispose()
        {
            _matchCluster.Clear();
            _disposable.Dispose();
        }
    }
}
