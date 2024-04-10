using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;

namespace BubbleShooter.Scripts.Gameplay.Models
{
    [Serializable]
    public class LevelModel
    {
        public List<int> MoveSequence = new();
        public List<BoardMapPosition> BoardMapPositions = new();
        public List<CeilMapPosition> CeilMapPositions = new();
        public List<BoardThresholdMapPosition> BoardThresholdMapPositions = new(); // To be tested
        public List<ColorMapData> ColorMapDatas = new(); // To be tested
        public List<EntityMapPosition> StartingEntityMap = new();
        public List<TargetMapPosition> TargetMapPositions = new();

        public void ClearData()
        {
            MoveSequence.Clear();
            BoardMapPositions.Clear();
            CeilMapPositions.Clear();
            BoardThresholdMapPositions.Clear();
            ColorMapDatas.Clear();
            StartingEntityMap.Clear();
            TargetMapPositions.Clear();
        }
    }
}
