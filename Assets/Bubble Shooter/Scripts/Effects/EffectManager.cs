using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Effects
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField] private GameObject ballPopEffect;
        [SerializeField] private GameObject textPopEffect;
        [SerializeField] private GameObject[] ballBoosterEffects;

        public static EffectManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Preload()
        {
            SimplePool.PoolPreLoad(ballPopEffect, 20, EffectContainer.Transform);
            SimplePool.PoolPreLoad(textPopEffect, 20, EffectContainer.Transform);

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
            return SimplePool.Spawn(textPopEffect, EffectContainer.Transform, position, rotation);
        }

        public GameObject SpawnBoosterEffect(EntityType color, Vector3 position, Quaternion rotation)
        {
            // ????????????????
            return SimplePool.Spawn(ballBoosterEffects[0], EffectContainer.Transform, position, rotation);
        }
    }
}
