using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;
using BubbleShooter.Scripts.Mainhome.GameManagers;
using Scripts.Configs;

namespace Scripts.App
{
    public static class ApplicationStart
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            RegisterServicesBeforeSceneLoad();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad()
        {
            GameSetup();
            RegisterServicesAfterSceneLoad();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void OnBeforeSplashScene()
        {
            RegisterServicesBeforeSplashScene();
        }

        private static void RegisterServicesBeforeSceneLoad() { }

        private static void RegisterServicesBeforeSplashScene() { }

        private static void RegisterServicesAfterSceneLoad()
        {
            Register<AppInitializer>("App/App Initializer");
            Register<UpdateHandlerManager>("Handlers/Update Behaviour Handler");
            Register<MusicManager>("Managers/Music Manager");
            Register<GameManager>("Managers/Game Manager");
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
            DataManager.LoadData();
        }
    }
}
