using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using Scripts.Configs;

namespace Scripts.App
{
    public static class ApplicationStart
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            GameSetup();
            RegisterServicesBeforeSceneLoad();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad()
        {
            RegisterServicesAfterSceneLoad();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void OnBeforeSplashScene()
        {
            RegisterServicesBeforeSplashScene();
        }

        private static void RegisterServicesBeforeSplashScene()
        {

        }

        private static void RegisterServicesBeforeSceneLoad()
        {
            Register<AppInitializer>("App/App Initializer");
            Register<UpdateHandlerManager>("Handlers/Update Behaviour Handler");
        }

        private static void RegisterServicesAfterSceneLoad()
        {

        }

        private static T Register<T>(string serviceName) where T : Component
        {
            T service = Resources.Load<T>(serviceName);
            T instance = Object.Instantiate(service);
            Object.DontDestroyOnLoad(instance);
            return service;
        }

        private static void GameSetup()
        {
            Application.targetFrameRate = GameSetupConstants.NormalTargetFramerate;
        }
    }
}
