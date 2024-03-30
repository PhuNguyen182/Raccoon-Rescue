using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.LevelDesign.Scripts.Databases;
using BubbleShooter.LevelDesign.Scripts.CustomTiles;

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
            tilemap.ClearAllTiles();

            for (int i = 0; i < mapPositions.Count; i++)
            {
                var mapPosition = mapPositions[i];
                var tile = _tileDatabase.FindBallTile(mapPosition.MapData.ID, mapPosition.MapData.EntityType);
                tilemap.SetTile(mapPosition.Position, tile);
            }

            return this;
        }

        public void Import()
        {

        }
    }
}
