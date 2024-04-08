using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using UnityEngine.Pool;

namespace BubbleShooter.Scripts.Gameplay.GameTasks.BoosterTasks
{
    public class SunBallBoosterTask : IDisposable, IBoosterTask
    {
        private readonly GridCellManager _gridCellManager;
        private readonly BreakGridTask _breakGridTask;

        private readonly int[] _xNeighbours;
        private readonly int[] _yNeighbours;
        private HashSet<int> _checkedIndicies;

        public SunBallBoosterTask(GridCellManager gridCellManager, BreakGridTask breakGridTask)
        {
            _gridCellManager = gridCellManager;
            _breakGridTask = breakGridTask;

            _checkedIndicies = new();
            _xNeighbours = new int[] { 0, 1, 0, -1, -1, -1 };
            _yNeighbours = new int[] { 1, 0, -1, -1, 0, 1 };
        }

        // To do: Destroy a triple of ball
        public async UniTask Execute(Vector3Int position)
        {
            IGridCell boosterCell = _gridCellManager.Get(position);
            if (boosterCell.BallEntity is IBallBooster booster)
                await booster.Activate();
            
            using (var listPool = ListPool<UniTask>.Get(out var breakTasks))
            {
                List<Vector3Int> gridPosition = GetTripleBall(position);
                for (int i = 0; i < gridPosition.Count; i++)
                {
                    if (position == gridPosition[i])
                        continue;

                    IGridCell cell = _gridCellManager.Get(gridPosition[i]);
                    if (cell == null)
                        continue;

                    if (!cell.ContainsBall)
                        continue;

                    if (cell.BallEntity is IBallBooster ballBooster)
                        if (ballBooster.IsIgnored)
                            continue;

                    breakTasks.Add(_breakGridTask.Break(cell));
                }

                await UniTask.WhenAll(breakTasks);
            }
        }

        private List<Vector3Int> GetTripleBall(Vector3Int position)
        {
            _checkedIndicies.Clear();
            List<Vector3Int> triplePosition = new();

            while(_checkedIndicies.Count < 3)
            {
                int rand = Random.Range(0, 6);
                if (!_checkedIndicies.Contains(rand))
                {
                    _checkedIndicies.Add(rand);
                    triplePosition.Add(new Vector3Int(_xNeighbours[rand], _yNeighbours[rand]));
                }
            }

            return triplePosition;
        }

        public void Dispose()
        {
            _checkedIndicies.Clear();
        }
    }
}
