using R3;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Effects.Tweens;
using BubbleShooter.Scripts.Common.Configs;
using BubbleShooter.Scripts.Common.Enums;
using UnityEngine.SceneManagement;
using Scripts.SceneUtils;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.Screens
{
    public class CompletePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Button nextButton;
        [SerializeField] private GameObject background;
        [SerializeField] private AnimationClip appearClip;

        [Header("Feel")]
        [SerializeField] private Animator panelAnimator;
        [SerializeField] private TweenValueEffect scoreTween;

        [Header("Audios")]
        [SerializeField] private AudioSource popupAudio;
        [SerializeField] private AudioClip starClip;
        [SerializeField] private AudioClip scoreClip;

        private CancellationToken _token;
        private ReactiveProperty<int> _reactiveScore = new(0);

        private readonly int _tierHash = Animator.StringToHash("Tier");
        private readonly int _closeHash = Animator.StringToHash("Close");

        private int _level = 0;
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
            _level = level;
            levelText.text = $"LEVEL {level}";
        }

        private void OnNext()
        {
            Close().Forget();
        }

        private void ShowScore(int score)
        {
            scoreText.text = $"{score}";
        }

        private async UniTask ShowNextStage()
        {
            background.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(appearClip.length), cancellationToken: _token);

            panelAnimator.SetInteger(_tierHash, _tier);
            _reactiveScore.Value = _score;
            await PlayScoreAudio();
        }

        public async UniTask Close()
        {
            panelAnimator.SetTrigger(_closeHash);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: _token);
            gameObject.SetActive(false);

            if (!PlayConfig.Current.IsTest)
            {
                bool levelIncrease = false;
                GameData.Instance.AddHeart(1);
                int currentLevel = GameData.Instance.GetCurrentLevel();

                // Save current win level immediately
                if (_level == currentLevel)
                {
                    levelIncrease = true;
                    GameData.Instance.SetLevel(_level + 1);
                }

                BackHomeConfig.Current = new BackHomeConfig
                {
                    IsWin = true,
                    Level = _level,
                    Star = _tier,
                    LevelIncreased = levelIncrease
                };

                GameData.Instance.AddLevelProgress(new LevelProgress
                {
                    Level = _level,
                    Star = _tier
                });

                TransitionConfig.Current = new TransitionConfig
                {
                    SceneName = SceneName.Mainhome
                };

                PlayConfig.Current = null;
                await SceneLoader.LoadScene(SceneConstants.Transition, LoadSceneMode.Single);
            }
        }

        private async UniTask PlayScoreAudio()
        {
            for (int i = 0; i < 5; i++)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.07f), cancellationToken: _token);
                popupAudio.PlayOneShot(scoreClip);
            }
        }

        // Play in animation clip keyframe
        public void PlayStarAudio()
        {
            popupAudio.PlayOneShot(starClip);
        }
    }
}
