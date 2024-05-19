using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Dreamteck.Splines;

namespace BubbleShooter.Scripts.Mainhome.ProgressMap
{
    public class PathFollower : MonoBehaviour
    {
        [SerializeField] private float moveDuration = 1f;
        [SerializeField] private Vector2 offset = new(0, 2.7f);
        [SerializeField] private SplineComputer splineComputer;

        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        public async UniTaskVoid Move(int startIndex, int endIndex, float duration)
        {
            float elapsedTime = 0;
            float progressPercent;
            float startPercent = (float)splineComputer.GetPointPercent(startIndex);
            float endPercent = (float)splineComputer.GetPointPercent(endIndex);
            Vector2 progressPosition;

            while (elapsedTime < duration)
            {
                progressPercent = Mathf.Lerp(startPercent, endPercent, elapsedTime / duration);
                progressPosition = splineComputer.EvaluatePosition(progressPercent);
                transform.position = progressPosition + offset;

                elapsedTime += Time.deltaTime;
                await UniTask.NextFrame(_token);
            }
        }

        public void SetPositionImediately(int index)
        {
            SplinePoint point = splineComputer.GetPoint(index);
            Vector2 position = point.position;
            transform.position = position + offset;
        }
    }
}
