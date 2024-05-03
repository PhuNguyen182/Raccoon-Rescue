using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.Strategies;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class GridCellManager : IDisposable
    {
        private readonly MetaBallManager _metaBallManager;

        private List<Vector3Int> _gridPosition;
        private Dictionary<Vector3Int, IGridCell> _gridCells;
        private Dictionary<Vector3Int, bool> _visitedMatrix;
        private OrderablePartitioner<Vector3Int> _partitioner;
        private BoundsInt _levelBounds;

        public BoundsInt LevelBounds => _levelBounds;
        public List<Vector3Int> GridPositions => _gridPosition;
        public Func<Vector3Int, Vector3> ConvertGridToWorldFunction { get; }
        public Func<Vector3, Vector3Int> ConvertWorldToGridFunction { get; }

        public GridCellManager(Func<Vector3Int, Vector3> gridToWorld, Func<Vector3, Vector3Int> worldToGrid, MetaBallManager metaBallManager)
        {
            _gridCells = new();
            ConvertGridToWorldFunction = gridToWorld;
            ConvertWorldToGridFunction = worldToGrid;
            _metaBallManager = metaBallManager;
        }

        public Dictionary<Vector3Int, IGridCell> GetBoardGridCells()
        {
            return _gridCells;
        }

        public IGridCell Get(Vector3Int position)
        {
            return _gridCells.TryGetValue(position, out IGridCell gridCell) ? gridCell : null;
        }

        public void GetRow(Vector3Int position, out List<IGridCell> row)
        {
            int count = 1;
            IGridCell gridCell;
            List<IGridCell> gridCells = new();

            gridCell = Get(position);
            gridCells.Add(gridCell);

            while(gridCell != null)
            {
                count = count + 1;
                Vector3Int checkPos = position + new Vector3Int(-1, 0) * count;
                gridCell = Get(checkPos);

                if (gridCell != null)
                    gridCells.Add(gridCell);
            }

            count = 0;
            gridCell = Get(position);

            while (gridCell != null)
            {
                count = count + 1;
                Vector3Int checkPos = position + new Vector3Int(1, 0) * count;
                gridCell = Get(checkPos);
                
                if(gridCell != null)
                    gridCells.Add(gridCell);
            }

            row = gridCells;
        }

        public void Add(IGridCell gridCell)
        {
            if(gridCell != null)
            {
                gridCell.WorldPosition = ConvertGridToWorldFunction.Invoke(gridCell.GridPosition);
            }

            if(_gridCells.ContainsKey(gridCell.GridPosition))
            {
                _gridCells[gridCell.GridPosition] = gridCell;
                return;
            }

            _gridCells.Add(gridCell.GridPosition, gridCell);
        }

        public void Remove(Vector3Int position)
        {
            if (_gridCells.ContainsKey(position))
            {
                _gridCells[position].SetBall(null);
                _metaBallManager.RemoveEntity(position);
            }
        }

        public bool DestroyAt(Vector3Int position)
        {
            IGridCell gridCell = Get(position);
            _metaBallManager.RemoveEntity(position);
            return gridCell != null ? gridCell.Destroy() : false;
        }

        public List<IGridCell> GetNeighbourGrids(Vector3Int checkPosition)
        {
            List<IGridCell> gridCells = new();

            for (int i = 0; i < CommonProperties.MaxNeighborCount; i++)
            {
                Vector3Int neighborOffset = checkPosition.y % 2 == 0 
                                            ? CommonProperties.EvenYNeighborOffsets[i] 
                                            : CommonProperties.OddYNeighborOffsets[i];
                gridCells.Add(Get(checkPosition + neighborOffset));
            }

            return gridCells;
        }

        public void Encapsulate()
        {
            _gridPosition = _gridCells.Keys.ToList();
            _partitioner = Partitioner.Create(_gridPosition, true);

            _visitedMatrix = new Dictionary<Vector3Int, bool>();
            for (int i = 0; i < _gridPosition.Count; i++)
            {
                _visitedMatrix.Add(_gridPosition[i], false);
            }

            _levelBounds = new BoundsInt
            {
                min = _gridPosition[0],
                max = _gridPosition[_gridPosition.Count - 1]
            };
        }

        public bool GetIsVisited(Vector3Int position)
        {
            return _visitedMatrix[position];
        }

        public void SetAsVisited(Vector3Int position)
        {
            _visitedMatrix[position] = true;
        }

        public void ClearVisitedPositions()
        {
            // It's okay when use paralel loop because it doesn't mean running in the main thread
            Parallel.ForEach(_partitioner, gridPosition =>
            {
                _visitedMatrix[gridPosition] = false;
            });
        }

        private void ClearAll()
        {
            _gridCells?.Clear();
            _gridPosition?.Clear();
            _visitedMatrix?.Clear();
        }

        public void Dispose()
        {
            ClearAll();
        }
    }
}
