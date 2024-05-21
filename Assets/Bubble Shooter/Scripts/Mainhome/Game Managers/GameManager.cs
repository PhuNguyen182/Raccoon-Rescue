using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Mainhome.GameManagers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private HeartTimeManager heartManager;

        protected override void OnAwake()
        {
            
        }

        private void Start()
        {
            heartManager.LoadHeartOnStart();
        }

        private void Update()
        {
            heartManager.UpdateHeartTime();
        }

        private void OnDestroy()
        {
            DataManager.SaveData();
        }
    }
}
