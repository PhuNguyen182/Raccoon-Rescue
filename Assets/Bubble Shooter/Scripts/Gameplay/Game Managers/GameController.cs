using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.GameBoard;
using BubbleShooter.Scripts.Gameplay.Strategies;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Gameplay.GameTasks.IngameBoosterTasks;
using BubbleShooter.Scripts.Gameplay.GameTasks;
using BubbleShooter.Scripts.Common.Factories;
using BubbleShooter.Scripts.Common.Databases;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.GameUI.Screens;
using BubbleShooter.Scripts.Gameplay.Inputs;
using BubbleShooter.Scripts.Common.Configs;
using Scripts.Configs;
using Scripts.Service;

namespace BubbleShooter.Scripts.Gameplay.GameManagers
{
    public class GameController : MonoBehaviour, IService
    {
        [Header("Game Handler")]
        [SerializeField] private BallShooter ballShooter;
        [SerializeField] private BallProvider ballProvider;
        [SerializeField] private InputController inputHandler;
        [SerializeField] private GridCellHolder gridPrefab;
        [SerializeField] private EntityDatabase entityDatabase;

        [Header("Containers")]
        [SerializeField] private Transform entityContainer;
        [SerializeField] private Transform gridCellContainer;

        [Header("Tilemaps")]
        [SerializeField] private Tilemap boardTilemap;

        [Header("GUI")]
        [SerializeField] private MainScreenManager mainScreen;

        [Header("Tutorials")]
        [SerializeField] private GameplayTutorialManager tutorialManager;

        [Header("Miscs")]
        [SerializeField] private GameDecorator gameDecorator;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private Material ballMaterial;

        private InputProcessor _inputProcessor;
        private EntityFactory _entityFactory;
        private TargetFactory _targetFactory;
        private EntityManager _entityManager;
        private GridCellManager _gridCellManager;
        private MetaBallManager _metaBallManager;
        private FillBoardTask _fillBoardTask;
        private CheckTargetTask _checkTargetTask;
        private CheckScoreTask _checkScoreTask;
        private IngameBoosterHandler _ingameBoosterHandler;
        private MoveGameViewTask _moveGameViewTask;
        private StartingGameTask _startingGameTask;
        private GameTaskManager _gameTaskManager;

        public GameDecorator GameDecorator => gameDecorator;
        public MainScreenManager MainScreenManager => mainScreen;
        public static GameController Instance { get; private set; }

        private void Awake()
        {
            Setup();
            Initialize();
        }

        private void Start()
        {
            CreateLevel();
            ballShooter.PreloadBalls(entityContainer);
        }

        private void Setup()
        {
            Instance = this;
            Application.targetFrameRate = GameSetupConstants.MaxTargetFramerate;
        }

        public void Initialize()
        {
            DisposableBuilder builder = Disposable.CreateBuilder();

            _inputProcessor = new(inputHandler);
            _entityFactory = new(entityDatabase, entityContainer);
            _targetFactory = new(entityDatabase, entityContainer);
            ballShooter.SetBallFactory(_entityFactory);

            _entityManager = new(_targetFactory, _entityFactory);
            _metaBallManager = new(_entityManager);
            _metaBallManager.AddTo(ref builder);
            
            _checkTargetTask = new(mainScreen.InGamePanel);
            _checkTargetTask.AddTo(ref builder);
            ballShooter.SetCheckTargetTask(_checkTargetTask);

            _checkScoreTask = new(mainScreen.InGamePanel);
            _checkScoreTask.AddTo(ref builder);

            _gridCellManager = new(ConvertGridPositionToWolrdPosition
                                   , ConvertWorldPositionToGridPosition
                                   , _metaBallManager);
            _gridCellManager.AddTo(ref builder);

            _entityFactory.SetGridCellManager(_gridCellManager);
            _fillBoardTask = new(_gridCellManager, _metaBallManager);

            _moveGameViewTask = new(_gridCellManager, cameraController, _inputProcessor
                                    , mainScreen.NotificationPanel, _checkTargetTask);
            _moveGameViewTask.AddTo(ref builder);

            _startingGameTask = new(_inputProcessor, _moveGameViewTask, tutorialManager);
            _startingGameTask.AddTo(ref builder);

            _ingameBoosterHandler = new(mainScreen.BoosterPanel, ballProvider, ballShooter, _inputProcessor);
            _ingameBoosterHandler.AddTo(ref builder);

            _gameTaskManager = new(_gridCellManager, mainScreen, _inputProcessor, _checkTargetTask, _checkScoreTask, ballProvider
                                   , ballShooter,_metaBallManager, gameDecorator, _moveGameViewTask, _ingameBoosterHandler);
            _gameTaskManager.AddTo(ref builder);

            _gameTaskManager.SetBallMaterialEndGame(ballMaterial);
            builder.RegisterTo(this.destroyCancellationToken);
        }

