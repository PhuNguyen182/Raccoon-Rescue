using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Effects.Tweens;
using TMPro;

public class TestTween : MonoBehaviour
{
    [SerializeField] private Image imageValue;
    [SerializeField] private TMP_Text textValue;
    [SerializeField] private TweenValueEffect tweenValueEffect;

    private ReactiveProperty<float> _reactiveValue = new();

    private void Awake()
    {
        tweenValueEffect.BindFloat(_reactiveValue, TestTweenFunc);
        _reactiveValue.Value = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _reactiveValue.Value = Random.value;
        }

        if (Input.GetMouseButtonDown(1))
        {
            _reactiveValue.Value = 0;
        }
    }

    private void TestTweenFunc(float value)
    {
        textValue.text = $"{value:F3}";
        imageValue.fillAmount = value;
    }
}
