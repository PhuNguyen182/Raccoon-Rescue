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
        [SerializeField] private Tilemap ceilTilemap;
        [SerializeField] private Tilemap entityTilemap;
        [SerializeField] private TileDatabase tileDatabase;

        [Title("Level Data")]
        [SerializeField] private string inputLevel;
        [SerializeField] private string outputLevel;
        [SerializeField] private string externalBuildPath;

        [Space(10)]
        [SerializeField] private int targetCount;
        [SerializeField] private int moveCount;

        [Header("Scores")]
        [SerializeField] private int maxScore;
        [SerializeField] private int tierOneScore;
        [SerializeField] private int tierTwoScore;
        [SerializeField] private int tierThreeScore;

        [Header("Random Fill Color Proportion")]
        [SerializeField] private List<ColorProportion> colorProportions;

        private LevelImporter _levelImporter;
        private LevelExporter _levelExporter = new();

        public string ExportLevelData => outputLevel;

        [Button]
        public void Clear()
        {
            targetCount = 0;
            tierOneScore = 0;
            tierTwoScore = 0;
            tierThreeScore = 0;

            colorProportions.Clear();
            boardTilemap.ClearAllTiles();
            ceilTilemap.ClearAllTiles();
            entityTilemap.ClearAllTiles();
        }

        [Button]
        public void CompressTilemaps()
        {
            boardTilemap.CompressBounds();
            entityTilemap.CompressBounds();
        }

        [HorizontalGroup(GroupID = "Level Builder")]
        [Button(Style = ButtonStyle.Box)]
        public void Export(int level, bool useResource = true)
        {
            CompressTilemaps();

            string output = _levelExporter.Clear()
                                          .BuildTarget(targetCount)
                                          .BuildMoveSequence(moveCount)
                                          .BuildScores(maxScore, tierOneScore, tierTwoScore, tierThreeScore)
                                          .BuildBoardMap(boardTilemap)
                                          .BuildCeilMap(ceilTilemap)
                                          .BuildBallMap(entityTilemap)
                                          .BuildColorProportion(colorProportions)
                                          .BuildEntityMap(entityTilemap)
                                          .BuildTargetMap(entityTilemap)
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
            _levelImporter.BuildTarget(levelModel.TargetCount, out targetCount)
                          .BuildScore(levelModel.MaxScore, levelModel.TierOneScore, levelModel.TierTwoScore, levelModel.TierThreeScore,
                                      out maxScore, out tierOneScore, out tierTwoScore, out tierThreeScore)
                          .BuildBoardMap(boardTilemap, levelModel.BoardMapPositions)
                          .BuildCeilMap(ceilTilemap, levelModel.CeilMapPositions)
                          .BuildBallMap(entityTilemap, levelModel.StartingEntityMap)
                          .BuildColorProportion(levelModel.ColorMapDatas, out colorProportions)
                          .BuildMoveSequence(levelModel.MoveCount, out moveCount)
                          .BuildEntityMap(entityTilemap, levelModel.StartingEntityMap)
                          .BuildTargetMap(entityTilemap, levelModel.TargetMapPositions)
                          .Import();
        }
    }
}