        private void CreateLevel()
        {
            if (PlayConfig.Current == null)
                return;

            if (PlayConfig.Current.LevelModel == null)
                return;

            gameDecorator.SetBackground(PlayConfig.Current.Level);
            LevelModel levelModel = PlayConfig.Current.LevelModel;
            mainScreen.EndGameScreen.CompletePanel.SetLevel(PlayConfig.Current.Level);
            GenerateLevel(levelModel, PlayConfig.Current.Level, PlayConfig.Current.IsTest);

            if (PlayConfig.Current.IsTest)
            {
                _ingameBoosterHandler.InitBooster(new()
                {
                    new IngameBoosterModel() { BoosterType = IngameBoosterType.Colorful, Amount = 1000 },
                    new IngameBoosterModel() { BoosterType = IngameBoosterType.PreciseAimer, Amount = 1000 },
                    new IngameBoosterModel() { BoosterType = IngameBoosterType.ChangeBall, Amount = 1000 },
                });
            }
            else
            {
                _ingameBoosterHandler.InitBooster(new()
                {
                    new IngameBoosterModel() 
                    { 
                        BoosterType = IngameBoosterType.Colorful, 
                        Amount = GameData.Instance.GetBooster(IngameBoosterType.Colorful)
                    },
                    new IngameBoosterModel() 
                    { 
                        BoosterType = IngameBoosterType.PreciseAimer, 
                        Amount = GameData.Instance.GetBooster(IngameBoosterType.PreciseAimer)
                    },
                    new IngameBoosterModel() 
                    { 
                        BoosterType = IngameBoosterType.ChangeBall, 
                        Amount = GameData.Instance.GetBooster(IngameBoosterType.ChangeBall)
                    },
                });
            }
        }

        private void GenerateLevel(LevelModel levelModel, int level, bool isTest)
        {
            GenerateGridCell(levelModel);
            GenerateEntities(levelModel);

            _checkScoreTask.SetScores(levelModel);
            _checkTargetTask.SetTargetCount(levelModel);

            SetShootSequence(levelModel);
            _fillBoardTask.Fill();

            if (!isTest)
            {
                _startingGameTask.OnStartGame(level).Forget();
            }

            else
            {
                _moveGameViewTask.CalculateFirstItemHeight();
                _moveGameViewTask.MoveViewOnStart().Forget();
            }
        }

        private void GenerateGridCell(LevelModel levelModel)
        {
            for (int i = 0; i < levelModel.BoardMapPositions.Count; i++)
            {
                GridCell gridCell;
                Vector3Int gridPosition = levelModel.BoardMapPositions[i].Position;
                Vector3 worldPosition = ConvertGridPositionToWolrdPosition(gridPosition);
                var gridHolder = Instantiate(gridPrefab, worldPosition, Quaternion.identity, gridCellContainer);
                gridHolder.gameObject.name = $"Grid Cell: ({gridPosition.x}, {gridPosition.y})";
                gridHolder.GridPosition = gridPosition;

                gridCell = new();
                gridCell.GridPosition = gridPosition;
                _gridCellManager.Add(gridCell);
            }
        }

        private void GenerateEntities(LevelModel levelModel)
        {
            SetTopCeilPosition(levelModel.CeilMapPositions[0].Position);
            _moveGameViewTask.SetCeilHeight(levelModel.CeilMapPositions[0].Position);

            for (int i = 0; i < levelModel.CeilMapPositions.Count; i++)
            {
                var gridCell = _gridCellManager.Get(levelModel.CeilMapPositions[i].Position);
                gridCell.IsCeil = true;
            }

            for (int i = 0; i < levelModel.BoardBottomMapPositions.Count; i++)
            {
                var gridCell = _gridCellManager.Get(levelModel.BoardBottomMapPositions[i].Position);
                gridCell.IsBottom = true;
            }

            _metaBallManager.SetColorStrategy(levelModel.ColorMapDatas);
            for (int i = 0; i < levelModel.StartingEntityMap.Count; i++)
            {
                var data = levelModel.StartingEntityMap[i].MapData;
                if (data.EntityType == EntityType.Random)
                {
                    _metaBallManager.AddRandomMapPosition(levelModel.StartingEntityMap[i]);
                    continue;
                }

                _metaBallManager.AddEntity(levelModel.StartingEntityMap[i]);
            }

            for (int i = 0; i < levelModel.TargetMapPositions.Count; i++)
            {
                _metaBallManager.AddTarget(levelModel.TargetMapPositions[i]);
            }
        }

        public void SetInputActive(bool active)
        {
            _gameTaskManager.SetInputActive(active);
        }

        public Vector3 ConvertGridPositionToWolrdPosition(Vector3Int position)
        {
            return boardTilemap.GetCellCenterWorld(position);
        }

        public Vector3Int ConvertWorldPositionToGridPosition(Vector3 position)
        {
            return boardTilemap.WorldToCell(position);
        }

        public IGridCell GetCell(Vector3Int position)
        {
            return _gridCellManager.Get(position);
        }

        public void AddEntity(IBallEntity ballEntity)
        {
            _metaBallManager.AddEntity(ballEntity);
        }

        private void SetShootSequence(LevelModel levelModel)
        {
            ballProvider.SetMoveCount(levelModel.ColorMapDatas);
        }

        private void SetTopCeilPosition(Vector3Int position)
        {
            Vector3 pos = ConvertGridPositionToWolrdPosition(position);
            Vector3 ceilPosition = new Vector3(0, pos.y + 0.425f);
            gameDecorator.SetTopCeilPosition(ceilPosition);
        }
    }
}
