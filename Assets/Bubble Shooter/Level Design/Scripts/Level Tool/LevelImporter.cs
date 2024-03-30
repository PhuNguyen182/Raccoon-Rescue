using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.LevelDesign.Scripts.Databases;

namespace BubbleShooter.LevelDesign.Scripts.LevelTool
{
    public class LevelImporter
    {
        private readonly TileDatabase _tileDatabase;

        public LevelImporter(TileDatabase tileDatabase)
        {
            _tileDatabase = tileDatabase;
        }

        public LevelImporter BuildBallMapPosition(Tilemap tilemap, List<EntityMapPosition> mapPositions)
        {
            return this;
        }

        public void Import()
        {

        }
    }
}
