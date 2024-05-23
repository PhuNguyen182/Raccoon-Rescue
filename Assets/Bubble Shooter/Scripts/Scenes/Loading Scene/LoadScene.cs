using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Scripts.SceneUtils;

namespace BubbleShooter.Scripts.Scenes.LoadingScene
{
    public class LoadScene : MonoBehaviour
    {
        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        private void Start()
        {
            LoadMainhome().Forget();
        }

        private async UniTask LoadMainhome()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2.5f), cancellationToken: _token);
            await SceneLoader.LoadScene(SceneConstants.Mainhome, LoadSceneMode.Single);
        }

        private async UniTask LoadGameplay()
        {
            // To do: init a play config with specific tutorial if possible
            await UniTask.Delay(TimeSpan.FromSeconds(2.5f), cancellationToken: _token);
            await SceneLoader.LoadScene(SceneConstants.Gameplay, LoadSceneMode.Single);
        }
    }
}
