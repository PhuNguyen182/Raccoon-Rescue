using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.MessageBrokers;
using DG.Tweening;

namespace Scripts.Service
{
    public class InitializeService : SingletonClass<InitializeService>, IService
    {
        private MessageBrokerRegister _messageBroker;

        public void Initialize()
        {
            InitMessageBroker();
            LoadGameData();
            InitDOTween();
        }

        private void InitMessageBroker()
        {
            _messageBroker = new();
            _messageBroker.InitializeMessages();
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
