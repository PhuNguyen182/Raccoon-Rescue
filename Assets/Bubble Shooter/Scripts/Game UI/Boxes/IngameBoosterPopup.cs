using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.Boxes
{
    public class IngameBoosterPopup : BaseBox<IngameBoosterPopup>
    {
        [SerializeField] private TMP_Text coinAmount;
        [SerializeField] private IngameBoosterType boosterType;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private Animator boxAnimator;

        [Header("Stage Objects")]
        [SerializeField] private GameObject[] stage1Objects;
        [SerializeField] private GameObject[] stage2Objects;

        private int _stage = 0;
        private CancellationToken _token;
        private static readonly int _appearHash = Animator.StringToHash("Appear");
        private static readonly int _disappearHash = Animator.StringToHash("Disappear");

        protected override void OnAwake()
        {
            _token = this.GetCancellationTokenOnDestroy();
            purchaseButton.onClick.AddListener(Purchase);
            closeButton.onClick.AddListener(Close);
        }

        protected override void OnStart()
        {
            SetObjectsActive(stage1Objects, true);
            SetObjectsActive(stage2Objects, false);
        }

        private void Purchase()
        {
            if(_stage == 0)
            {
                DoNextStage().Forget();
                // Do purchase
            }

            else if(_stage == 1)
            {
                Close();
            }
        }

        private async UniTask DoNextStage()
        {
            _stage = _stage + 1;
            boxAnimator.SetTrigger(_disappearHash);
            await UniTask.Delay(TimeSpan.FromSeconds(0.775f), cancellationToken: _token);

            SetObjectsActive(stage1Objects, false);
            SetObjectsActive(stage2Objects, true);
            boxAnimator.SetTrigger(_appearHash);
        }

        private void SetCoint(int coin)
        {
            coinAmount.text = $"{coin}";
        }

        protected override void DoClose()
        {
            DoCloseAnimation().Forget();
        }

        private async UniTask DoCloseAnimation()
        {
            boxAnimator.SetTrigger(_disappearHash);
            await UniTask.Delay(TimeSpan.FromSeconds(0.85f), cancellationToken: _token);
            base.DoClose();
        }

        private void SetObjectsActive(GameObject[] objects, bool active)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(active);
            }
        }

        protected override void OnDisable()
        {
            _stage = 0;
            boxAnimator.ResetTrigger(_appearHash);
            boxAnimator.ResetTrigger(_disappearHash);
            base.OnDisable();
        }
    }
}
