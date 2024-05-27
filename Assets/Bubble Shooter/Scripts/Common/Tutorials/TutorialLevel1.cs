using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Common.Tutorials
{
    public class TutorialLevel1 : BaseTutorial
    {
        [SerializeField] private Button playButton;

        private static readonly int _disappearHash = Animator.StringToHash("");

        protected override void OnAwake()
        {
            playButton.onClick.AddListener(Close);
        }

        public override void DoNextStep() { }

        public override async UniTask Hide()
        {
            tutorialAnimator.SetTrigger(_disappearHash);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: destroyToken);
            base.DoClose();
        }

        public override UniTask Show()
        {
            return UniTask.CompletedTask;
        }

        protected override void DoClose()
        {
            Hide().Forget();
        }
    }
}
