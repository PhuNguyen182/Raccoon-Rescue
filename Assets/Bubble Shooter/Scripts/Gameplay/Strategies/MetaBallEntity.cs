using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.Strategies
{
    public class MetaBallEntity : IBallEntity
    {
        public bool IsMatchable { get; }

        public bool IsFixedOnStart { get; set; }

        public Vector3 WorldPosition { get; }

        public Vector3Int GridPosition { get; set; }

        public EntityType EntityType { get; }
        public bool IsCeilAttached { get; set; }

        public UniTask Blast()
        {
            return UniTask.CompletedTask;
        }

        public void CheckCeilAttach()
        {
            
        }

        public void DestroyEntity()
        {
            
        }

        public void OnSnapped()
        {
            
        }

        public void SetWorldPosition(Vector3 position)
        {
            
        }
    }
}
