using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.LevelDesign.Scripts.BoardTiles;
using BubbleShooter.LevelDesign.Scripts.CustomTiles;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.Scripts.Gameplay.GameDatas;
using BubbleShooter.Scripts.Utils.BoundsUtils;
using BubbleShooter.Scripts.Common.Enums;
using Newtonsoft.Json;
using UnityEditor;

namespace BubbleShooter.LevelDesign.Scripts.LevelTool
{
    public class LevelExporter
    {
        private readonly LevelModel _levelModel = new();

        public LevelExporter Clear()
        {
            _levelModel.ClearData();
            return this;
        }

        public LevelExporter BuildBoardMap(Tilemap tilemap)
        {
            var positions = tilemap.cellBounds.Iterator();
            foreach (Vector3Int position in positions)
            {
                var tile = tilemap.GetTile<BoardTile>(position);

                if (tile == null)
                    continue;

                _levelModel.BoardMapPositions.Add(new BoardMapPosition
                {
                    Position = position
                });
            }

            return this;
        }

        public LevelExporter BuildBoardThresholdMap(Tilemap tilemap)
        {
            var positions = tilemap.cellBounds.Iterator();
            foreach (Vector3Int position in positions)
            {
                var tile = tilemap.GetTile<BoardThresholdTile>(position);

                if (tile == null)
                    continue;

                _levelModel.BoardThresholdMapPositions.Add(new BoardThresholdMapPosition
                {
                    Position = position
                });
            }

            return this;
        }

        public LevelExporter BuildColorProportion(List<ColorProportion> colorProportions)
        {
            for (int i = 0; i < colorProportions.Count; i++)
            {
                _levelModel.ColorMapDatas.Add(new ColorMapData
                {
                    ColorProportion = colorProportions[i]
                });
            }

            return this;
        }

        public LevelExporter BuildMoveSequence(List<EntityType> sequence)
        {
            for (int i = 0; i < sequence.Count; i++)
            {
                _levelModel.MoveSequence.Add((int)sequence[i]);
            }

            return this;
        }

        public LevelExporter BuildBallMap(Tilemap tilemap)
        {
            var positions = tilemap.cellBounds.Iterator();
            foreach (Vector3Int position in positions)
            {
                var tile = tilemap.GetTile<BallTile>(position);
                
                if (tile == null)
                    continue;

                _levelModel.StartingEntityMap.Add(new EntityMapPosition
                {
                    Position = position,
                    MapData = new EntityMapData
                    {
                        ID = tile.ID,
                        EntityType = tile.EntityType,
                        HP = 1
                    }
                });
            }

            return this;
        }

        public LevelExporter BuildEntityMap(Tilemap tilemap)
        {
            var positions = tilemap.cellBounds.Iterator();
            foreach (Vector3Int position in positions)
            {
                var tile = tilemap.GetTile<EntityTile>(position);

                if (tile == null)
                    continue;

                _levelModel.StartingEntityMap.Add(new EntityMapPosition
                {
                    Position = position,
                    MapData = new EntityMapData
                    {
                        ID = tile.ID,
                        EntityType = tile.EntityType,
                        HP = tile.HP
                    }
                });
            }

            return this;
        }

        public LevelExporter BuildTargetMap(Tilemap tilemap)
        {
            var positions = tilemap.cellBounds.Iterator();
            foreach (Vector3Int position in positions)
            {
                var tile = tilemap.GetTile<TargetTile>(position);

                if (tile == null)
                    continue;

                _levelModel.TargetMapPositions.Add(new TargetMapPosition
                {
                    Position = position,
                    MapData = new TargetMapData
                    {
                        ID = tile.ID,
                        Color = tile.EntityType,
                        TargetColor = tile.TargetType
                    }
                });
            }

            return this;
        }

        public string Export(string level, bool useResource = true)
        {
            string levelPath = $"Assets/Bubble Shooter/Resources/Level Datas/{level}.txt";
            string json = JsonConvert.SerializeObject(_levelModel, Formatting.None);

            if (useResource)
            {
                using StreamWriter writer = new StreamWriter(levelPath);
                writer.Write(json);
                writer.Close();

#if UNITY_EDITOR
                AssetDatabase.ImportAsset(levelPath);
#endif
            }

            Debug.Log(useResource ? json : "Get level data at output placehold!");
            return json;
        }
    }
}
