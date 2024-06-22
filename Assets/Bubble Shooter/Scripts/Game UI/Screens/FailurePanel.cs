using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.GameUI.Screens
{
    public class FailurePanel : MonoBehaviour
    {
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject failedMessage;
        [SerializeField] private GameObject skippedMessage;
        [SerializeField] private Animator animator;

        [Header("Navigation Buttons")]
        [SerializeField] private Button addMoveButton;
        [SerializeField] private Button skipButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button quitButton;

        private CancellationToken _token;
        private UniTaskCompletionSource<bool> _completionSource;
        private readonly int _disappearHash = Animator.StringToHash("Disappear");

        private void Awake()
        {
            skipButton.onClick.AddListener(Skip);
            backButton.onClick.AddListener(Back);
            quitButton.onClick.AddListener(() => Quit().Forget());
            addMoveButton.onClick.AddListener(OnAddMove);
            _token = this.GetCancellationTokenOnDestroy();
        }

        private void OnEnable()
        {
            background.SetActive(true);
        }

        public UniTask<bool> Show()
        {
            gameObject.SetActive(true);
            _completionSource = new();
            return _completionSource.Task;
        }

        private void AddMove()
        {
            _completionSource?.TrySetResult(true);
            Close().Forget();
        }

        private void OnAddMove()
        {
            AddMove();
        }

        private void Skip()
        {
            failedMessage.SetActive(false);
            skippedMessage.SetActive(true);
        }

        private void Back()
        {
            skippedMessage.SetActive(false);
            failedMessage.SetActive(true);
        }

        private async UniTask Quit()
        {
            await Close();
            _completionSource?.TrySetResult(false);
        }

        public async UniTask Close()
        {
            animator.Play(_disappearHash);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: _token);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            background.SetActive(false);
        }
    }
}
