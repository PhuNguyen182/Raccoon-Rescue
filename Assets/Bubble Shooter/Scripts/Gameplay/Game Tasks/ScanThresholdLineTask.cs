using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.Scripts.Utils.Comparers;
using BubbleShooter.Scripts.Common.PlayDatas;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class ScanThresholdLineTask : IDisposable
    {
        private readonly GridCellManager _gridCellManager;
        private readonly ThresholdComparer _thresholdComparer;

        private List<ThresholdData> _thresholdDatas;

        public ScanThresholdLineTask(GridCellManager gridCellManager)
        {
            _gridCellManager = gridCellManager;

            _thresholdDatas = new();
            _thresholdComparer = new();
        }

        public void ScanLines()
        {
            for (int i = _thresholdDatas.Count - 1; i >= 0; i--)
            {
                bool isEmptyChecked = _thresholdDatas[i].IsEmptyChecked;
                bool isLineEmpty = CheckEmptyLine(_thresholdDatas[i].Position);
                
                if (!isEmptyChecked && isLineEmpty)
                {
                    // To do: execute move up/down logic
                    var threshold = _thresholdDatas[i];
                    threshold.IsEmptyChecked = true;
                    _thresholdDatas[i] = threshold;

                    // To do: Calculate offset between center and the current threashold
                }
            }
        }

        public void AddThreshold(ThresholdData data)
        {
            _thresholdDatas.Add(data);
        }

        public void Sort()
        {
            _thresholdDatas.Sort(_thresholdComparer);
        }

        private bool CheckEmptyLine(Vector3Int pointInLine)
        {
            List<IGridCell> line;
            _gridCellManager.GetRow(pointInLine, out line);

            for (int i = 0; i < line.Count; i++)
            {
                if (line[i].ContainsBall)
                    return false;
            }

            return true;
        }

        public void Dispose()
        {
            _thresholdDatas.Clear();
        }
    }
}
