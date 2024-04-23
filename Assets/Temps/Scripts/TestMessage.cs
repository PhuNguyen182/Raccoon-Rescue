using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MessagePipe;
using System.Threading;
using DG.Tweening;


public class TestMessage : MonoBehaviour
{
    private IAsyncSubscriber<Vector3> _positionAsyncSubscriber;
    private IAsyncPublisher<Vector3> _positionAsyncPublisher;

    private UniTaskCompletionSource<TargetInfo> source;

    private bool GetBool()
    {
        return source.GetResult(0).IsValid;
    }

    private async UniTask TestFunc()
    {
        var x = await source.Task;
        if (x.IsValid)
        {
            // XXX
        }
    }

}

public struct TargetInfo
{
    public bool IsValid;
    public Vector3 Value;
}

public static class IMessageBrokerUtil
{
    
}
