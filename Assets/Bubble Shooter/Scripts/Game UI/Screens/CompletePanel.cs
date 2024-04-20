using R3;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Effects.Tweens;
using Cysharp.Threading.Tasks;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.Screens
{
    public class CompletePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Button nextButton;
        [SerializeField] private AnimationClip appearClip;

        [Header("Feel")]
        [SerializeField] private Animator panelAnimator;
        [SerializeField] private TweenValueEffect scoreTween;

        private CancellationToken _token;
        private ReactiveProperty<int> _reactiveScore = new(0);
        private static readonly int _tierHash = Animator.StringToHash("Tier");

        private int _tier = 0;
        private int _score = 0;

        private void Awake()
        {
            nextButton.onClick.AddListener(OnNext);
            scoreTween.BindInt(_reactiveScore, ShowScore);
            _token = this.GetCancellationTokenOnDestroy();
        }

        private void OnEnable()
        {
            ShowNextStage().Forget();
        }

        public void SetScore(int tier, int score)
        {
            _tier = tier;
            _score = score;
        }

        public void SetLevel(int level)
        {
            levelText.text = $"{level}";
        }

        private void OnNext()
        {

        }

        private void ShowScore(int score)
        {
            scoreText.text = $"{score}";
        }

        private async UniTask ShowNextStage()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(appearClip.length + 0.5f), cancellationToken: _token);
            panelAnimator.SetInteger(_tierHash, _tier);
            _reactiveScore.Value = _score;
        }
    }
}
