using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameBoard;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Gameplay.GameTasks;
using BubbleShooter.Scripts.Common.Factories;
using BubbleShooter.Scripts.Common.Databases;
using Scripts.Service;
using R3;

namespace BubbleShooter.Scripts.Gameplay.GameManagers
{
    public class GameController : MonoBehaviour, IService
    {
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private Transform entityContainer;
        [SerializeField] private GridCellHolder gridPrefab;
        [SerializeField] private EntityDatabase entityDatabase;

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
            
            _gridCellManager = new();
            _gridCellManager.AddTo(ref d);
            _entityFactory = new(entityDatabase, entityContainer);

            _gameTaskManager = new(_gridCellManager, inputHandler);
            _gameTaskManager.AddTo(ref d);

            _disposable = d.Build();
        }

        private void GenerateLevel()
        {

        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}
