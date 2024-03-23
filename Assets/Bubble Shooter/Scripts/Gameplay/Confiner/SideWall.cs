using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Gameplay.Confiner
{
    public class SideWall : MonoBehaviour
    {
        [SerializeField] private WallSide wallSide;
        [SerializeField] private Collider2D wallCollider;
        [SerializeField] private Camera mainCamera;

        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();

            wallCollider.enabled = false;
            RestrictPosition();
        }

        private void Start()
        {
            ControllWallCollider().Forget();
        }

        public async UniTaskVoid ControllWallCollider()
        {
            wallCollider.enabled = false;
            await UniTask.DelayFrame(2, PlayerLoopTiming.Update, _token);
            wallCollider.enabled = true;
        }

        private void RestrictPosition()
        {
            Vector3 position = wallSide == WallSide.Left ? mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f))
                                                         : mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f));
            position.z = 0;
            transform.position = position;
        }
    }
}
