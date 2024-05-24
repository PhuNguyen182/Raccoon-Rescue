using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.GameUI.Notifications;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class MoveGameViewTask : IDisposable
    {
        private readonly InputProcessor _inputProcessor;
        private readonly GridCellManager _gridCellManager;
        private readonly CameraController _cameraController;
        private readonly NotificationPanel _notificationPanel;
        private readonly CheckTargetTask _checkTargetTask;

        private const float StopHeight = 5.465f;
        private const float UnitHeight = 0.5625f;

        private float _toCeilHeight = 0;
        private BoundsInt _levelBounds;
        private Vector3Int _sampleCeilPosition;

        private CancellationToken _cancellationToken;
        private CancellationTokenSource _tokenSource;

        public MoveGameViewTask(GridCellManager gridCellManager, CameraController cameraController
            , InputProcessor inputProcessor, NotificationPanel notificationPanel, CheckTargetTask checkTargetTask)
        {
            _gridCellManager = gridCellManager;
            _cameraController = cameraController;
            _inputProcessor = inputProcessor;
            _notificationPanel = notificationPanel;
            _checkTargetTask = checkTargetTask;

            _tokenSource = new();
            _cancellationToken = _tokenSource.Token;
        }

        public async UniTask MoveViewOnStart()
        {
            _inputProcessor.IsActive = false;
            Vector3 sampleCeilPosition = _gridCellManager
                                         .ConvertGridToWorldFunction
                                         .Invoke(_sampleCeilPosition);

            // If the ceil is close, don't move the game view
            if (Mathf.Abs(sampleCeilPosition.y - StopHeight) > 0.01)
            {
                _cameraController.SetPosition(new Vector3(0, sampleCeilPosition.y - StopHeight, -10));
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: _cancellationToken);
                await _cameraController.MoveToZero(Vector3.back * 10);
            }

            int targetCount = _checkTargetTask.TargetCount;
            string notice = targetCount > 2 ? $"Rescues {targetCount} baby raccoons!"
                                            : $"Rescues {targetCount} baby raccoon!";

            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: _cancellationToken);
            await _notificationPanel.SetNotificationInfo(notice);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: _cancellationToken);

            _inputProcessor.IsActive = true;
        }

        public async UniTask Check()
        {
            _levelBounds = _gridCellManager.LevelBounds;
            Vector3Int bottomPosition = new Vector3Int(0, _levelBounds.yMin);
            (bool isLineEmpty, bool isLineValid) = CheckEmptyLine(bottomPosition);

            while(isLineEmpty && isLineValid)
            {
                bottomPosition = bottomPosition + new Vector3Int(0, 1);
                (isLineEmpty, isLineValid) = CheckEmptyLine(bottomPosition);

                if (!isLineValid)
                    return;
            }

            float distance = GetBottomItemDistance(bottomPosition);

            if (distance != _toCeilHeight)
            {
                float offset = Mathf.Abs(distance - _toCeilHeight);
                
                if (offset <= 0.01f)
                    return;

                // This will ensure the accuracy in calculation in order to prevent floating problem
                int rowCount = Mathf.RoundToInt(offset / UnitHeight);
                float cameraHeight = GetCameraHeighDistance();
                float moveDistance = rowCount * UnitHeight;
                
                // Prevent camera move down exceedly the top screen
                if (cameraHeight - moveDistance <= StopHeight)
                    moveDistance = cameraHeight - StopHeight;

                // If the ceil is higher than the top of screen, move it
                // Compare 2 float numbers do not use = operator
                if (Mathf.Abs(cameraHeight - StopHeight) > 0.01f)
                {
                    Vector3 toPosition = Vector3.zero;

                    if (distance < _toCeilHeight) // Move up
                        toPosition = _cameraController.transform.position + moveDistance * Vector3.up;

                    else if (distance > _toCeilHeight) // Move down
                        toPosition = _cameraController.transform.position + moveDistance * Vector3.down;

                    if(toPosition != Vector3.zero)
                        await _cameraController.MoveTo(toPosition);
                }

                _toCeilHeight = distance;
            }
        }

        public void CalculateFirstItemHeight()
        {
            _toCeilHeight = CalculateBottomItemDistanceOnStart();
        }

        public void SetCeilHeight(Vector3Int position)
        {
            _sampleCeilPosition = position;
        }

        private (bool, bool) CheckEmptyLine(Vector3Int pointInLine)
        {
            List<IGridCell> line;
            _gridCellManager.GetRow(pointInLine, out line);

            if (line == null)
                return (false, false);

            for (int i = 0; i < line.Count; i++)
            {
                if (line[i] != null && line[i].ContainsBall)
                    return (false, true);
            }

            return (true, true);
        }

        private float GetCameraHeighDistance()
        {
            Vector3 sampleCeilPosition = _gridCellManager.ConvertGridToWorldFunction.Invoke(_sampleCeilPosition);
            float height = sampleCeilPosition.y - _cameraController.transform.position.y;
            return height;
        }

        // This function take any point in the primaryLine to calculate only height
        private float GetBottomItemDistance(Vector3Int position)
        {
            Vector3 bottomItemPosition = _gridCellManager.ConvertGridToWorldFunction.Invoke(position);
            Vector3 sampleCeilPosition = _gridCellManager.ConvertGridToWorldFunction.Invoke(_sampleCeilPosition);
            float height = sampleCeilPosition.y - bottomItemPosition.y;
            return height;
        }

        private float CalculateBottomItemDistanceOnStart()
        {
            Vector3Int bottomPosition = new Vector3Int(0, _levelBounds.yMin);
            (bool isLineEmpty, bool isLineValid) = CheckEmptyLine(bottomPosition);

            while (isLineEmpty && isLineValid)
            {
                bottomPosition = bottomPosition + new Vector3Int(0, 1);
                (isLineEmpty, isLineValid) = CheckEmptyLine(bottomPosition);
            }

            float height = GetBottomItemDistance(bottomPosition);
            return height;
        }

        public void Dispose()
        {
            _tokenSource.Dispose();
        }
    }
}
