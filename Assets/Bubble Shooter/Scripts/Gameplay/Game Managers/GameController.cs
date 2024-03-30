using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BubbleShooter.Scripts.Gameplay.GameBoard;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Gameplay.GameTasks;
using BubbleShooter.Scripts.Common.Factories;
using BubbleShooter.Scripts.Common.Databases;
using Scripts.Service;
using R3;
using BubbleShooter.Scripts.Gameplay.Models;

namespace BubbleShooter.Scripts.Gameplay.GameManagers
{
    public class GameController : MonoBehaviour, IService
    {
        [Header("Game Handler")]
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private Transform entityContainer;
        [SerializeField] private GridCellHolder gridPrefab;
        [SerializeField] private EntityDatabase entityDatabase;

        [Header("Tilemaps")]
        [SerializeField] private Tilemap boardTilemap;

        private EntityFactory _entityFactory;
        private GridCellManager _gridCellManager;
        private GameTaskManager _gameTaskManager;
        
        private IDisposable _disposable;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            var d = Disposable.CreateBuilder();
            
            _gridCellManager = new(ConvertGridPositionToWolrdPosition);
            _gridCellManager.AddTo(ref d);
            _entityFactory = new(entityDatabase, entityContainer);

            _gameTaskManager = new(_gridCellManager, inputHandler);
            _gameTaskManager.AddTo(ref d);

            _disposable = d.Build();
        }

        private void GenerateLevel(LevelModel levelModel)
        {

        }

        private Vector3 ConvertGridPositionToWolrdPosition(Vector3Int position)
        {
            return boardTilemap.GetCellCenterWorld(position);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}
