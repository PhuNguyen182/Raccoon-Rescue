using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Scripts.SceneUtils
{
    public class SceneLoader
    {
        public static async UniTask LoadScene(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            AsyncOperation sceneOperator = SceneManager.LoadSceneAsync(sceneName, loadMode);
            await sceneOperator;
        }

        public static async UniTask LoadScene(string sceneName, IProgress<float> progress, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            AsyncOperation sceneOperator = SceneManager.LoadSceneAsync(sceneName, loadMode);
            await sceneOperator.ToUniTask(progress);
        }
    }
}
