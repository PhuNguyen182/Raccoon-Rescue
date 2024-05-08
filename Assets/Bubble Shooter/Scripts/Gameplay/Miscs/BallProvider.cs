using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.Scripts.Gameplay.GameManagers;
using BubbleShooter.Scripts.Gameplay.Models;
using BubbleShooter.Scripts.Common.Enums;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class BallProvider : MonoBehaviour
    {
        [SerializeField] private Transform toPoint;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private BallShooter ballShooter;

        [Header("Dummy Balls")]
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall blue;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall green;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall orange;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall red;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall violet;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private DummyBall yellow;

        [Header("Buttons")]
        [SerializeField] private Button potButton;
        [SerializeField] private Button arrowButton;

        private bool _canClick;
        private BallShootModel _firstModel;
        private BallShootModel _secondModel;
        private CancellationToken _token;
        public DummyBall DummyBall;

        #region Random color calculating
        private List<int> _colorDensities = new();
        private List<float> _probabilities = new();
        private List<EntityType> _colors = new();
        private List<ColorMapData> _colorStrategy = new();
        #endregion

        private void Awake()
        {
            _canClick = true;
            _token = this.GetCancellationTokenOnDestroy();
            potButton.onClick.AddListener(SwitchBall);
            arrowButton.onClick.AddListener(SwitchBall);
        }

        public void SetMoveCount(List<ColorMapData> colorMapDatas)
        {
            _colorStrategy = colorMapDatas;
            CalculateRandomBall();
            CreateBallOnStartGame();
        }

        public async UniTask PopSequence()
        {
            _firstModel = _secondModel;
            _secondModel = GetRandomColorBallInPot();
            await DummyBall.SwapTo(ballShooter.ShotPoint.position);

            ballShooter.SetColorModel(_firstModel, true);
            SetBallColor(true, _secondModel.BallColor, DummyBallState.Create);
        }

        public void SetBallColor(bool isActive, EntityType color, DummyBallState ballState)
        {
            if (DummyBall != null)
                SimplePool.Despawn(DummyBall.gameObject);

            if (!isActive)
                return;

            DummyBall ballPrefab = color switch
            {
                EntityType.Red => red,
                EntityType.Yellow => yellow,
                EntityType.Green => green,
                EntityType.Blue => blue,
                EntityType.Violet => violet,
                EntityType.Orange => orange,
                _ => null
            };

            DummyBall = SimplePool.Spawn(ballPrefab, spawnPoint, spawnPoint.position, Quaternion.identity);

            if (ballState == DummyBallState.Create)
                DummyBall.transform.DOScale(0, 0.2f).SetEase(Ease.OutQuad).From();
        }

        private void CreateBallOnStartGame()
        {
            _firstModel = GetRandomColorBallInPot();
            ballShooter.SetColorModel(_firstModel, true);
            _secondModel = GetRandomColorBallInPot();
            SetBallColor(true, _secondModel.BallColor, DummyBallState.New);
        }

        public BallShootModel GetRandomHelperBall()
        {
            EntityType currentColor = ballShooter.BallModel.BallColor;
            int randomIndex = ProbabilitiesController.GetItemByProbabilityRarity(_probabilities);
            EntityType nextColor = _colors[Mathf.Abs(randomIndex) % _probabilities.Count];

            while (currentColor == nextColor)
            {
                randomIndex = ProbabilitiesController.GetItemByProbabilityRarity(_probabilities);
                nextColor = _colors[Mathf.Abs(randomIndex) % _probabilities.Count];
            }

            BallShootModel ballModel = new BallShootModel
            {
                BallColor = nextColor,
                BallCount = ballShooter.BallModel.BallCount,
                IsPowerup = ballShooter.BallModel.IsPowerup
            };

            return ballModel;
        }

        private BallShootModel GetRandomColorBallInPot()
        {
            int randomIndex = ProbabilitiesController.GetItemByProbabilityRarity(_probabilities);
            EntityType color = _colors[Mathf.Abs(randomIndex) % _probabilities.Count];

            BallShootModel newModel = new BallShootModel
            {
                IsPowerup = false,
                BallColor = color,
                BallCount = 1
            };

            return newModel;
        }

        private void CalculateRandomBall()
        {
            for (int i = 0; i < _colorStrategy.Count; i++)
            {
                _colorDensities.Add(_colorStrategy[i].ColorProportion.Coefficient);
                _colors.Add(_colorStrategy[i].ColorProportion.Color);
            }

            for (int i = 0; i < _colorDensities.Count; i++)
            {
                float probability = DistributeCalculator.GetPercentage(_colorDensities[i], _colorDensities);
                _probabilities.Add(probability * 100f);
            }
        }

        private void SwitchBall()
        {
            SwitchBallAsync().Forget();
        }

        private async UniTask SwitchBallAsync()
        {
            if (!_canClick)
                return;

            _canClick = false;
            GameController.Instance.SetInputActive(false);
            GameController.Instance.MainScreenManager.SetInvincibleObjectActive(true);

            if (arrowButton.gameObject.activeInHierarchy)
                arrowButton.gameObject.SetActive(false);

            UniTask swapBall1 = DummyBall.SwapTo(ballShooter.DummyBall.transform.position);
            UniTask swapBall2 = ballShooter.DummyBall.SwapTo(DummyBall.transform.position);
            await UniTask.WhenAll(swapBall1, swapBall2);

            (_firstModel, _secondModel) = (_secondModel, _firstModel);
            SetBallColor(true, _secondModel.BallColor, DummyBallState.Swap);
            ballShooter.SetColorModel(_firstModel, true);

            GameController.Instance.SetInputActive(true);
            await UniTask.DelayFrame(60, PlayerLoopTiming.Update, _token); ;
            GameController.Instance.MainScreenManager.SetInvincibleObjectActive(false);
            _canClick = true;
        }
    }
}
