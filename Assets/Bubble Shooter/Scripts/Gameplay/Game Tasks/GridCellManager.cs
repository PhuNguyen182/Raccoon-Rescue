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
        private Dictionary<Vector3Int, IGridCell> _gridCells;

        public Func<Vector3Int, Vector3> ConvertPositionFunction { get; }

        public GridCellManager(Func<Vector3Int, Vector3> convertFunction)
        {
            _gridCells = new();
            ConvertPositionFunction = convertFunction;
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
            if (!_gridCells.ContainsKey(gridCell.GridPosition))
            {
                _gridCells.Add(gridCell.GridPosition, gridCell);
            }
        }

        public void Remove(Vector3Int position)
        {
            if (_gridCells.ContainsKey(position))
            {
                _gridCells.Remove(position);
            }
        }

        public bool DestroyAt(Vector3Int position)
        {
            IGridCell gridCell = Get(position);

            if (gridCell != null)
                return gridCell.Destroy();

            return false;
        }

        public List<IGridCell> GetCrossNeighbors(Vector3Int position)
        {
            List<IGridCell> neighbors = new();

            IGridCell up = Get(position + Vector3Int.up);
            IGridCell down = Get(position + Vector3Int.down);
            IGridCell left = Get(position + Vector3Int.left);
            IGridCell right = Get(position + Vector3Int.right);

            if (up != null && up.BallEntity != null) neighbors.Add(up);
            if (down != null && down.BallEntity != null) neighbors.Add(down);
            if (left != null && left.BallEntity != null) neighbors.Add(left);
            if (right != null && right.BallEntity != null) neighbors.Add(right);

            return neighbors;
        }

        public void Encapsulate()
        {
            List<Vector3Int> positions = _gridCells.Keys.ToList();
            _gridBounds = BoundsExtension.Encapsulate(positions);
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
