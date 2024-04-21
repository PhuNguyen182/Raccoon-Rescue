using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.Scripts.Gameplay.Strategies;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.Scripts.Gameplay.GameDatas;

namespace BubbleShooter.Scripts.Gameplay.GameTasks
{
    public class FillBoardTask
    {
        private readonly GridCellManager _gridCellManager;
        private readonly MetaBallManager _metaBallManager;

        public FillBoardTask(GridCellManager gridCellManager, MetaBallManager metaBallManager)
        {
            _gridCellManager = gridCellManager;
            _metaBallManager = metaBallManager;
        }

        public void Fill()
        {
            FillEntity();
            FillRandom();
            Encapsulate();
        }

        private void FillEntity()
        {
            foreach (Vector3Int position in _metaBallManager.Iterator())
            {
                IGridCell gridCell = _gridCellManager.Get(position);
                IBallEntity ballEntity = _metaBallManager.GetEntity(position);
                gridCell.SetBall(ballEntity);
            }
        }

        private void FillRandom()
        {
            List<int> colorDensities = new();
            List<float> probabilities = new();
            List<EntityType> colors = new();

            var colorStrategy = _metaBallManager.GetColorStrategy();
            // Gathers all distributes of possible color
            for (int i = 0; i < colorStrategy.Count; i++)
            {
                colorDensities.Add(colorStrategy[i].ColorProportion.Coefficient);
                colors.Add(colorStrategy[i].ColorProportion.Color);
            }

            // GetEntity probability of all colors
            for (int i = 0; i < colorDensities.Count; i++)
            {
                float probability = DistributeCalculator.GetPercentage(colorDensities[i], colorDensities);
                probabilities.Add(probability * 100f);
            }

            var randomEntityFill = _metaBallManager.GetRandomEntityFill();
            // Set random color with calculated probability
            for (int i = 0; i < randomEntityFill.Count; i++)
            {
                // Create a new random ball data
                int randomIndex = ProbabilitiesController.GetItemByProbabilityRarity(probabilities);
                randomIndex = Mathf.Abs(randomIndex) % probabilities.Count;

                EntityMapPosition ballMapPosition = new EntityMapPosition
                {
                    Position = randomEntityFill[i].Position,
                    MapData = new EntityMapData
                    {
                        ID = randomEntityFill[i].MapData.ID,
                        HP = randomEntityFill[i].MapData.HP,
                        EntityType = colors[randomIndex]
                    }
                };

                // Spawn new ball with randomized color
                IBallEntity randomBall = _metaBallManager.AddEntity(ballMapPosition);
                IGridCell gridCell = _gridCellManager.Get(ballMapPosition.Position);
                gridCell.SetBall(randomBall);
            }
        }

        private void Encapsulate()
        {
            _gridCellManager.Encapsulate();
        }
    }
}
