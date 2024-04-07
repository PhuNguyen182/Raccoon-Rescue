using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Utils.BoundsUtils;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class GridCellManager : IDisposable
    {
        private BoundsInt _gridBounds;
        private List<Vector3Int> _gridPosition;
        private Dictionary<Vector3Int, IGridCell> _gridCells;
        private Dictionary<Vector3Int, bool> _visitedMatrix;

        private readonly int[] _xNeighbours;
        private readonly int[] _yNeighbours;

        public Func<Vector3Int, Vector3> ConvertPositionFunction { get; }

        public GridCellManager(Func<Vector3Int, Vector3> convertFunction)
        {
            _gridCells = new();
            ConvertPositionFunction = convertFunction;

            _xNeighbours = new int[] { 0, 1, 0, -1, -1, -1 };
            _yNeighbours = new int[] { 1, 0, -1, -1, 0, 1 };
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

        public void GetColumn(Vector3Int position, out List<IGridCell> column)
        {
            List<IGridCell> gridCells = new();
            var rowPositions = _gridBounds.GetColumn(position);

            foreach (Vector3Int rowPosition in rowPositions)
            {
                IGridCell cell = Get(rowPosition);
                gridCells.Add(cell);
            }

            column = gridCells;
        }

        public void Add(IGridCell gridCell)
        {
            if(gridCell != null)
            {
                gridCell.WorldPosition = ConvertPositionFunction.Invoke(gridCell.GridPosition);
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
                _gridCells[position] = null;
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

            for (int i = 0; i < 6; i++)
            {
                gridCells.Add(Get(checkPosition + new Vector3Int(_xNeighbours[i], _yNeighbours[i])));
            }

            return gridCells;
        }

        public void Encapsulate()
        {
            _gridPosition = _gridCells.Keys.ToList();
            _gridBounds = BoundsExtension.Encapsulate(_gridPosition);
            
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
            for (int i = 0; i < _gridPosition.Count; i++)
            {
                _visitedMatrix[_gridPosition[i]] = false;
            }
        }

        public void ClearAll()
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
