using R3;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Effects.Tweens;
using BubbleShooter.Scripts.Common.Messages;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TMPro;

namespace BubbleShooter.Scripts.GameUI.Boxes
{
    public class IngameBoosterPopup : BaseBox<IngameBoosterPopup>
    {
        [SerializeField] private TMP_Text coinAmount;
        [SerializeField] private TMP_Text boosterPrice;
        [SerializeField] private IngameBoosterType boosterType;
        [SerializeField] private TweenValueEffect coinTween;

        [SerializeField] private Button closeButton;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private Animator boxAnimator;

        [Header("Stage Objects")]
        [SerializeField] private GameObject[] stage1Objects;
        [SerializeField] private GameObject[] stage2Objects;

        private int _stage = 0;
        private int _price = 0;
        private CancellationToken _token;
        private IPublisher<AddIngameBoosterMessage> _boosterPublisher;
        private ReactiveProperty<int> _coinReactive = new(0);

        private static readonly int _appearHash = Animator.StringToHash("Appear");
        private static readonly int _disappearHash = Animator.StringToHash("Disappear");

        protected override void OnAwake()
        {
            _token = this.GetCancellationTokenOnDestroy();
            purchaseButton.onClick.AddListener(Purchase);
            closeButton.onClick.AddListener(Close);
            coinTween.BindInt(_coinReactive, value => coinAmount.text = $"{value}");
        }

        protected override void DoAppear()
        {
            SetCoin();
            SetObjectsActive(stage1Objects, true);
            SetObjectsActive(stage2Objects, false);

            _boosterPublisher = GlobalMessagePipe.GetPublisher<AddIngameBoosterMessage>();
        }

        private void Purchase()
        {
            if(_stage == 0)
            {
                DoNextStage().Forget();
                BuyBooster();
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

        private void SetCoin()
        {
            int coin = GameData.Instance.GetCoins();
            _price = GameData.Instance.GameInventory.GetIngameBoosterPrice(boosterType);

            coinAmount.text = $"{coin}";
            boosterPrice.text = $"{_price}";
            _coinReactive.Value = coin;

            purchaseButton.interactable = coin >= _price;
        }

        private void BuyBooster()
        {
            _boosterPublisher.Publish(new AddIngameBoosterMessage
            {
                Amount = 1,
                BoosterType = boosterType
            });

            GameData.Instance.SpendCoins(_price);
            GameData.Instance.AddBooster(boosterType, 1);
            _coinReactive.Value = GameData.Instance.GetCoins();
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
