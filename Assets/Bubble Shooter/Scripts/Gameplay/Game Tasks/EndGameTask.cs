using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Constants;
using BubbleShooter.Scripts.Gameplay.Strategies;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.GameUI.Notifications;
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
        private readonly CheckTargetTask _checkTargetTask;
        private readonly NotificationPanel _notificationPanel;
        private readonly ISubscriber<BallDestroyMessage> _ballDestroySubscriber;

        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _tokenSource;
        private readonly IDisposable _disposable;

        private int _fallBallCount = 0;
        private Material _ballMaterial;
        private readonly int _greyScaleProperty;

        public EndGameTask(MetaBallManager metaBallManager, BallShooter ballShooter
            , BallProvider ballProvider, CheckTargetTask checkTargetTask
            , NotificationPanel notificationPanel)
        {
            _greyScaleProperty = Shader.PropertyToID("_Modifier");

            _ballShooter = ballShooter;
            _ballProvider = ballProvider;
            _metaBallManager = metaBallManager;
            _checkTargetTask = checkTargetTask;
            _notificationPanel = notificationPanel;

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

            MusicManager.Instance.PlaySoundEffect(SoundEffectEnum.Complete);
            var fixedBalls = _metaBallManager.GetFixedEntities();
            _fallBallCount = fixedBalls.Count + _checkTargetTask.MoveCount;

            foreach (IBallEntity fixedBall in fixedBalls)
            {
                fixedBall.IsFallen = true;
                fixedBall.IsEndOfGame = true;

                if (fixedBall is IBallPhysics ballPhysics)
                {
                    if (fixedBall is IBallGraphics ballGraphics)
                        ballGraphics.ChangeLayer(BallConstants.HigherLayer);

                    ballPhysics.SetBodyActive(true);
                    BallAddForce(ballPhysics, false);
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: _cancellationToken);
            await ShootRemainBalls();

            await UniTask.WaitUntil(IsOutOfBall, cancellationToken: _cancellationToken);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _cancellationToken);
            if (_cancellationToken.IsCancellationRequested) return;
            MusicManager.Instance.PlaySoundEffect(SoundEffectEnum.Cheer);
        }

        public async UniTask OnLoseGame()
        {
            _ballShooter.SetColorModel(new BallShootModel { }, false);
            MusicManager.Instance.PlaySoundEffect(SoundEffectEnum.Creak);

            await UniTask.Delay(TimeSpan.FromSeconds(0.8f), cancellationToken: _cancellationToken);
            await _notificationPanel.ShowLosePanel();

            float greyScale = 0;
            await DOTween.To(() => greyScale, x => greyScale = x, 1, 0.3f).OnUpdate(() =>
            {
                _ballMaterial.SetFloat(_greyScaleProperty, greyScale);
            }).SetEase(Ease.InOutSine);

            await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: _cancellationToken);
            MusicManager.Instance.PlaySoundEffect(SoundEffectEnum.GameOver);
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

        private async UniTask ShootRemainBalls()
        {
            int count = _checkTargetTask.MoveCount;
            _ballProvider.SetBallColor(false, EntityType.None, DummyBallState.None).Forget();
            
            while(count > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: _cancellationToken);

                EntityType color = _ballProvider.GetRandomColor();
                IBallEntity ball = _ballShooter.ShootFreeBall(color);

                if (ball is IBallPhysics ballPhysics)
                {
                    ballPhysics.SetBodyActive(true);
                    BallAddForce(ballPhysics, true);
                }

                SetBallEndGame(ball).Forget();
                count--;
            }
        }

        private async UniTask SetBallEndGame(IBallEntity ball)
        {
            ball.IsFallen = false;
            ball.IsEndOfGame = false;

            await UniTask.Delay(TimeSpan.FromSeconds(0.3f)
                                , cancellationToken: _cancellationToken);
            
            ball.IsEndOfGame = true;
            ball.IsFallen = true;
        }

        private bool IsOutOfBall()
        {
            return _fallBallCount <= 0;
        }

        private void BallAddForce(IBallPhysics ballPhysics, bool afterWin)
        {
            float x = !afterWin ? Random.Range(-0.25f, 0.25f) : Random.Range(-0.075f, 0.075f);
            float y = !afterWin ? Random.Range(0.5f, 1f) : 1f;

            Vector2 forceUnit = new(x, y);
            forceUnit.Normalize();

            float forceMagnitude = !afterWin ? Random.Range(BallConstants.MinForce, BallConstants.MaxForce)
                                             : BallConstants.WinForce;
            ballPhysics.AddForce(forceMagnitude * forceUnit, ForceMode2D.Impulse);
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _tokenSource.Dispose();
        }
    }
}
