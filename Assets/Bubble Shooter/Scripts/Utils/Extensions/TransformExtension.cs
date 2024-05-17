using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Utils.Extentions
{
    public static class TransformExtension
    {
        public static void SetTR(this Transform transform, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = Vector3.one;
            transform.SetParent(parent);
        }

        public static void SetTR(this Transform transform, Vector3 position, Vector3 rotation, Transform parent = null)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(rotation);
            transform.localScale = Vector3.one;
            transform.SetParent(parent);
        }

        public static void SetTRS(this Transform transform, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
            transform.SetParent(parent);
        }

        public static void SetTRS(this Transform transform, Vector3 position, Vector3 rotation, Vector3 scale, Transform parent = null)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(rotation);
            transform.localScale = scale;
            transform.SetParent(parent);
        }

        public static bool IsCloseTo(this Transform transform, Transform point, float closeDistance)
        {
            return Vector3.SqrMagnitude(point.position - transform.position) < closeDistance * closeDistance;
        }

        public static bool IsCloseTo(this Transform transform, Vector3 position, float closeDistance)
        {
            return Vector3.SqrMagnitude(position - transform.position) < closeDistance * closeDistance;
        }
    }
}
