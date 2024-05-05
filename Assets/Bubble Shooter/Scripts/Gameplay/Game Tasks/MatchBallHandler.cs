using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class MatchBallHandler : IDisposable
    {
        private readonly InputProcessor _inputProcessor;
        private readonly BreakGridTask _breakGridTask;
        private readonly CheckBallClusterTask _checkBallClusterTask;
        private readonly GridCellManager _gridCellManager;
        private readonly CheckTargetTask _checkTargetTask;
        private readonly BoardThresholdCheckTask _boardThresholdCheckTask;

        private readonly IPublisher<PowerupMessage> _powerupPublisher;
        private readonly IPublisher<PublishScoreMessage> _addScorePublisher;
        private readonly ISubscriber<CheckMatchMessage> _checkMatchSubscriber;

        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;
        private IDisposable _disposable;

        public MatchBallHandler(GridCellManager gridCellManager, BreakGridTask breakGridTask
            , CheckBallClusterTask checkBallClusterTask, InputProcessor inputProcessor
            , CheckTargetTask checkTargetTask, BoardThresholdCheckTask boardThresholdCheckTask)
        {
            _gridCellManager = gridCellManager;
            _checkBallClusterTask = checkBallClusterTask;
            _checkTargetTask = checkTargetTask;
            _breakGridTask = breakGridTask;
            _boardThresholdCheckTask = boardThresholdCheckTask;

            _tokenSource = new();
            _token = _tokenSource.Token;

            var builder = DisposableBag.CreateBuilder();

            _powerupPublisher = GlobalMessagePipe.GetPublisher<PowerupMessage>();
            _addScorePublisher = GlobalMessagePipe.GetPublisher<PublishScoreMessage>();
            _checkMatchSubscriber = GlobalMessagePipe.GetSubscriber<CheckMatchMessage>();
            _checkMatchSubscriber.Subscribe(Match).AddTo(builder);

            _disposable = builder.Build();
            _inputProcessor = inputProcessor;
        }

        public void Match(CheckMatchMessage message)
        {
            MatchAsync(message.Position).Forget();
        }

        private async UniTask MatchAsync(Vector3Int position)
        {
            using (var listPool = ListPool<IGridCell>.Get(out var matchCluster))
            {
                _inputProcessor.IsActive = false;
                CheckMatch(position, matchCluster);

                _gridCellManager.ClearVisitedPositions();
                (bool, bool) clusterResult = await ExecuteCluster(matchCluster);
                
                bool isMatched = clusterResult.Item1;
                bool containTarget = clusterResult.Item2;

                if (isMatched)
                {
                    _checkBallClusterTask.CheckFreeCluster();
                    
                    if (!containTarget)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: _token);
                        _checkTargetTask.CheckTarget();
                    }
                }
                else
                {
                    await _checkBallClusterTask.CheckNeighborCluster(position);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: _token);
                    _checkTargetTask.CheckTarget();
                }

                _boardThresholdCheckTask.Check();
                _inputProcessor.IsActive = true;
            }
        }

        private void CheckMatch(Vector3Int position, List<IGridCell> matchCluster)
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

                    matchCluster.Add(neighbours[i]);
                    _gridCellManager.SetAsVisited(neighbours[i].GridPosition);
                    CheckMatch(neighbours[i].GridPosition, matchCluster);
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

                    CheckMatch(neighbours[i].GridPosition, matchCluster);
                }
            }
        }

        private async UniTask<(bool, bool)> ExecuteCluster(List<IGridCell> cluster)
        {
            if (cluster.Count < 3)
                return (false, false);

            int totalScore = 0;
            bool containTarget = false;
            
            using (var listPool = ListPool<UniTask>.Get(out var breakTask))
            {
                _powerupPublisher.Publish(new PowerupMessage
                {
                    Amount = cluster.Count,
                    PowerupColor = cluster[2].EntityType,
                    Command = ReactiveValueCommand.Changing
                });

                for (int i = 0; i < cluster.Count; i++)
                {
                    if (cluster[i].BallEntity is ITargetBall)
                        containTarget = true;

                    totalScore += cluster[i].BallEntity.Score; 
                    breakTask.Add(_breakGridTask.Break(cluster[i]));
                }

                _addScorePublisher.Publish(new PublishScoreMessage { Score = totalScore });

                await UniTask.WhenAll(breakTask);
            }

            return (true, containTarget);
        }

        public void Dispose()
        {
            _tokenSource.Dispose();
            _disposable.Dispose();
        }
    }
}
