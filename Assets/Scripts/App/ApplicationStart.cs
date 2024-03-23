using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RaccoonRescue.Scripts.Gameplay.Common.UpdateHandlerPattern;

namespace Scripts.App
{
    public static class ApplicationStart
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {

        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad()
        {
            RegisterServices();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void OnBeforeSplashScene()
        {

        }

        private static void RegisterServices()
        {
            Register<UpdateHandlerManager>("Handlers/Update Behaviour Handler");
        }

        private static T Register<T>(string serviceName) where T : Component
        {
            T service = Resources.Load<T>(serviceName);
            T instance = Object.Instantiate(service);
            Object.DontDestroyOnLoad(instance);
            return service;
        }
    }
}
