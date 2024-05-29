using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.UpdateHandlerPattern;

public class AutoDespawn : MonoBehaviour, IUpdateHandler
{
    [SerializeField] private float duration;

    private float _timer = 0;

    private void Awake()
    {
        UpdateHandlerManager.Instance.AddUpdateBehaviour(this);
    }

    private void OnEnable()
    {
        _timer = 0;
    }

    public void OnUpdate(float deltaTime)
    {
        _timer += deltaTime;
        if (_timer >= duration)
        {
            _timer = 0;
            SimplePool.Despawn(this.gameObject);
        }
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }

    private void OnDestroy()
    {
        UpdateHandlerManager.Instance?.RemoveUpdateBehaviour(this);
    }
}
