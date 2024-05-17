using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Utils.BoundsUtils;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks
{
    public class FireBallBoosterTask : IDisposable, IBoosterTask
    {
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;

        public FireBallBoosterTask(GridCellManager gridCellManager, BreakGridTask breakGridTask)
        {
            _gridCellManager = gridCellManager;
            _breakGridTask = breakGridTask;
        }

        public async UniTask Execute(Vector3Int position)
        {
            IGridCell boosterCell = _gridCellManager.Get(position);

            if (boosterCell.BallEntity is IBallBooster booster)
                await booster.Activate();
                
            _gridCellManager.DestroyAt(position);
            using (var breakListPool = ListPool<UniTask>.Get(out List<UniTask> breakTasks))
            {
                using (var effectlistPool = ListPool<IBallPlayBoosterEffect>.Get(out var boosterEffects))
                {
                    var gridPositions = GetBoosterRange(position);

                    foreach (Vector3Int gridPosition in gridPositions)
                    {
                        if (position == gridPosition)
                            continue;

                        IGridCell cell = _gridCellManager.Get(gridPosition);
                        if (cell == null)
                            continue;

                        if (!cell.ContainsBall)
                            continue;

                        if (cell.BallEntity is IBallBooster ballBooster)
                            if (ballBooster.IsIgnored)
                                continue;

                        if (cell.BallEntity is IBallPlayBoosterEffect boosterEffect)
                        {
                            boosterEffects.Add(boosterEffect);
                            await boosterEffect.PlayBoosterEffect(EntityType.FireBall);
                        }

                        breakTasks.Add(_breakGridTask.Break(cell));
                    }

                    await UniTask.WhenAll(breakTasks);
                    for (int i = 0; i < boosterEffects.Count; i++)
                        boosterEffects[i].ReleaseEffect();
                }
            }
        }

        private IEnumerable<Vector3Int> GetBoosterRange(Vector3Int position)
        {
            BoundsInt boosterRange = position.GetBounds2D(5);
            return boosterRange.IteratorIgnoreCorner();
        }

        public void Dispose()
        {

        }
    }
}
