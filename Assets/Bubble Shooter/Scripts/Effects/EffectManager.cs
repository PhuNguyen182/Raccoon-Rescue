using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Effects.BallEffects;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Effects
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField] private GameObject ballPopEffect;
        [SerializeField] private FlyTextEffect[] textPopEffect;
        [SerializeField] private GameObject[] ballBoosterEffects;

        public static EffectManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Preload()
        {
            SimplePool.PoolPreLoad(ballPopEffect, 20, EffectContainer.Transform);

            for (int i = 0; i < textPopEffect.Length; i++)
            {
                SimplePool.PoolPreLoad(textPopEffect[i].gameObject, 20, EffectContainer.Transform);
            }

            for (int i = 0; i < ballBoosterEffects.Length; i++)
            {
                SimplePool.PoolPreLoad(ballBoosterEffects[i], 20, EffectContainer.Transform);
            }
        }

        public void SpawnBallPopEffect(Vector3 position, Quaternion rotation)
        {
            SimplePool.Spawn(ballPopEffect, EffectContainer.Transform, position, rotation);
        }

        public GameObject SpawnTextPopEffect(EntityType color, Vector3 position, Quaternion rotation)
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

            return SimplePool.Spawn(effect.gameObject, EffectContainer.Transform, position, rotation);
        }

        public GameObject SpawnBoosterEffect(EntityType color, Vector3 position, Quaternion rotation)
        {
            GameObject effect = color switch
            {
                EntityType.Blue => ballBoosterEffects[0],
                EntityType.Green => ballBoosterEffects[1],
                EntityType.Orange => ballBoosterEffects[2],
                EntityType.Red => ballBoosterEffects[3],
                EntityType.Violet => ballBoosterEffects[4],
                EntityType.Yellow => ballBoosterEffects[5],
                _ => null
            };

            return SimplePool.Spawn(effect, EffectContainer.Transform, position, rotation);
        }
    }
}
