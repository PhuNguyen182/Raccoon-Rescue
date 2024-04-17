using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Utils.BoundsUtils;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class GridCellManager : IDisposable
    {
        private BoundsInt _gridBounds;
        private List<Vector3Int> _gridPosition;
        private Dictionary<Vector3Int, IGridCell> _gridCells;
        private Dictionary<Vector3Int, bool> _visitedMatrix;

        private OrderablePartitioner<Vector3Int> _partitioner;

        public List<Vector3Int> GridPositions => _gridPosition;
        public Func<Vector3Int, Vector3> ConvertGridToWorldFunction { get; }
        public Func<Vector3, Vector3Int> ConvertWorldToGridFunction { get; }

        public GridCellManager(Func<Vector3Int, Vector3> gridToWorld, Func<Vector3, Vector3Int> worldToGrid)
        {
            _gridCells = new();
            ConvertGridToWorldFunction = gridToWorld;
            ConvertWorldToGridFunction = worldToGrid;
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
            List<IGridCell> gridCells = new();
            var rowPositions = _gridBounds.GetRow(position);
            
            foreach (Vector3Int rowPosition in rowPositions)
            {
                IGridCell cell = Get(rowPosition);
                gridCells.Add(cell);
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
            }
        }

        public bool DestroyAt(Vector3Int position)
        {
            IGridCell gridCell = Get(position);
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
            _gridBounds = BoundsExtension.Encapsulate(_gridPosition);
            _partitioner = Partitioner.Create(_gridPosition, true);

            _visitedMatrix = new Dictionary<Vector3Int, bool>();
            for (int i = 0; i < _gridPosition.Count; i++)
            {
                _visitedMatrix.Add(_gridPosition[i], false);
            }
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
