using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.LevelDesign.Scripts.CustomTiles;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.Scripts.Gameplay.GameDatas;
using BubbleShooter.Scripts.Utils.BoundsUtils;
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
                        HP = 1
                    }
                });
            }

            return this;
        }

        public void Export(string level)
        {
            string levelPath = $"Assets/Bubble Shooter/Resources/Level Datas/{level}.txt";
            string json = JsonConvert.SerializeObject(_levelModel, Formatting.None);

            using StreamWriter writer = new StreamWriter(levelPath);
            writer.Write(json);
            writer.Close();

#if UNITY_EDITOR
            AssetDatabase.ImportAsset(levelPath);
            Debug.Log(json);
#endif
        }
    }
}
