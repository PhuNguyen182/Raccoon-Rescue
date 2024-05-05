using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.Gameplay.Strategies;
using System.Linq;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class BoardThresholdCheckTask
    {
        private readonly GridCellManager _gridCellManager;
        private readonly MetaBallManager _metaBallManager;
        private readonly CameraController _cameraController;
        private readonly BoundsInt _levelBounds;

        private const float StopHeight = 5.465f;
        private const float UnitHeight = 0.5625f;

        private float _toCeilHeight = 0;
        private Vector3Int _sampleCeilPosition;

        public BoardThresholdCheckTask(GridCellManager gridCellManager, MetaBallManager metaBallManager, CameraController cameraController)
        {
            _gridCellManager = gridCellManager;
            _metaBallManager = metaBallManager;
            _cameraController = cameraController;
            _levelBounds = _gridCellManager.LevelBounds;

            _toCeilHeight = CalculateBottomItemDistanceOnStart();
        }

        public void Check()
        {
            bool isLineEmpty;
            Vector3Int bottomPosition = new Vector3Int(0, _levelBounds.yMin);
            isLineEmpty = CheckEmptyLine(bottomPosition);

            while(!isLineEmpty)
            {
                bottomPosition = bottomPosition + new Vector3Int(0, 1);
                isLineEmpty = CheckEmptyLine(bottomPosition);
            }

            float distance = GetBottomItemDistance(bottomPosition);

            if (distance != _toCeilHeight)
            {
                float offset = Mathf.Abs(distance - _toCeilHeight);
                // This will ensure the accuracy in calculation in order to prevent floating problem
                int rowCount = Mathf.RoundToInt(offset / UnitHeight);

                _toCeilHeight = distance;
                float cameraHeight = GetCameraHeighDistance();
                float moveDistance = rowCount * UnitHeight;

                // If the ceil is higher than the top of screen, move it
                if (Mathf.Abs(cameraHeight - StopHeight) > 0.01f)
                {
                    Vector3 toPosition = Vector3.zero;

                    if (distance > _toCeilHeight) // Move up
                        toPosition = _cameraController.transform.position + moveDistance * Vector3.up;

                    else if (distance < _toCeilHeight) // Move down
                        toPosition = _cameraController.transform.position + moveDistance * Vector3.down;

                    _cameraController.MoveTo(toPosition);
                }
            }
        }

        public void SetCeilHeight(Vector3Int position)
        {
            _sampleCeilPosition = position;
        }

        private bool CheckEmptyLine(Vector3Int pointInLine)
        {
            List<IGridCell> line;
            _gridCellManager.GetRow(pointInLine, out line);

            for (int i = 0; i < line.Count; i++)
            {
                if (line[i] != null && line[i].ContainsBall)
                    return false;
            }

            return true;
        }

        private float GetCameraHeighDistance()
        {
            Vector3 sampleCeilPosition = _gridCellManager.ConvertGridToWorldFunction.Invoke(_sampleCeilPosition);
            float height = sampleCeilPosition.y - _cameraController.transform.position.y;
            return height;
        }

        // This function take any point in the line to calculate only height
        private float GetBottomItemDistance(Vector3Int position)
        {
            Vector3 bottomItemPosition = _gridCellManager.ConvertGridToWorldFunction.Invoke(position);
            Vector3 sampleCeilPosition = _gridCellManager.ConvertGridToWorldFunction.Invoke(_sampleCeilPosition);
            float height = sampleCeilPosition.y - _cameraController.transform.position.y;
            return height;
        }

        private float CalculateBottomItemDistanceOnStart()
        {
            var itemPositions = _metaBallManager.Iterator();
            Vector3Int sampleBottomPos = itemPositions.First();
            float height = GetBottomItemDistance(sampleBottomPos);
            return height;
        }
    }
}
