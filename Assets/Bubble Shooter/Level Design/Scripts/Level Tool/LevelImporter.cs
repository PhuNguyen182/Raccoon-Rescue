using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.LevelDesign.Scripts.Databases;
using BubbleShooter.Scripts.Gameplay.Models;

namespace BubbleShooter.LevelDesign.Scripts.LevelTool
{
    public class LevelImporter
    {
        private readonly TileDatabase _tileDatabase;

        public LevelImporter(TileDatabase tileDatabase)
        {
            _tileDatabase = tileDatabase;
        }

        public LevelImporter BuildBoardMapPosition(Tilemap tilemap, List<BoardMapPosition> mapPositions)
        {
            tilemap.ClearAllTiles();

            for (int i = 0; i < mapPositions.Count; i++)
            {
                var mapPosition = mapPositions[i];
                var tile = _tileDatabase.GetBoardTile();
                tilemap.SetTile(mapPosition.Position, tile);
            }

            return this;
        }

        public LevelImporter BuildBoardThresholdMapPosition(Tilemap tilemap, List<BoardThresholdMapPosition> mapPositions)
        {
            tilemap.ClearAllTiles();

            for (int i = 0; i < mapPositions.Count; i++)
            {
                var mapPosition = mapPositions[i];
                var tile = _tileDatabase.GetBoardThresholdTile();
                tilemap.SetTile(mapPosition.Position, tile);
            }

            return this;
        }

        public LevelImporter BuildColorProportion(List<ColorMapData> colorMapDatas, out List<ColorProportion> colorProportions)
        {
            List<ColorProportion> colors = new();
            for (int i = 0; i < colorMapDatas.Count; i++)
            {
                colors.Add(colorMapDatas[i].ColorProportion);
            }

            colorProportions = colors;
            return this;
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
            Debug.Log("Import Successfully!!!");
        }
    }
}
