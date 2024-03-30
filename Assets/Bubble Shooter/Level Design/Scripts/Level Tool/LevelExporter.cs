using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BubbleShooter.Scripts.Gameplay.Models;
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

        public LevelExporter BuildBallMap(Tilemap ballMapData)
        {
            

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
