using R3;
using System;
using UnityEngine;
using TMPro;

public class WaitingBox : MonoBehaviour
{
    [SerializeField] TMP_Text messageText;

    private Action _onTimeOut;
    private static WaitingBox _instance;
    private IDisposable _waitDispose;

    public const string WaitingBoxPath = "Boxes/WaitingBox";

    public static WaitingBox Setup()
    {
        if (_instance == null)
        {
            _instance = Instantiate(Resources.Load<WaitingBox>(WaitingBoxPath));
        }

        _instance.gameObject.SetActive(true);
        return _instance;
    }


    public void ShowWaiting()
    {
        BoxController.Instance.isLockEscape = true;
        gameObject.SetActive(true);
    }


    public void HideWaiting(bool isLockEscape = false)
    {
        BoxController.Instance.isLockEscape = isLockEscape;

        gameObject.SetActive(false);

        if (_waitDispose != null)
            _waitDispose.Dispose();
    }

    public void ShowWaiting(float time)
    {
        _onTimeOut = null;
        ShowWaiting();
        TimeOut(time);
    }

    public void ShowWaiting(float time, Action action)
    {
        ShowWaiting();
        _onTimeOut = action;
        TimeOut(time);
    }

    private void TimeOut(float time)
    {
        _waitDispose?.Dispose();
        _waitDispose = Observable.Timer(TimeSpan.FromSeconds(time))
                       .Subscribe(_ =>
                       {
                            HideWaiting();
                            _onTimeOut?.Invoke();
                       });
    }

    private void OnDestroy()
    {
        _waitDispose?.Dispose();
    }
}