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
        public List<BoardThresholdMapPosition> BoardThresholdMapPositions = new();
        public List<EntityMapPosition> StartingEntityMap = new();

        public void ClearData()
        {
            BoardMapPositions.Clear();
            BoardThresholdMapPositions.Clear();
            StartingEntityMap.Clear();
        }
    }
}
