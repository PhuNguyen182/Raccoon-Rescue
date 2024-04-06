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

        public void Add(Vector3Int position, IBallEntity ballEntity)
        {
            if(_metaBalls.ContainsKey(position))
            {
                _metaBalls[position] = ballEntity;
                return;
            }

            _metaBalls.Add(position, ballEntity);
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

        public IEnumerable<Vector3Int> Iterator()
        {
            foreach (Vector3Int position in _metaBalls.Keys)
            {
                yield return position;
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
