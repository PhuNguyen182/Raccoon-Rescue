using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Feels
{
    public interface IReactiveComponent 
    {
        public string InstanceID { get; }
        public UniTask OnReactive();
        public void SetCompletionSource(UniTaskCompletionSource tcs);
    }

    public interface IReactiveComponent<T> : IReactiveComponent where T : Component
    {
        public T Receiver { get; }
        public Vector3 Position { get; }
    }
}
