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

        public Func<Vector3Int, Vector3> ConvertPositionFunction { get; }

        public GridCellManager(Func<Vector3Int, Vector3> convertFunction)
        {
            _gridCells = new();
            ConvertPositionFunction = convertFunction;

            _xNeighbours = new int[] { 0, 1, 0, -1, -1, -1 };
            _yNeighbours = new int[] { 1, 0, -1, -1, 0, 1 };
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

            if(!_gridCells.TryAdd(gridCell.GridPosition, gridCell))
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
                return gridCell.Destroy();

            return false;
        }

        public bool CheckNeighbours(Vector3Int checkPosition, out List<Vector3Int> neighbourPositions)
        {
            // Check 6 neighbour cells
            List<Vector3Int> positions;

            IGridCell checkCell = Get(checkPosition);
            if(checkCell.BallEntity == null)
            {
                neighbourPositions = null;
                return false;
            }

            using (var listPool = ListPool<Vector3Int>.Get(out positions))
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector3Int checkPos = new(_xNeighbours[i], _yNeighbours[i]);
                    IGridCell gridCell = Get(checkPos);

                    if (gridCell.BallEntity == null)
                        continue;

                    if (gridCell.BallEntity.EntityType != checkCell.BallEntity.EntityType)
                        continue;

                    if (!gridCell.BallEntity.IsMatchable)
                        continue;

                    positions.Add(checkPos);
                }

                if (positions.Count > 0)
                {
                    neighbourPositions = positions;
                    return true;
                }
            }

            neighbourPositions = null;
            return false;
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
