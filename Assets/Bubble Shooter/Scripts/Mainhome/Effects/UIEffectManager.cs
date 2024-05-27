using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BubbleShooter.Scripts.Feedbacks;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Mainhome.Effects
{
    public class UIEffectManager : MonoBehaviour
    {
        [SerializeField] private UIObjectEffect coinPrefab;

        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        public async UniTask SpawnFlyCoin()
        {
            Vector3 toPos = MainhomeController.Instance.Coin.Position;

            using (var listPool = ListPool<UIObjectEffect>.Get(out var coins))
            {
                for (int i = 0; i < 10; i++)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: _token);
                    Vector2 point = Random.insideUnitCircle * 1f;
                    var coin = SimplePool.Spawn(coinPrefab, UIEffectContainer.Transform
                                                , point, Quaternion.identity);

                    coin.transform.localScale = Vector3.one;
                    coin.SetFade(0);
                    coin.FadeIn(0.3f).Forget();
                    coins.Add(coin);
                }

                for (int i = 0; i < 10; i++)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: _token);
                    coins[i].MoveTo(toPos, () => SimplePool.Despawn(coins[i].gameObject)).Forget();
                    
                    if (i == 4)
                        Emittable.Default.Emit("CoinHolder").Forget();
                }
            }
        }
    }
}
