using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Messages;
using MessagePipe;

namespace Scripts.Common.MessageBrokers
{
    public class MessageBrokerRegister
    {
        private IServiceProvider _provider;
        private BuiltinContainerBuilder _builder;

        public void InitializeMessages()
        {
            _builder = new();
            _builder.AddMessagePipe();

            AddMeggageBrokers();
            _provider = _builder.BuildServiceProvider();
            GlobalMessagePipe.SetProvider(_provider);
        }

        private void AddMeggageBrokers()
        {
            _builder.AddMessageBroker<PowerupMessage>();
            _builder.AddMessageBroker<CheckMatchMessage>();
            _builder.AddMessageBroker<ActiveBoosterMessage>();
            _builder.AddMessageBroker<CheckTargetMessage>();
            _builder.AddMessageBroker<DecreaseMoveMessage>();
            _builder.AddMessageBroker<AddScoreMessage>();
        }
    }
}
