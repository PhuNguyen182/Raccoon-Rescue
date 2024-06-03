using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Configs;
using BubbleShooter.Scripts.Gameplay.Models;
using Cysharp.Threading.Tasks;
using Scripts.SceneUtils;
using Newtonsoft.Json;

namespace BubbleShooter.LevelDesign.Scripts.LevelTool
{
    public class PlayTestLevel : MonoBehaviour
    {
        [SerializeField] private LevelBuilder levelBuilder;

        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        private void Start()
        {
            PlayTest().Forget();
        }

        private async UniTask PlayTest()
        {
            levelBuilder.Export(0, false);
            string levelJson = levelBuilder.ExportLevelData;
            using (StringReader streamReader = new(levelJson))
            {
                using (JsonReader jsonReader = new JsonTextReader(streamReader))
                {
                    JsonSerializer jsonSerializer = new();
                    LevelModel levelModel = jsonSerializer.Deserialize<LevelModel>(jsonReader);

                    PlayConfig.Current = new PlayConfig
                    {
                        IsTest = true,
                        LevelModel = levelModel
                    };
                }
        }

            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _token);
            await SceneLoader.LoadScene(SceneConstants.Gameplay);
        }
    }
}
