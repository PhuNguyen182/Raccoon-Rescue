using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Scripts.SceneUtils;
using TMPro;

namespace BubbleShooter.Scripts.Scenes.LoadingScene
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] private AudioClip mainhomeMusic;
        [SerializeField] private TMP_Text threeDots;

        private int _number = -1;
        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            PlayThreeDots().Forget();
            DelayPlayBackground().Forget();
            LoadMainhome().Forget();
        }

        private async UniTask DelayPlayBackground()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _token);
            MusicManager.Instance.SetBackGroundMusic(mainhomeMusic, true, 0.6f);
        }

        private async UniTask PlayThreeDots()
        {
            while (true)
            {
                _number = _number + 1;
                if (_number > 3)
                    _number = 0;

                if (_number == 0)
                    threeDots.text = "";

                else if (_number == 1)
                    threeDots.text = ".";

                else if (_number == 2)
                    threeDots.text = "..";

                else if (_number == 3)
                    threeDots.text = "...";

                await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: _token);
                if (_token.IsCancellationRequested)
                    return;
            }
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
