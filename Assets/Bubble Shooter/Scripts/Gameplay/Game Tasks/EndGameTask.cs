using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Constants;
using BubbleShooter.Scripts.Gameplay.Strategies;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.Gameplay.Miscs;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;
using MessagePipe;
using DG.Tweening;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class EndGameTask : IDisposable
    {
        private readonly BallShooter _ballShooter;
        private readonly BallProvider _ballProvider;
        private readonly MetaBallManager _metaBallManager;
        private readonly ISubscriber<BallDestroyMessage> _ballDestroySubscriber;

        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _tokenSource;
        private readonly IDisposable _disposable;

        private int _fallBallCount = 0;
        private Material _ballMaterial;
        private static readonly int _greyScaleProperty = Shader.PropertyToID("_Modifier");

        public EndGameTask(MetaBallManager metaBallManager, BallShooter ballShooter, BallProvider ballProvider)
        {
            _ballShooter = ballShooter;
            _ballProvider = ballProvider;
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
            await UniTask.NextFrame(_cancellationToken);
            
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
                }
            }

            await UniTask.WaitUntil(IsOutOfBall, cancellationToken: _cancellationToken);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _cancellationToken);
            if (_cancellationToken.IsCancellationRequested) return;
        }

        public async UniTask OnLoseGame()
        {
            // Turn off current ball
            _ballShooter.SetColorModel(new BallShootModel { }, false);

            float greyScale = 0;
            await DOTween.To(() => greyScale, x => greyScale = x, 1, 1).OnUpdate(() =>
            {
                _ballMaterial.SetFloat(_greyScaleProperty, greyScale);
            }).SetEase(Ease.InOutSine);

            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _cancellationToken);
        }

        public void SetMaterial(Material material)
        {
            _ballMaterial = material;
        }

        public void ResetBallColor()
        {
            if (_ballMaterial != null)
                _ballMaterial.SetFloat(_greyScaleProperty, 0);
        }

        public void ContinueSpawnBall()
        {
            _ballProvider.PopSequence().Forget();
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
