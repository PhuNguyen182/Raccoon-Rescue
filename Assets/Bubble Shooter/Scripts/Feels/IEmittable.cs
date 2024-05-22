using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Feels
{
    public interface IEmittable
    {
        public UniTask Emit(string id, Action action);
        public void Subscribe(IReactiveComponent component);
        public void Unsubscribe(IReactiveComponent component);
    }
}
