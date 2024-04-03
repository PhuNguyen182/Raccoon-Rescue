using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BubbleShooter.LevelDesign.Scripts.Databases;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.Scripts.Gameplay.Models;
using Sirenix.OdinInspector;
using Newtonsoft.Json;

namespace BubbleShooter.LevelDesign.Scripts.LevelTool
{
    public class LevelBuilder : MonoBehaviour
    {
        [Header("Builder Tilemaps")]
        [SerializeField] private Tilemap boardTilemap;
        [SerializeField] private Tilemap boardThresholdTilemap;
        [SerializeField] private Tilemap entityTilemap;
        [SerializeField] private TileDatabase tileDatabase;

        [Title("Level Data")]
        [SerializeField] private string inputLevel;
        [SerializeField] private string outputLevel;

        [Header("Color Proportion")]
        [SerializeField] private List<ColorProportion> colorProportions;

        private LevelImporter _levelImporter;
        private LevelExporter _levelExporter = new();

        [Button]
        public void Clear()
        {
            colorProportions.Clear();
            boardTilemap.ClearAllTiles();
            boardThresholdTilemap.ClearAllTiles();
            entityTilemap.ClearAllTiles();
        }

        [Button]
        public void CompressTilemaps()
        {
            boardTilemap.CompressBounds();
            boardThresholdTilemap.CompressBounds();
            entityTilemap.CompressBounds();
        }

        [HorizontalGroup(GroupID = "Level Builder")]
        [Button(Style = ButtonStyle.Box)]
        public void Export(int level, bool useResource = true)
        {
            CompressTilemaps();

            string output = _levelExporter.Clear()
                                          .BuildBoardMap(boardTilemap)
                                          .BuildBoardThresholdMap(boardThresholdTilemap)
                                          .BuildBallMap(entityTilemap)
                                          .BuildColorProportion(colorProportions)
                                          .BuildEntityMap(entityTilemap)
                                          .Export($"level_{level}", useResource);
            
            if (!useResource)
            {
                outputLevel = output;
                Debug.Log(outputLevel);
            }
        }

        [HorizontalGroup(GroupID = "Level Builder")]
        [Button(Style = ButtonStyle.Box)]
        public void Import(int level, bool useResource = true)
        {
            string levelData = useResource ? Resources.Load<TextAsset>($"Level Datas/level_{level}").text 
                                           : inputLevel;
            
            if (string.IsNullOrEmpty(levelData))
            {
                Debug.LogError("Invalid input level data!!!");
                return;
            }

            LevelModel levelModel = JsonConvert.DeserializeObject<LevelModel>(levelData);

            _levelImporter = new(tileDatabase);
            _levelImporter.BuildBoardMapPosition(boardTilemap, levelModel.BoardMapPositions)
                          .BuildBoardThresholdMapPosition(boardThresholdTilemap, levelModel.BoardThresholdMapPositions)
                          .BuildColorProportion(levelModel.ColorMapDatas, out colorProportions)
                          .BuildBallMapPosition(entityTilemap, levelModel.StartingEntityMap)
                          .Import();
        }
    }
}
