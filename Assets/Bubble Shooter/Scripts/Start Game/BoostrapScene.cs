using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Scripts.SceneUtils;

namespace BubbleShooter.Scripts.StartGame
{
    public class BoostrapScene : MonoBehaviour
    {
        private CancellationToken _token;

        private void Start()
        {
            _token = this.GetCancellationTokenOnDestroy();
            OnBoostrapScene().Forget();
        }

        private async UniTask OnBoostrapScene()
        {
            DataManager.LoadData();
            await LoadToLoadingScene();
        }

        private async UniTask LoadToLoadingScene()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _token);
            await SceneLoader.LoadScene(SceneConstants.Loading);
        }
    }
}
