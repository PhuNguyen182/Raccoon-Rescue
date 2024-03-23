using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;

namespace Scripts.Common.MessageBrokers
{
    public class MessageBrokerRegister
    {
        private readonly IServiceProvider _provider;
        private readonly BuiltinContainerBuilder _builder;

        public MessageBrokerRegister()
        {
            _builder = new();
            AddMeggageBrokers();

            _provider = _builder.BuildServiceProvider();
            GlobalMessagePipe.SetProvider(_provider);
        }

        private void AddMeggageBrokers()
        {
            // Add message type here. For example:
            // _builder.AddMessageBroker<int>();
            // _builder.AddMessageBroker<Vector2>();
        }
    }
}
