using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Effects;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks
{
    public class WaterBallBoosterTask : IDisposable, IBoosterTask
    {
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;

        public WaterBallBoosterTask(GridCellManager gridCellManager, BreakGridTask breakGridTask)
        {
            _gridCellManager = gridCellManager;
            _breakGridTask = breakGridTask;
        }

        public async UniTask Execute(Vector3Int position)
        {
            IGridCell boosterCell = _gridCellManager.Get(position);

            if (boosterCell.BallEntity is IBallBooster booster)
                await booster.Activate();

            Vector3 pos = new Vector3(0, boosterCell.WorldPosition.y);
            EffectManager.Instance.SpawnHorizontalEffect(pos, Quaternion.identity);

            _gridCellManager.DestroyAt(position);
            using (var breakListPool = ListPool<UniTask>.Get(out List<UniTask> breakTasks))
            {
                using (var boosterListPool = ListPool<IBallPlayBoosterEffect>.Get(out var boosterEffects))
                {
                    _gridCellManager.GetRow(position, out List<IGridCell> row);

                    for (int i = 0; i < row.Count; i++)
                    {
                        if (row[i] == null)
                            continue;

                        if (position == row[i].GridPosition)
                            continue;

                        if (!row[i].ContainsBall)
                            continue;

                        if (row[i].BallEntity is IBallBooster ballBooster)
                            if (ballBooster.IsIgnored)
                                continue;

                        if (row[i].BallEntity is IBallPlayBoosterEffect boosterEffect)
                        {
                            boosterEffects.Add(boosterEffect);
                            await boosterEffect.PlayBoosterEffect(EntityType.WaterBall);
                        }

                        breakTasks.Add(_breakGridTask.Break(row[i]));
                    }

                    await UniTask.WhenAll(breakTasks);
                    for (int i = 0; i < boosterEffects.Count; i++)
                        boosterEffects[i].ReleaseEffect();
                }
            }
        }

        public void Dispose()
        {
            
        }
    }
}
