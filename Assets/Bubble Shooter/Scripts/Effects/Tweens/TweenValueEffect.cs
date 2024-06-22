using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BubbleShooter.Scripts.Effects.Tweens
{
    public class TweenValueEffect : MonoBehaviour
    {
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private Ease ease = Ease.OutQuad;

        public void BindInt(ReactiveProperty<int> reactiveValue, Action<int> callback = null)
        {
            DisposableBuilder builder = Disposable.CreateBuilder();
            
            reactiveValue.Prepend(reactiveValue.Value)
                         .Pairwise().Index()
                         .Subscribe(value => CreateIntTween(value.Item.Previous, value.Item.Current, duration, ease, callback))
                         .AddTo(ref builder);

            builder.RegisterTo(this.destroyCancellationToken);
        }

        public void BindFloat(ReactiveProperty<float> reactiveValue, Action<float> callback = null)
        {
            DisposableBuilder builder = Disposable.CreateBuilder();

            reactiveValue.Prepend(reactiveValue.Value)
                         .Pairwise().Index()
                         .Subscribe(value => CreateFloatTween(value.Item.Previous, value.Item.Current, duration, ease, callback))
                         .AddTo(ref builder);

            builder.RegisterTo(this.destroyCancellationToken);
        }

        public void BindVector2(ReactiveProperty<Vector2> reactiveValue, Action<Vector2> callback = null)
        {
            DisposableBuilder builder = Disposable.CreateBuilder();

            reactiveValue.Prepend(reactiveValue.Value)
                         .Pairwise().Index()
                         .Subscribe(value => CreateVector2Tween(value.Item.Previous, value.Item.Current, duration, ease, callback))
                         .AddTo(ref builder);

            builder.RegisterTo(this.destroyCancellationToken);
        }

        public void BindVector3(ReactiveProperty<Vector3> reactiveValue, Action<Vector3> callback = null)
        {
            DisposableBuilder builder = Disposable.CreateBuilder();

            reactiveValue.Prepend(reactiveValue.Value)
                         .Pairwise().Index()
                         .Subscribe(value => CreateVector3Tween(value.Item.Previous, value.Item.Current, duration, ease, callback))
                         .AddTo(ref builder);

            builder.RegisterTo(this.destroyCancellationToken);
        }

        public void BindColor(ReactiveProperty<Color> reactiveValue, Action<Color> callback = null)
        {
            DisposableBuilder builder = Disposable.CreateBuilder();

            reactiveValue.Prepend(reactiveValue.Value)
                         .Pairwise().Index()
                         .Subscribe(value => CreateColorTween(value.Item.Previous, value.Item.Current, duration, ease, callback))
                         .AddTo(ref builder);

            builder.RegisterTo(this.destroyCancellationToken);
        }

        public float EvaluateFloat(float from, float to, float perccentage, Ease ease)
        {
            return DOVirtual.EasedValue(from, to, perccentage, ease);
        }

        public float EvaluateFloat(float from, float to, float perccentage, AnimationCurve curve)
        {
            return DOVirtual.EasedValue(from, to, perccentage, curve);
        }

        public Vector2 EvaluateVector2(Vector2 from, Vector2 to, float perccentage, Ease ease)
        {
            return DOVirtual.EasedValue(from, to, perccentage, ease);
        }

        public Vector2 EvaluateVector2(Vector2 from, Vector2 to, float perccentage, AnimationCurve curve)
        {
            return DOVirtual.EasedValue(from, to, perccentage, curve);
        }

        public Vector3 EvaluateVector3(Vector3 from, Vector3 to, float perccentage, Ease ease)
        {
            return DOVirtual.EasedValue(from, to, perccentage, ease);
        }

        public Vector3 EvaluateVector3(Vector3 from, Vector3 to, float perccentage, AnimationCurve curve)
        {
            return DOVirtual.EasedValue(from, to, perccentage, curve);
        }

        public void DelayCallback(float delayTime, Action callback = null, bool isIgnoreTimeScale = true)
        {
            DOVirtual.DelayedCall(delayTime, () => callback?.Invoke(), isIgnoreTimeScale);
        }

        private Tween CreateIntTween(int from, int to, float duration, Ease ease, Action<int> callback = null)
        {
            Tween tween = DOVirtual.Int(from, to, duration, value =>
            {
                callback?.Invoke(value);
            });

            tween.SetEase(ease);
            tween.OnComplete(() => tween.Kill());
            return tween;
        }

        private Tween CreateFloatTween(float from, float to, float duration, Ease ease, Action<float> callback = null)
        {
            Tween tween = DOVirtual.Float(from, to, duration, value =>
            {
                callback?.Invoke(value);
            });

            tween.SetEase(ease);
            tween.OnComplete(() => tween.Kill());
            return tween;
        }

        private Tween CreateVector2Tween(Vector2 from, Vector2 to, float duration, Ease ease, Action<Vector2> callback = null)
        {
            Tween tween = DOVirtual.Vector2(from, to, duration, value =>
            {
                callback?.Invoke(value);
            });

            tween.SetEase(ease);
            tween.OnComplete(() => tween.Kill());
            return tween;
        }

        private Tween CreateVector3Tween(Vector3 from, Vector3 to, float duration, Ease ease, Action<Vector3> callback = null)
        {
            Tween tween = DOVirtual.Vector3(from, to, duration, value =>
            {
                callback?.Invoke(value);
            });

            tween.SetEase(ease);
            tween.OnComplete(() => tween.Kill());
            return tween;
        }

        private Tween CreateColorTween(Color from, Color to, float duration, Ease ease, Action<Color> callback = null)
        {
            Tween tween = DOVirtual.Color(from, to, duration, value =>
            {
                callback?.Invoke(value);
            });

            tween.SetEase(ease);
            tween.OnComplete(() => tween.Kill());
            return tween;
        }
    }
}
