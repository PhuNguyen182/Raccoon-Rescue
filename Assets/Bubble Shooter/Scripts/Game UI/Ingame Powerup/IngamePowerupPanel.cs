using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameHandlers;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.GameUI.IngamePowerup
{
    public class IngamePowerupPanel : MonoBehaviour
    {
        [SerializeField] private ParticleSystem followParticle;
        [SerializeField] private BallShooter ballShooter;

        [Header("Dummy Balls")]
        [SerializeField] private DummyBall fireBall;
        [SerializeField] private DummyBall leafBall;
        [SerializeField] private DummyBall sunBall;
        [SerializeField] private DummyBall waterBall;

        [Header("Powerup Buttons")]
        [SerializeField] private PowerupButton fireballButton;
        [SerializeField] private PowerupButton leafballButton;
        [SerializeField] private PowerupButton sunballButton;
        [SerializeField] private PowerupButton waterballButton;

        private const string UIObjectsLayer = "UIObjects";
        private const string ObjectsLayer = "Objects";

        public void ControlPowerupButtons(float fillAmount, EntityType powerupType)
        {
            switch (powerupType)
            {
                case EntityType.FireBall:
                    fireballButton.SetFillAmount(fillAmount);
                    break;
                case EntityType.LeafBall:
                    leafballButton.SetFillAmount(fillAmount);
                    break;
                case EntityType.WaterBall:
                    waterballButton.SetFillAmount(fillAmount);
                    break;
                case EntityType.SunBall:
                    sunballButton.SetFillAmount(fillAmount);
                    break;
            }
        }

        public async UniTask SpawnPowerup(EntityType powerupType)
        {
            switch (powerupType)
            {
                case EntityType.FireBall:
                    await SpawnFireBall();
                    break;
                case EntityType.LeafBall:
                    await SpawnLeafBall();
                    break;
                case EntityType.WaterBall:
                    await SpawnWaterBall();
                    break;
                case EntityType.SunBall:
                    await SpawnSunBall();
                    break;
            }
        }

        private async UniTask SpawnFireBall()
        {
            DummyBall ball = SimplePool.Spawn(fireBall, DummyBallContainer.Transform
                                              , fireballButton.transform.position, Quaternion.identity);
            var particle = SimplePool.Spawn(followParticle, ball.transform, ball.transform.position, Quaternion.identity);

            ball.ToggleEffect(false);
            ball.ChangeLayer(UIObjectsLayer);
            await ball.SwapTo(ballShooter.ShotPoint.position);
            ball.ChangeLayer(ObjectsLayer);
            particle.transform.SetParent(EffectContainer.Transform);
            SimplePool.Despawn(ball.gameObject);
        }

        private async UniTask SpawnLeafBall()
        {
            DummyBall ball = SimplePool.Spawn(leafBall, DummyBallContainer.Transform
                                              , leafballButton.transform.position, Quaternion.identity);
            var particle = SimplePool.Spawn(followParticle, ball.transform, ball.transform.position, Quaternion.identity);

            ball.ToggleEffect(false);
            ball.ChangeLayer(UIObjectsLayer);
            await ball.SwapTo(ballShooter.ShotPoint.position);
            ball.ChangeLayer(ObjectsLayer);
            particle.transform.SetParent(EffectContainer.Transform);
            SimplePool.Despawn(ball.gameObject);
        }

        private async UniTask SpawnSunBall()
        {
            DummyBall ball = SimplePool.Spawn(sunBall, DummyBallContainer.Transform
                                              , sunballButton.transform.position, Quaternion.identity);
            var particle = SimplePool.Spawn(followParticle, ball.transform, ball.transform.position, Quaternion.identity);

            ball.ToggleEffect(false);
            ball.ChangeLayer(UIObjectsLayer);
            await ball.SwapTo(ballShooter.ShotPoint.position);
            ball.ChangeLayer(ObjectsLayer);
            particle.transform.SetParent(EffectContainer.Transform);
            SimplePool.Despawn(ball.gameObject);
        }

        private async UniTask SpawnWaterBall()
        {
            DummyBall ball = SimplePool.Spawn(waterBall, DummyBallContainer.Transform
                                              , waterballButton.transform.position, Quaternion.identity);
            var particle = SimplePool.Spawn(followParticle, ball.transform, ball.transform.position, Quaternion.identity);

            ball.ToggleEffect(false);
            ball.ChangeLayer(UIObjectsLayer);
            await ball.SwapTo(ballShooter.ShotPoint.position);
            ball.ChangeLayer(ObjectsLayer);
            particle.transform.SetParent(EffectContainer.Transform);
            SimplePool.Despawn(ball.gameObject);
        }
    }
}
