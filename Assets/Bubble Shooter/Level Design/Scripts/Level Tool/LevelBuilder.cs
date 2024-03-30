using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BubbleShooter.LevelDesign.Scripts.Databases;
using Sirenix.OdinInspector;
using BubbleShooter.Scripts.Gameplay.Models;
using Newtonsoft.Json;

namespace BubbleShooter.LevelDesign.Scripts.LevelTool
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private Tilemap entityTilemap;
        [SerializeField] private TileDatabase tileDatabase;

        private LevelImporter _levelImporter;
        private LevelExporter _levelExporter = new();

        [Button]
        public void Clear()
        {
            entityTilemap.ClearAllTiles();
        }

        [HorizontalGroup(GroupID = "Level Builder")]
        [Button(Style = ButtonStyle.Box)]
        public void Export(int level)
        {
            _levelExporter.Clear()
                          .BuildBallMap(entityTilemap)
                          .BuildEntityMap(entityTilemap)
                          .Export($"level_{level}");
        }

        [HorizontalGroup(GroupID = "Level Builder")]
        [Button(Style = ButtonStyle.Box)]
        public void Import(int level)
        {
            string levelData = Resources.Load<TextAsset>($"Level Datas/level_{level}").text;
            LevelModel levelModel = JsonConvert.DeserializeObject<LevelModel>(levelData);

            _levelImporter = new(tileDatabase);
            _levelImporter.BuildBallMapPosition(entityTilemap, levelModel.StartingEntityMap)
                          .Import();
        }
    }
}
