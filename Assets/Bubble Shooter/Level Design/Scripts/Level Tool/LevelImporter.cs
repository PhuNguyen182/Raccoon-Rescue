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

        public LevelImporter BuildBoardThresholdMap(Tilemap tilemap, List<BoardThresholdMapPosition> mapPositions)
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

        public LevelImporter BuildMoveSequence(List<int> sequence, out List<EntityType> moveSequence)
        {
            List<EntityType> seq = new();
            
            for (int i = 0; i < sequence.Count; i++)
            {
                seq.Add((EntityType)sequence[i]);
            }

            moveSequence = seq;
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
