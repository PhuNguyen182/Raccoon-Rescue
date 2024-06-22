using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Constants;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks
{
    public class LeafBallBoosterTask : IDisposable, IBoosterTask
    {
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;

        private CancellationToken _cancellationToken;
        private CancellationTokenSource _tokenSource;

        public LeafBallBoosterTask(GridCellManager gridCellManager, BreakGridTask breakGridTask)
        {
            _gridCellManager = gridCellManager;
            _breakGridTask = breakGridTask;

            _tokenSource = new();
            _cancellationToken = _tokenSource.Token;
        }

        public async UniTask Execute(Vector3Int position)
        {
            IGridCell boosterCell = _gridCellManager.Get(position);

            if (boosterCell.BallEntity is IBallBooster booster)
                await booster.Activate();

            _gridCellManager.DestroyAt(position);
            using (var breakListPool = ListPool<UniTask>.Get(out List<UniTask> breakTasks))
            {
                using (var boosterListPool = ListPool<IBallPlayBoosterEffect>.Get(out var boosterEffects))
                {
                    var column = GetVerticalLine(position);

                    for (int i = 0; i < column.Count; i++)
                    {
                        if (column[i] == null)
                            continue;

                        if (position == column[i].GridPosition)
                            continue;

                        if (!column[i].ContainsBall)
                            continue;

                        if (column[i].BallEntity is IBallBooster ballBooster)
                            if (ballBooster.IsIgnored)
                                continue;

                        if (column[i].BallEntity is IBallPlayBoosterEffect boosterEffect)
                        {
                            boosterEffects.Add(boosterEffect);
                            await boosterEffect.PlayBoosterEffect(EntityType.LeafBall);
                        }

                        UniTask breakTask = _breakGridTask.Break(column[i]);
                        breakTasks.Add(breakTask);
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _cancellationToken);
                    await UniTask.WhenAll(breakTasks);
                    for (int i = 0; i < boosterEffects.Count; i++)
                        boosterEffects[i].ReleaseEffect();
                }
            }
        }

        private List<IGridCell> GetVerticalLine(Vector3Int startPosition)
        {
            List<IGridCell> column = new();

            for (int i = 1; i < BoosterConstants.LeafBoosterAttackRange - 1; i++)
            {
                IGridCell gridCell = _gridCellManager.Get(startPosition + Vector3Int.up * i);
                column.Add(gridCell);
            }

            return column;
        }

        public void Dispose()
        {
            _tokenSource.Dispose();
        }
    }
}
