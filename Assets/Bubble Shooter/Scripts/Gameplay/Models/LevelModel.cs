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

        public List<ColorMapData> ColorMapDatas = new();
        public List<BoardMapPosition> BoardMapPositions = new();
        public List<BoardBottomMapPosition> BoardBottomMapPositions = new();
        public List<TargetMapPosition> TargetMapPositions = new();
        public List<EntityMapPosition> StartingEntityMap = new();
        public List<CeilMapPosition> CeilMapPositions = new();

        public void ClearData()
        {
            MoveCount = 0;
            TargetCount = 0;
            TierOneScore = 0;
            TierTwoScore = 0;
            TierThreeScore = 0;
            MaxScore = 0;

            ColorMapDatas.Clear();
            BoardMapPositions.Clear();
            BoardBottomMapPositions.Clear();
            TargetMapPositions.Clear();
            StartingEntityMap.Clear();
            CeilMapPositions.Clear();
        }
    }
}
