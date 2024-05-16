using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Effects.BallEffects;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Effects
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private ParticleSystem ballPopEffect;
        [SerializeField] private ParticleSystem colorfulEffect;
        [SerializeField] private ParticleSystem woodenEffect;
        [SerializeField] private FlyTextEffect textPopEffect;
        [SerializeField] private ParticleSystem[] ballBoosterEffects;

        public Camera MainCamera => mainCamera;
        public static EffectManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            PreloadEffect();
        }

        private void PreloadEffect()
        {
            SimplePool.PoolPreLoad(ballPopEffect.gameObject, 20, EffectContainer.Transform);
            SimplePool.PoolPreLoad(colorfulEffect.gameObject, 20, EffectContainer.Transform);
            SimplePool.PoolPreLoad(textPopEffect.gameObject, 20, EffectContainer.Transform);
        }

        public FlyTextEffect SpawnFlyText(Vector3 position, Quaternion rotation)
        {
            return SimplePool.Spawn(textPopEffect, EffectContainer.Transform, position, rotation);
        }

        public void SpawnBallPopEffect(Vector3 position, Quaternion rotation)
        {
            SimplePool.Spawn(ballPopEffect, EffectContainer.Transform, position, rotation);
        }

        public void SpawnColorfulEffect(Vector3 position, Quaternion rotation)
        {
            SimplePool.Spawn(colorfulEffect, EffectContainer.Transform, position, rotation);
        }

        public void SpawnWoodenEffect(Vector3 position, Quaternion rotation)
        {
            SimplePool.Spawn(woodenEffect, EffectContainer.Transform, position, rotation);
        }

        public ParticleSystem SpawnBoosterEffect(EntityType color, Vector3 position, Quaternion rotation)
        {
            ParticleSystem effect = color switch
            {
                EntityType.FireBall => ballBoosterEffects[0],
                EntityType.LeafBall => ballBoosterEffects[1],
                EntityType.SunBall => ballBoosterEffects[2],
                EntityType.WaterBall => ballBoosterEffects[3],
                _ => null
            };

            return effect != null ? SimplePool.Spawn(effect, EffectContainer.Transform, position, rotation) : null ;
        }
    }
}
