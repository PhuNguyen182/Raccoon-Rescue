using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TestAsync : MonoBehaviour
{
    private int _count = 5;
    private CancellationToken _destroyToken;

    private void Awake()
    {
        _destroyToken = this.GetCancellationTokenOnDestroy();
    }

    private void Start()
    {
        TestWaitUntil().Forget();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _count = _count - 1;
        }
    }

    private async UniTask TestWaitUntil()
    {
        await UniTask.WaitUntil(() => _count == 0, cancellationToken: _destroyToken);
        if (_destroyToken.IsCancellationRequested)
            return;

        Debug.Log("Time out!");
    }
}
