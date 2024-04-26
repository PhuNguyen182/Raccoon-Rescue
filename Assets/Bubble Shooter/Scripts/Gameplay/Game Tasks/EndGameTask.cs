using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Constants;
using BubbleShooter.Scripts.Gameplay.Strategies;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;
using MessagePipe;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class EndGameTask : IDisposable
    {
        private readonly MetaBallManager _metaBallManager;
        private readonly GridCellManager _gridCellManager;

        private readonly ISubscriber<BallDestroyMessage> _ballDestroySubscriber;

        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _tokenSource;
        private readonly IDisposable _disposable;

        private int _fallBallCount = 0;

        public EndGameTask(GridCellManager gridCellManager, MetaBallManager metaBallManager)
        {
            _gridCellManager = gridCellManager;
            _metaBallManager = metaBallManager;

            _tokenSource = new();
            _cancellationToken = _tokenSource.Token;

            DisposableBagBuilder builder = DisposableBag.CreateBuilder();

            _ballDestroySubscriber = GlobalMessagePipe.GetSubscriber<BallDestroyMessage>();
            _ballDestroySubscriber.Subscribe(OnBallFall).AddTo(builder);

            _disposable = builder.Build();
        }

        public async UniTask OnWinGame()
        {
            var fixedBalls = _metaBallManager.GetFixedEntities();
            _fallBallCount = fixedBalls.Count;

            foreach (IBallEntity fixedBall in fixedBalls)
            {
                fixedBall.IsFallen = true;
                fixedBall.IsEndOfGame = true;

                if (fixedBall is IBallPhysics ballPhysics)
                {
                    if (fixedBall is IBallGraphics ballGraphics)
                        ballGraphics.ChangeLayer(BallConstants.HigherLayer);

                    ballPhysics.SetBodyActive(true);
                    BallAddForce(ballPhysics);
                    _gridCellManager.Remove(fixedBall.GridPosition);
                }
            }

            await UniTask.WaitUntil(IsOutOfBall, cancellationToken: _cancellationToken);
            if (_cancellationToken.IsCancellationRequested) return;
        }

        public async UniTask OnLoseGame()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _cancellationToken);
        }

        private void OnBallFall(BallDestroyMessage message)
        {
            if (message.IsEndOfGame)
            {
                _fallBallCount = _fallBallCount - 1;
            }
        }

        private bool IsOutOfBall()
        {
            return _fallBallCount <= 0;
        }

        private void BallAddForce(IBallPhysics ballPhysics)
        {
            float x = Random.Range(-0.25f, 0.25f);
            float y = Random.Range(0.5f, 1f);

            Vector2 forceUnit = new(x, y);
            forceUnit.Normalize();

            float forceMagnitude = Random.Range(BallConstants.MinForce, BallConstants.MaxForce);
            ballPhysics.AddForce(forceMagnitude * forceUnit, ForceMode2D.Impulse);
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _tokenSource.Dispose();
        }
    }
}
