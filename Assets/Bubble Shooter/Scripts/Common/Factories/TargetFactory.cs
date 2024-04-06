using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Common.Factories;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.Scripts.Gameplay.GameEntities;
using BubbleShooter.Scripts.Common.Databases;
using BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls;

namespace BubbleShooter.Scripts.Common.Factories
{
    public class TargetFactory : IFactory<TargetMapData, BaseEntity>
    {
        private readonly EntityDatabase _entityDatabase;
        private readonly Transform _entityContainer;

        public TargetFactory(EntityDatabase entityDatabase, Transform entityContainer)
        {
            _entityDatabase = entityDatabase;
            _entityContainer = entityContainer;
        }

        public BaseEntity Create(TargetMapData data)
        {
            for (int i = 0; i < _entityDatabase.TargetModels.Length; i++)
            {
                var model = _entityDatabase.TargetModels[i];
                if (data.Color == model.EntityType && data.TargetColor == model.TargetType)
                {
                    TargetBall target = SimplePool.Spawn(_entityDatabase.TargetPrefab);
                    target.transform.SetParent(_entityContainer);

                    target.SetID(data.ID);
                    target.SetColor(data.Color);
                    target.SetTargetColor(data.TargetColor);
                    target.InitMessages();
                    target.ResetBall();

                    return target;
                }
            }

            return default;
        }
    }
}
