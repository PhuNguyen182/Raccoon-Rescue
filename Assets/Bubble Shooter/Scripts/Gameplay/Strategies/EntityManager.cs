using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Factories;
using BubbleShooter.Scripts.Common.Interfaces;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;

namespace BubbleShooter.Scripts.Gameplay.Strategies
{
    public class EntityManager
    {
        private readonly TargetFactory _targetFactory;
        private readonly EntityFactory _entityFactory;

        public EntityManager(TargetFactory targetFactory, EntityFactory entityFactory)
        {
            _targetFactory = targetFactory;
            _entityFactory = entityFactory;
        }

        public IBallEntity SpawnEntity(EntityMapPosition mapPosition)
        {
            return _entityFactory.Create(mapPosition.MapData);
        }

        public IBallEntity SpawnTarget(TargetMapPosition mapPosition)
        {
            return _targetFactory.Create(mapPosition.MapData);
        }
    }
}
