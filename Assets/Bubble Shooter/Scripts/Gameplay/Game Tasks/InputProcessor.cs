using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.GameBoard;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class InputProcessor : IDisposable
    {
        private readonly InputHandler _inputHandler;
        private readonly GridCellManager _gridCellManager;
        private readonly int _gridCellLayer = 0;

        private const string GridCellLayerName = "Grid";

        private IGridCell _currentGridCell;
        private Collider2D _gridCollider;

        public InputProcessor(InputHandler inputHandler, GridCellManager gridCellManager)
        {
            _inputHandler = inputHandler;
            _gridCellManager = gridCellManager;

            _inputHandler.OnClicked += OnPress;
            _inputHandler.OnRelease += OnRelease;

            _gridCellLayer = LayerMask.NameToLayer(GridCellLayerName);
        }

        private void OnPress()
        {
            _gridCollider = Physics2D.OverlapPoint(_inputHandler.MousePosition, _gridCellLayer);
            
            if(_gridCollider != null)
            {
                if(_gridCollider.TryGetComponent<GridCellHolder>(out var holder))
                {
                    _currentGridCell = _gridCellManager.Get(holder.GridPosition);
                }
            }
        }

        private void OnRelease()
        {
            if (_currentGridCell == null)
                return;

            IBallEntity ball = _currentGridCell.BallEntity;
            
            if (ball is IBallAnimation ballAnimation)
                ballAnimation.PlayBounceAnimation();

            if (ball is IBallEffect ballEffect)
                ballEffect.PlayBlastEffect();

            _currentGridCell = null;
            _gridCollider = null;
        }

        public void Dispose()
        {
            _inputHandler.OnClicked -= OnPress;
            _inputHandler.OnRelease -= OnRelease;
        }
    }
}
