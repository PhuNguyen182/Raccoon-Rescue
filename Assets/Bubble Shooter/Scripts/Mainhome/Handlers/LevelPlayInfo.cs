using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using BubbleShooter.Scripts.Common.Databases;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Mainhome.Handlers
{
    public class LevelPlayInfo
    {
        private LevelStreakData _levelStreakData;

        public LevelPlayInfo(LevelStreakData levelStreakData)
        {
            _levelStreakData = levelStreakData;
        }

        public async UniTask<string> GetLevelData(int level)
        {
            string levelData;

            if (level > 0 && level <= 100)
                levelData = await GetLocalLevelData(level);
            
            else
                levelData = await GetRemoteLevelData(level);

            return levelData;
        }

        private async UniTask<string> GetLocalLevelData(int level)
        {
            TextAsset textAsset = await Resources.LoadAsync<TextAsset>($"Level Datas/level_{level}") as TextAsset;
            return textAsset.text;
        }

        private async UniTask<string> GetRemoteLevelData(int level)
        {
            for (int i = 0; i < _levelStreakData.LevelRanges.Count; i++)
            {
                if (ComparableUtils.IsInRange(level, _levelStreakData.LevelRanges[i]))
                {
                    int minRange = _levelStreakData.LevelRanges[i].MinValue;
                    int maxRange = _levelStreakData.LevelRanges[i].MaxValue;

                    string path = $"LevelData{minRange}_{maxRange}/level_{level}";
                    TextAsset levelText = await Addressables.LoadAssetAsync<TextAsset>(path);
                    return levelText != null ? levelText.text : null;
                }
            }

            return null;
        }
    }
}
