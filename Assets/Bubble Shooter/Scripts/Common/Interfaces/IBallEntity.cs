using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IBallEntity
    {
        public int Score { get; }
        public int ScoreMultiplier { get; set; }

        public bool IsMatchable { get; }
        public bool IsFallen { get; set; }
        public bool IsEndOfGame { get; set; }
        public bool IsFixedOnStart { get; set; }

        public Vector3 WorldPosition { get; }
        public Vector3Int GridPosition { get; set; }
        public EntityType EntityType { get; }

        public UniTask Blast();
        public void OnSnapped();
        public void DestroyEntity();
        public void PublishScore();
        public void OnFallenDestroy();
        public void SetWorldPosition(Vector3 position);
    }
}
