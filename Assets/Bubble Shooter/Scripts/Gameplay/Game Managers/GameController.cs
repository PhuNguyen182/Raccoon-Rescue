using R3;
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
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.Gameplay.Strategies;
using Newtonsoft.Json;
using Scripts.Configs;
using Scripts.Service;

namespace BubbleShooter.Scripts.Gameplay.GameManagers
{
    public class GameController : MonoBehaviour, IService
    {
        [Header("Game Handler")]
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private GridCellHolder gridPrefab;
        [SerializeField] private EntityDatabase entityDatabase;

        [Header("Containers")]
        [SerializeField] private Transform entityContainer;
        [SerializeField] private Transform gridCellContainer;

        [Header("Tilemaps")]
        [SerializeField] private Tilemap boardTilemap;

        private EntityFactory _entityFactory;
        private TargetFactory _targetFactory;
        private GridCellManager _gridCellManager;
        private MetaBallManager _metaBallManager;
        private FillBoardTask _fillBoardTask;
        private GameTaskManager _gameTaskManager;
        
        private IDisposable _disposable;

        public GridCellManager GridCellManager => _gridCellManager;

        public static GameController Instance { get; private set; }

        private void Awake()
        {
            Setup();
            Initialize();
        }

        private void Start()
        {
            GetLevel();
        }

        private void Setup()
        {
            Instance = this;
            Application.targetFrameRate = GameSetupConstants.MaxTargetFramerate;
        }

        public void Initialize()
        {
            DisposableBuilder builder = Disposable.CreateBuilder();
            
            _gridCellManager = new(ConvertGridPositionToWolrdPosition);
            _gridCellManager.AddTo(ref builder);

            _entityFactory = new(entityDatabase, entityContainer);
            _targetFactory = new(entityDatabase, entityContainer);

            _metaBallManager = new();
            _metaBallManager.AddTo(ref builder);

            _fillBoardTask = new(_gridCellManager, _metaBallManager);

            _gameTaskManager = new(_gridCellManager, inputHandler);
            _gameTaskManager.AddTo(ref builder);

            _disposable = builder.Build();
        }

        private void GetLevel()
        {
            string levelData = Resources.Load<TextAsset>("Level Datas/level_1").text;
            LevelModel levelModel = JsonConvert.DeserializeObject<LevelModel>(levelData);
            GenerateLevel(levelModel);
        }

        private void GenerateLevel(LevelModel levelModel)
        {
            for (int i = 0; i < levelModel.BoardMapPositions.Count; i++)
            {
                GridCell gridCell;
                Vector3Int gridPosition = levelModel.BoardMapPositions[i].Position;
                Vector3 worldPosition = ConvertGridPositionToWolrdPosition(gridPosition);
                var gridHolder = Instantiate(gridPrefab, worldPosition, Quaternion.identity, gridCellContainer);
                gridHolder.GridPosition = gridPosition;
                
                gridCell = new();
                gridCell.GridPosition = gridPosition;
                _gridCellManager.Add(gridCell);
            }

            for (int i = 0; i < levelModel.StartingEntityMap.Count; i++)
            {
                var data = levelModel.StartingEntityMap[i].MapData;
                var ballEntity = _entityFactory.Create(data);
                _metaBallManager.Add(levelModel.StartingEntityMap[i].Position, ballEntity);
            }

            for (int i = 0; i < levelModel.TargetMapPositions.Count; i++)
            {
                var data = levelModel.TargetMapPositions[i].MapData;
                var targetEntity = _targetFactory.Create(data);
                _metaBallManager.Add(levelModel.TargetMapPositions[i].Position, targetEntity);
            }

            _fillBoardTask.Fill();
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
