using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.Miscs;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.GameUI.IngameBooster
{
    public class BoosterPanel : MonoBehaviour
    {
        [Header("Dummy Balls")]
        [SerializeField] private DummyBall blue;
        [SerializeField] private DummyBall green;
        [SerializeField] private DummyBall orange;
        [SerializeField] private DummyBall red;
        [SerializeField] private DummyBall violet;
        [SerializeField] private DummyBall yellow;
        [SerializeField] private DummyBall colorfulBall;

        [Header("Booster Buttons")]
        [SerializeField] private BoosterButton colorfulBooster;
        [SerializeField] private BoosterButton aimingBooster;
        [SerializeField] private BoosterButton changeBallBooster;

        private const string UIObjectsLayer = "UIObjects";
        private const string ObjectsLayer = "Objects";

        public BoosterButton Colorful => colorfulBooster;
        public BoosterButton Aiming => aimingBooster;
        public BoosterButton ChangeBall => changeBallBooster;

        public async UniTask SpawnColorful(Vector3 toPosition)
        {
            var ball = SpawnBall(EntityType.ColorfulBall, colorfulBooster.transform);
            ball.ChangeLayer(UIObjectsLayer);
            await ball.MoveTo(toPosition);
            ball.ChangeLayer(ObjectsLayer);
            SimplePool.Despawn(ball.gameObject);
        }

        public async UniTask SpawnColorBall(EntityType color, Vector3 toPosition)
        {
            var ball = SpawnBall(color, changeBallBooster.transform);
            ball.ChangeLayer(UIObjectsLayer);
            await ball.MoveTo(toPosition);
            ball.ChangeLayer(ObjectsLayer);
            SimplePool.Despawn(ball.gameObject);
        }

        private DummyBall SpawnBall(EntityType color, Transform point)
        {
            DummyBall ballPrefab = color switch
            {
                EntityType.Red => red,
                EntityType.Yellow => yellow,
                EntityType.Green => green,
                EntityType.Blue => blue,
                EntityType.Violet => violet,
                EntityType.Orange => orange,
                EntityType.ColorfulBall => colorfulBall,
                _ => null
            };

            return SimplePool.Spawn(ballPrefab, DummyBallContainer.Transform
                                    , point.position, Quaternion.identity);
        }
    }
}
