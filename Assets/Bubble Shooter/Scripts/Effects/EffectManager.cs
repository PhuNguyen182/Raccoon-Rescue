using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Effects.BallEffects;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Effects
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem ballPopEffect;
        [SerializeField] private ParticleSystem colorfulEffect;
        [SerializeField] private ParticleSystem woodenEffect;
        [SerializeField] private FlyTextEffect[] textPopEffect;
        [SerializeField] private ParticleSystem[] ballBoosterEffects;

        public static EffectManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Preload()
        {
            SimplePool.PoolPreLoad(ballPopEffect.gameObject, 20, EffectContainer.Transform);
            SimplePool.PoolPreLoad(colorfulEffect.gameObject, 20, EffectContainer.Transform);

            for (int i = 0; i < textPopEffect.Length; i++)
            {
                SimplePool.PoolPreLoad(textPopEffect[i].gameObject, 20, EffectContainer.Transform);
            }
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

        public FlyTextEffect SpawnTextPopEffect(EntityType color, Vector3 position, Quaternion rotation)
        {
            FlyTextEffect effect = color switch
            {
                EntityType.Blue => textPopEffect[0],
                EntityType.Green => textPopEffect[1],
                EntityType.Orange => textPopEffect[2],
                EntityType.Red => textPopEffect[3],
                EntityType.Violet => textPopEffect[4],
                EntityType.Yellow => textPopEffect[5],
                _ => null
            };

            return effect != null ? SimplePool.Spawn(effect, EffectContainer.Transform, position, rotation) : null;
        }

        public ParticleSystem SpawnBoosterEffect(EntityType color, Vector3 position, Quaternion rotation)
        {
            ParticleSystem effect = color switch
            {
                EntityType.Blue => ballBoosterEffects[0],
                EntityType.Green => ballBoosterEffects[1],
                EntityType.Orange => ballBoosterEffects[2],
                EntityType.Red => ballBoosterEffects[3],
                EntityType.Violet => ballBoosterEffects[4],
                EntityType.Yellow => ballBoosterEffects[5],
                _ => null
            };

            return effect != null ? SimplePool.Spawn(effect, EffectContainer.Transform, position, rotation) : null ;
        }
    }
}
