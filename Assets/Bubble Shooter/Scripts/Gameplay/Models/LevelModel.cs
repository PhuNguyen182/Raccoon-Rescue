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
        public List<BoardMapPosition> BoardMapPositions = new();
        public List<BoardThresholdMapPosition> BoardThresholdMapPositions = new(); // To be tested
        public List<ColorMapData> ColorMapDatas = new(); // To be tested
        public List<EntityMapPosition> StartingEntityMap = new();

        public void ClearData()
        {
            BoardMapPositions.Clear();
            BoardThresholdMapPositions.Clear();
            ColorMapDatas.Clear();
            StartingEntityMap.Clear();
        }
    }
}
