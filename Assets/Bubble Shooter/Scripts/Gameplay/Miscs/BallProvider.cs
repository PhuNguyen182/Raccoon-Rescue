using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Gameplay.Models;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class BallProvider : MonoBehaviour
    {
        [SerializeField] private Transform toPoint;
        [SerializeField] private SpriteRenderer nextBall;
        [SerializeField] private BallShooter ballShooter;
        [SerializeField] private DummyBall dummyBall;

        [Header("Buttons")]
        [SerializeField] private Button potButton;
        [SerializeField] private Button arrowButton;

        private BallShootModel _firstModel;
        private BallShootModel _secondModel;

        #region Random color calculating
        private List<int> _colorDensities = new();
        private List<float> _probabilities = new();
        private List<EntityType> _colors = new();
        private List<ColorMapData> _colorStrategy = new();
        #endregion

        private void Awake()
        {
            potButton.onClick.AddListener(SwitchBall);
            arrowButton.onClick.AddListener(SwitchBall);
        }

        public void SetMoveCount(List<ColorMapData> colorMapDatas)
        {
            _colorStrategy = colorMapDatas;
            CalculateRandomBall();
            CreateBallOnStartGame();
        }

        public void PopSequence()
        {
            _firstModel = _secondModel;
            ballShooter.SetColorModel(_firstModel, true);
            _secondModel = GetRandomColorBall();
            SetBallColor(true, _secondModel.BallColor);
        }

        public void SwitchRandomBall()
        {
            EntityType currentColor = ballShooter.BallModel.BallColor;
            int randomIndex = ProbabilitiesController.GetItemByProbabilityRarity(_probabilities);
            EntityType nextColor = _colors[Mathf.Abs(randomIndex) % _probabilities.Count];

            while (currentColor == nextColor)
            {
                randomIndex = ProbabilitiesController.GetItemByProbabilityRarity(_probabilities);
                nextColor = _colors[Mathf.Abs(randomIndex) % _probabilities.Count];
            }

            var ballModel = new BallShootModel
            {
                BallColor = nextColor,
                BallCount = ballShooter.BallModel.BallCount,
                IsPowerup = ballShooter.BallModel.IsPowerup
            };

            ballShooter.SetColorModel(ballModel, true);
        }

        public void SetBallColor(bool isActive, EntityType color)
        {
            dummyBall.SetBallColor(isActive, color);
        }

        private void CreateBallOnStartGame()
        {
            _firstModel = GetRandomColorBall();
            ballShooter.SetColorModel(_firstModel, true);
            _secondModel = GetRandomColorBall();
            SetBallColor(true, _secondModel.BallColor);
        }

        private BallShootModel GetRandomColorBall()
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
            if (arrowButton.gameObject.activeInHierarchy)
                arrowButton.gameObject.SetActive(false);

            (_firstModel, _secondModel) = (_secondModel, _firstModel);
            SetBallColor(true, _secondModel.BallColor);
            ballShooter.SetColorModel(_firstModel, true);
        }
    }
}
