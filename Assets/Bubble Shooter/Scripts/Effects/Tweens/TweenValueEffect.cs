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

        public void BindVector(ReactiveProperty<Vector3> reactiveValue, Action<Vector3> callback = null)
        {
            DisposableBuilder builder = Disposable.CreateBuilder();

            reactiveValue.Prepend(reactiveValue.Value)
                         .Pairwise().Index()
                         .Subscribe(value => CreateVectorTween(value.Item.Previous, value.Item.Current, duration, ease, callback))
                         .AddTo(ref builder);

            builder.RegisterTo(this.destroyCancellationToken);
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

        private Tween CreateVectorTween(Vector3 from, Vector3 to, float duration, Ease ease, Action<Vector3> callback = null)
        {
            Tween tween = DOVirtual.Vector3(from, to, duration, value =>
            {
                callback?.Invoke(value);
            });

            tween.SetEase(ease);
            tween.OnComplete(() => tween.Kill());
            return tween;
        }
    }
}
