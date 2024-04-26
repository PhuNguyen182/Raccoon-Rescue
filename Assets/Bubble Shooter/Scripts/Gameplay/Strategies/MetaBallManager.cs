using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.Scripts.Common.Interfaces;

namespace BubbleShooter.Scripts.Gameplay.Strategies
{
    public class MetaBallManager : IDisposable
    {
        private readonly EntityManager _entityManager;

        private List<ColorMapData> _colorStrategy;
        private List<EntityMapPosition> _randomEntityFill;
        private Dictionary<Vector3Int, IBallEntity> _metaBalls;

        public MetaBallManager(EntityManager entityManager)
        {
            _entityManager = entityManager;

            _metaBalls = new();
            _randomEntityFill = new();
        }

        public void AddEntity(IBallEntity ballEntity)
        {
            if (!_metaBalls.ContainsKey(ballEntity.GridPosition))
                _metaBalls.Add(ballEntity.GridPosition, ballEntity);

            else 
                _metaBalls[ballEntity.GridPosition] = ballEntity;
        }

        public IBallEntity AddEntity(EntityMapPosition mapPosition)
        {
            IBallEntity ballEntity = _entityManager.SpawnEntity(mapPosition);

            if (!_metaBalls.ContainsKey(mapPosition.Position))
            {
                _metaBalls.Add(mapPosition.Position, ballEntity);
                return ballEntity;
            }

            _metaBalls[mapPosition.Position] = ballEntity;
            return _metaBalls[mapPosition.Position];
        }

        public IBallEntity AddTarget(TargetMapPosition mapPosition)
        {
            IBallEntity ballEntity = _entityManager.SpawnTarget(mapPosition);

            if (!_metaBalls.ContainsKey(mapPosition.Position))
            {
                _metaBalls.Add(mapPosition.Position, ballEntity);
                return ballEntity;
            }

            _metaBalls[mapPosition.Position] = ballEntity;
            return _metaBalls[mapPosition.Position];
        }

        public void AddRandomMapPosition(EntityMapPosition mapPosition)
        {
            _randomEntityFill.Add(mapPosition);
        }

        public void RemoveEntity(Vector3Int position)
        {
            if (_metaBalls.ContainsKey(position))
            {
                _metaBalls.Remove(position);
            }
        }

        public void SetColorStrategy(List<ColorMapData> colors)
        {
            _colorStrategy = colors;
        }

        public List<ColorMapData> GetColorStrategy() 
        { 
            return _colorStrategy;
        }
        
        public List<EntityMapPosition> GetRandomEntityFill()
        {
            return _randomEntityFill;;
        }

        public IBallEntity GetEntity(Vector3Int position)
        {
            return _metaBalls.TryGetValue(position, out var ballEntity) ? ballEntity : null;
        }

        public IEnumerable<Vector3Int> Iterator()
        {
            foreach (Vector3Int position in _metaBalls.Keys)
            {
                yield return position;
            }
        }

        public IEnumerable<IBallEntity> GetEntities()
        {
            foreach (Vector3Int position in _metaBalls.Keys)
            {
                yield return _metaBalls[position];
            }
        }

        public List<IBallEntity> GetFixedEntities()
        {
            List<IBallEntity> ballEntities = new();

            foreach (var item in _metaBalls)
            {
                if (item.Value != null && !item.Value.IsFallen)
                    ballEntities.Add(item.Value);
            }

            return ballEntities;
        }

        public void Clear()
        {
            _metaBalls.Clear();
            _randomEntityFill.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
