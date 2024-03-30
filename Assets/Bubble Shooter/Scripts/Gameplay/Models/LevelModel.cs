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
        public List<EntityMapPosition> StartingEntityMap = new();

        public void ClearData()
        {
            StartingEntityMap.Clear();
        }
    }
}
