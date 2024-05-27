using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.MessageBrokers;

namespace Scripts.Service
{
    public class InitializeService : SingletonClass<InitializeService>, IService
    {
        private MessageBrokerRegister _messageBroker;

        public void Initialize()
        {
            InitMessageBroker();
        }

        private void InitMessageBroker()
        {
            _messageBroker = new();
            _messageBroker.InitializeMessages();
            DataManager.LoadData();
        }
    }
}
