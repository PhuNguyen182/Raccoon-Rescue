using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Feedbacks
{
    public class Emittable : IEmittable, IDisposable
    {
        private static Emittable _instance;
        private Dictionary<string, IReactiveComponent> _receivers;

        public Action<string> OnComplete;
        public static Emittable Default => _instance ?? (_instance = new());

        public Emittable()
        {
            _receivers = new();
        }

        public async UniTask Emit(string id, Action action)
        {
            if (_receivers.ContainsKey(id))
            {
                action?.Invoke();
                UniTaskCompletionSource tcs = new();
                _receivers[id].SetCompletionSource(tcs);

                await tcs.Task;
                OnComplete?.Invoke(id);
            }
        }

        public void Subscribe(IReactiveComponent component)
        {
            if (_receivers.ContainsKey(component.InstanceID))
                _receivers.Add(component.InstanceID, component);
        }

        public void Unsubscribe(IReactiveComponent component)
        {
            if (_receivers.ContainsKey(component.InstanceID))
                _receivers.Remove(component.InstanceID);
        }

        public void Dispose()
        {
            _receivers?.Clear();
        }
    }
}
