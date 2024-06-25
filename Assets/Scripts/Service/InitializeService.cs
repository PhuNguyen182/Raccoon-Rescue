using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.MessageBrokers;
using DG.Tweening;

namespace Scripts.Service
{
    public class InitializeService : SingletonClass<InitializeService>, IService
    {
        public void Initialize()
        {
            LoadGameData();
            InitMessageBroker();
            InitDOTween();
        }

        private void InitMessageBroker()
        {
            MessageBrokerRegister messageBroker = new();
            messageBroker.InitializeMessages();
        }

        private void LoadGameData()
        {
            DataManager.LoadData();
        }

        private void InitDOTween()
        {
            DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(1200, 120);
        }
    }
}
