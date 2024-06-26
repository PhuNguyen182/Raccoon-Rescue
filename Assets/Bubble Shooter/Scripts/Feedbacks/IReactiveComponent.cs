using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Feedbacks
{
    public interface IReactiveComponent 
    {
        public string InstanceID { get; }
        public UniTask OnReactive(UniTask task);
        public void SetCompletionSource(UniTaskCompletionSource tcs);
    }

    public interface IReactiveComponent<T> : IReactiveComponent where T : Component
    {
        public T Receiver { get; }
        public Vector3 Position { get; }
    }
}
