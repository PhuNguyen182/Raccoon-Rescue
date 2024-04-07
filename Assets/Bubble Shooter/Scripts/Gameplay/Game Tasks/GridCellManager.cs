using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Utils.BoundsUtils;
using UnityEngine.Pool;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class GridCellManager : IDisposable
    {
        private BoundsInt _gridBounds;
        private Dictionary<Vector3Int, IGridCell> _gridCells;

        private readonly int[] _xNeighbours;
        private readonly int[] _yNeighbours;

        private bool[,] _visitedMatrix;

        public bool[,] VisitedMatrix => _visitedMatrix;
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

            if (gridCell != null)
            {
                gridCell.SetBall(null);
                return gridCell.Destroy();
            }

            return false;
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
            List<Vector3Int> positions = _gridCells.Keys.ToList();
            _gridBounds = BoundsExtension.Encapsulate(positions);
            _visitedMatrix = new bool[_gridBounds.size.x, _gridBounds.size.y];
        }

        public void ClearVisitedMatrix()
        {
            for (int i = 0; i < _visitedMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < _visitedMatrix.GetLength(1); j++)
                {
                    _visitedMatrix[i, j] = false;
                }
            }
        }

        public void ClearAll()
        {
            _gridCells?.Clear();
        }

        public void Dispose()
        {
            ClearAll();
        }
    }
}
