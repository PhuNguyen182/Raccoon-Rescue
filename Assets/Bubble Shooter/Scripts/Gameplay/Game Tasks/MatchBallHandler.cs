using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks;
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
        private readonly BallRippleTask _ballRippleTask;
        private readonly MoveGameViewTask _moveGameViewTask;
        private readonly IngameBoosterHandler _ingameBoosterHandler;

        private readonly IPublisher<PowerupMessage> _powerupPublisher;
        private readonly IPublisher<PublishScoreMessage> _addScorePublisher;
        private readonly ISubscriber<CheckMatchMessage> _checkMatchSubscriber;

        private const int RippleSpreadLevel = 3;

        private bool _isMatchWithColorful;
        
        private CancellationTokenSource _tokenSource;
        private GameStateController _gameStateController;
        
        private CancellationToken _token;
        private IDisposable _disposable;

        public MatchBallHandler(GridCellManager gridCellManager, BreakGridTask breakGridTask , CheckBallClusterTask checkBallClusterTask
            , InputProcessor inputProcessor, CheckTargetTask checkTargetTask, MoveGameViewTask moveGameViewTask
            , BallRippleTask ballRippleTask, IngameBoosterHandler ingameBoosterHandler)
        {
            _gridCellManager = gridCellManager;
            _checkBallClusterTask = checkBallClusterTask;
            _checkTargetTask = checkTargetTask;
            _breakGridTask = breakGridTask;
            _moveGameViewTask = moveGameViewTask;
            _ingameBoosterHandler = ingameBoosterHandler;

            _tokenSource = new();
            _token = _tokenSource.Token;

            var builder = DisposableBag.CreateBuilder();

            _powerupPublisher = GlobalMessagePipe.GetPublisher<PowerupMessage>();
            _addScorePublisher = GlobalMessagePipe.GetPublisher<PublishScoreMessage>();
            _checkMatchSubscriber = GlobalMessagePipe.GetSubscriber<CheckMatchMessage>();
            _checkMatchSubscriber.Subscribe(Match).AddTo(builder);

            _disposable = builder.Build();
            _inputProcessor = inputProcessor;
            _ballRippleTask = ballRippleTask;
            _isMatchWithColorful = false;
        }

        public void SetGameStateController(GameStateController controller)
        {
            _gameStateController = controller;
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
                await _checkBallClusterTask.CheckNeighborCluster(position);
                await _ballRippleTask.RippleAt(position, RippleSpreadLevel);
                _ballRippleTask.ResetRippleIgnore();

                (bool, bool) clusterResult = await ExecuteCluster(matchCluster);
                
                _isMatchWithColorful = false;
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
                    await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: _token);
                    _checkTargetTask.CheckTarget();
                }

                await _moveGameViewTask.Check();
                _ingameBoosterHandler.AfterUseBooster();
                
                if(!_gameStateController.IsEndGame)
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
                _isMatchWithColorful = true;
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

            _powerupPublisher.Publish(new PowerupMessage
            {
                Amount = cluster.Count,
                PowerupColor = GetBoosterColor(cluster[2].EntityType),
                Command = ReactiveValueCommand.Changing
            });

            for (int i = 0; i < cluster.Count; i++)
            {
                if (cluster[i].BallEntity is ITargetBall)
                    containTarget = true;

                if (_isMatchWithColorful && cluster[i].BallEntity is IBallEffect effect)
                    effect.PlayColorfulEffect();

                totalScore += cluster[i].BallEntity.Score;
                await _breakGridTask.Break(cluster[i]);
            }

            _addScorePublisher.Publish(new PublishScoreMessage { Score = totalScore });

            return (true, containTarget);
        }

        private EntityType GetBoosterColor(EntityType entityType)
        {
            return entityType switch
            {
                EntityType.Blue => EntityType.WaterBall,
                EntityType.Green => EntityType.LeafBall,
                EntityType.Red => EntityType.FireBall,
                EntityType.Yellow => EntityType.SunBall,
                _ => EntityType.None
            };
        }

        public void Dispose()
        {
            _tokenSource.Dispose();
            _disposable.Dispose();
        }
    }
}
