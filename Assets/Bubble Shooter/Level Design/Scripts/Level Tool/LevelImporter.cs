using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.LevelDesign.Scripts.Databases;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.LevelDesign.Scripts.LevelTool
{
    public class LevelImporter
    {
        private readonly TileDatabase _tileDatabase;

        public LevelImporter(TileDatabase tileDatabase)
        {
            _tileDatabase = tileDatabase;
        }

        public LevelImporter BuildTarget(int targets, out int targetCount)
        {
            targetCount = targets;
            return this;
        }

        public LevelImporter BuildScore(int max, int tier1, int tier2, int tier3, out int maxScore, out int score1, out int score2, out int score3)
        {
            score1 = tier1;
            score2 = tier2;
            score3 = tier3;
            maxScore = max;
            return this;
        }

        public LevelImporter BuildBoardMap(Tilemap tilemap, List<BoardMapPosition> mapPositions)
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

        public LevelImporter BuildBoardBottomMap(Tilemap tilemap, List<BoardBottomMapPosition> mapPositions)
        {
            tilemap.ClearAllTiles();

            for (int i = 0; i < mapPositions.Count; i++)
            {
                var mapPosition = mapPositions[i];
                var tile = _tileDatabase.GetBoardBottomTile();
                tilemap.SetTile(mapPosition.Position, tile);
            }

            return this;
        }

        public LevelImporter BuildCeilMap(Tilemap tilemap, List<CeilMapPosition> mapPositions)
        {
            tilemap.ClearAllTiles();

            for (int i = 0; i < mapPositions.Count; i++)
            {
                var mapPosition = mapPositions[i];
                var tile = _tileDatabase.GetCeilTile();
                tilemap.SetTile(mapPosition.Position, tile);
            }

            return this;
        }

        public LevelImporter BuildMoveSequence(int moveCount, out int moveCounts)
        {
            moveCounts = moveCount;
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

        public LevelImporter BuildBallMap(Tilemap tilemap, List<EntityMapPosition> mapPositions)
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

        public LevelImporter BuildEntityMap(Tilemap tilemap, List<EntityMapPosition> mapPositions)
        {
            // Use same tilemap with ball tilemap so son't clear this tilemap
            for (int i = 0; i < mapPositions.Count; i++)
            {
                var mapPosition = mapPositions[i];
                var tile = _tileDatabase.FindEntityTile(mapPosition.MapData.ID
                                                        , mapPosition.MapData.HP
                                                        , mapPosition.MapData.EntityType);
                if(tile != null)
                    tilemap.SetTile(mapPosition.Position, tile);
            }

            return this;
        }

        public LevelImporter BuildTargetMap(Tilemap tilemap, List<TargetMapPosition> mapPositions)
        {
            // Use same tilemap with ball tilemap so son't clear this tilemap
            for (int i = 0; i < mapPositions.Count; i++)
            {
                var mapPosition = mapPositions[i];
                var tile = _tileDatabase.FindTargetTile(mapPosition.MapData.ID
                                                        , mapPosition.MapData.Color
                                                        , mapPosition.MapData.TargetColor);
                if(tile != null)
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
