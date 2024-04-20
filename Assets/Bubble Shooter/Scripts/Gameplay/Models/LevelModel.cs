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
        public int MoveCount = 0;
        public int TargetCount = 0;
        public int TierOneScore = 0;
        public int TierTwoScore = 0;
        public int TierThreeScore = 0;
        public int MaxScore = 0;

        public List<BoardMapPosition> BoardMapPositions = new();
        public List<CeilMapPosition> CeilMapPositions = new();
        public List<BoardThresholdMapPosition> BoardThresholdMapPositions = new(); // To be tested
        public List<ColorMapData> ColorMapDatas = new(); // To be tested
        public List<EntityMapPosition> StartingEntityMap = new();
        public List<TargetMapPosition> TargetMapPositions = new();

        public void ClearData()
        {
            MoveCount = 0;
            TargetCount = 0;
            TierOneScore = 0;
            TierTwoScore = 0;
            TierThreeScore = 0;
            MaxScore = 0;

            BoardMapPositions.Clear();
            CeilMapPositions.Clear();
            BoardThresholdMapPositions.Clear();
            ColorMapDatas.Clear();
            StartingEntityMap.Clear();
            TargetMapPositions.Clear();
        }
    }
}
