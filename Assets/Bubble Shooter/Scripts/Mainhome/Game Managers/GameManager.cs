using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Mainhome.GameManagers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private HeartTimeManager heartManager;

        public HeartTimeManager HeartTime => heartManager;

        private void Start()
        {
            heartManager.LoadHeartOnStart();
        }

        private void Update()
        {
            heartManager.UpdateHeartTime();
            Delete();
        }

        private void OnDestroy()
        {
            DataManager.SaveData();
        }

#if !UNITY_EDITOR
        private void OnApplicationQuit()
        {
            DataManager.SaveData();
        }
#endif

#if UNITY_EDITOR
        private void Delete()
        {
            if(Input.GetKeyUp(KeyCode.D))
            {
                DataManager.DeleteData();
            }
        }
#endif

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        private void OnApplicationFocus(bool focus)
        {
            if(focus)
                DataManager.SaveData();
        }

        private void OnApplicationPause(bool pause)
        {
            if(pause)
                DataManager.SaveData();
        }
#endif
    }
}
