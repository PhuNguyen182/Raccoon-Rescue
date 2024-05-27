using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleContainer : MonoBehaviour 
{
    [SerializeField] protected bool isAwakeInit;
}

public class SimpleContainer<T> : SimpleContainer where T : Component
{
    private static SimpleContainer _instance;

    public static SimpleContainer Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameObject($"{typeof(T)}").AddComponent<SimpleContainer>();

            return _instance;
        }
    }

    private void Awake()
    {
        if (isAwakeInit)
            _instance = this;
    }

    public static Transform Transform => Instance.transform;
}
