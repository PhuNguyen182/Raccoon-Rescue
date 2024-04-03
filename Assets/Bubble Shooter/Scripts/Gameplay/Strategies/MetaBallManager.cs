using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;

namespace BubbleShooter.Scripts.Gameplay.Strategies
{
    public class MetaBallManager : IDisposable
    {
        private Dictionary<Vector3Int, IBallEntity> _metaBalls;

        public MetaBallManager()
        {
            _metaBalls = new();
        }

        public void Add(IBallEntity ballEntity)
        {
            if(!_metaBalls.TryAdd(ballEntity.GridPosition, ballEntity))
            {
                IBallEntity ball = _metaBalls[ballEntity.GridPosition];
                _metaBalls[ballEntity.GridPosition] = ballEntity;
            }

            _metaBalls.Add(ballEntity.GridPosition, ballEntity);
        }

        public IBallEntity Get(Vector3Int position)
        {
            return _metaBalls.TryGetValue(position, out var ballEntity) ? ballEntity : null;
        }

        public void Remove(IBallEntity ballEntity)
        {
            if (_metaBalls.ContainsKey(ballEntity.GridPosition))
            {
                _metaBalls.Remove(ballEntity.GridPosition);
            }
        }

        public void Clear()
        {
            _metaBalls.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
