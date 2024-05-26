using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using TMPro;
using System;

public class TestAsync : MonoBehaviour
{
    public TMP_Text lifeText;
    public TMP_Text timeText;
    public Button button;

    public int _count = 5;
    private int life = 0;
    private DateTime saveTime;
    private TimeSpan diff;
    private CancellationToken _destroyToken;
    private UniTaskCompletionSource _tcs = new();

    private void Awake()
    {
        _destroyToken = this.GetCancellationTokenOnDestroy();

        button.onClick.AddListener(() =>
        {
            life -= 1;
        });
    }

    private void Start()
    {
        //CalculateHeart();
        TestTCS().Forget();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _tcs.TrySetResult();
            TestTCS().Forget();
        }
    }

    private void Tick()
    {
        if(life < 5)
        {
            var offset = DateTime.Now.Subtract(saveTime);
            diff = TimeSpan.FromSeconds(15).Subtract(offset);

            if(diff.TotalSeconds <= 0)
            {
                life += 1;
                if(life >= 5)
                {
                    life = 5;
                    saveTime = DateTime.Now;
                }
                else
                {
                    saveTime = saveTime.AddSeconds(15);
                }
            }

            lifeText.text = $"Life: {life}";
            timeText.text = $"{diff.Minutes:D2}:{diff.Seconds:D2}";
        }
    }

    private void CalculateHeart()
    {
        saveTime = DateTime.Now.Subtract(TimeSpan.FromSeconds(15 * _count));
        diff = DateTime.Now.Subtract(saveTime);
        Debug.Log(diff.TotalSeconds);

        do
        {
            diff = diff.Subtract(TimeSpan.FromSeconds(15));
            Debug.Log(diff.TotalSeconds);

            life = life + 1;
            lifeText.text = $"Life: {life}";

            if (life >= 5)
            {
                life = 5;
                lifeText.text = $"Life: {life}";
                saveTime = DateTime.Now;
                break;
            }
            else
            {
                saveTime = saveTime.Add(TimeSpan.FromSeconds(15));
            }
        } while (diff.TotalSeconds > 0);
    }

    private async UniTask TestTCS()
    {
        await _tcs.Task;
        Debug.Log("Mouse click!");
    }

    private async UniTask AddressableTest()
    {
        var x = await Addressables.LoadAssetAsync<GameObject>("");
    }
}
