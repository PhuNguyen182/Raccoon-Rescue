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
        }

        private async UniTask LoadMainhomeScene()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _token);
            transition.SetTrigger(_fadeHash);
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: _token);
            await SceneLoader.LoadScene(SceneConstants.Mainhome, LoadSceneMode.Single);
        }

        private async UniTask LoadGameplayScene()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _token);
            transition.SetTrigger(_fadeHash);
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: _token);
            await SceneLoader.LoadScene(SceneConstants.Gameplay, LoadSceneMode.Single);
        }
    }
}
