using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.Factories;
using BubbleShooter.Scripts.Gameplay.GameDatas;
using BubbleShooter.Scripts.Gameplay.GameEntities;
using BubbleShooter.Scripts.Common.Databases;
using BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls;
using BubbleShooter.Scripts.Gameplay.GameEntities.Boosters;
using BubbleShooter.Scripts.Common.Interfaces;

namespace BubbleShooter.Scripts.Common.Factories
{
    public class EntityFactory : IFactory<EntityMapData, BubbleEntity>
    {
        private readonly EntityDatabase _entityDatabase;
        private readonly Transform _entityContainer;

        public EntityFactory(EntityDatabase database, Transform container)
        {
            _entityDatabase = database;
            _entityContainer = container;
            
            PreloadEntities();
        }

        public BubbleEntity Create(EntityMapData data)
        {
            for (int i = 0; i < _entityDatabase.BallModels.Length; i++)
            {
                if(data.EntityType == _entityDatabase.BallModels[i].EntityType)
                {
                    CommonBall ball = SimplePool.Spawn(_entityDatabase.BallPrefab);
                    
                    ball.ResetBall();
                    ball.transform.SetParent(_entityContainer);

                    return ball;
                }
            }

            for (int i = 0; i < _entityDatabase.Boosters.Length; i++)
            {
                if (data.EntityType == _entityDatabase.Boosters[i].BoosterType)
                {
                    BaseBooster booster = SimplePool.Spawn(_entityDatabase.Boosters[i]);

                    booster.ResetBall();
                    booster.transform.SetParent(_entityContainer);

                    return booster;
                }
            }

            for (int i = 0; i < _entityDatabase.BallEntities.Length; i++)
            {
                if (data.EntityType == _entityDatabase.BallEntities[i].EntityType)
                {
                    BubbleEntity entity = SimplePool.Spawn(_entityDatabase.BallEntities[i]);

                    if(entity is IBallHealth health)
                    {
                        health.SetMaxHP(data.HP);
                    }

                    entity.ResetBall();
                    entity.transform.SetParent(_entityContainer);

                    return entity;
                }
            }

            return default;
        }

        private void PreloadEntities()
        {
            SimplePool.PoolPreLoad(_entityDatabase.BallPrefab.gameObject, 20, _entityContainer);
        }
    }
}
