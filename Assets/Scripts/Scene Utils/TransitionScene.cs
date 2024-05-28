using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BubbleShooter.Scripts.Common.Configs;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace Scripts.SceneUtils
{
    public class TransitionScene : MonoBehaviour
    {
        [SerializeField] private AudioClip mainhomeMusic;
        [SerializeField] private AudioClip gameplayMusic;
        [SerializeField] private Animator transition;

        private static readonly int _fadeHash = Animator.StringToHash("Fade");

        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        private void Start()
        {
            LoadScene().Forget();
        }

        private async UniTask LoadScene()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _token);
            transition.SetTrigger(_fadeHash);

            if (TransitionConfig.Current != null)
            {
                SceneName sceneName = TransitionConfig.Current.SceneName;
                switch (sceneName)
                {
                    case SceneName.Mainhome:
                        await LoadMainhomeScene();
                        break;
                    case SceneName.Gameplay:
                        await LoadGameplayScene();
                        break;
                }
            }

            TransitionConfig.Current = null;
        }

        private async UniTask LoadMainhomeScene()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _token);
            await SceneLoader.LoadScene(SceneConstants.Mainhome, LoadSceneMode.Single).ContinueWith(() =>
            {
                MusicManager.Instance.SetBackGroundMusic(mainhomeMusic, true, 0.6f);
            });
        }

        private async UniTask LoadGameplayScene()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _token);
            await SceneLoader.LoadScene(SceneConstants.Gameplay, LoadSceneMode.Single).ContinueWith(() =>
            {
                MusicManager.Instance.SetBackGroundMusic(gameplayMusic, true, 0.6f);
            });
        }
    }
}
