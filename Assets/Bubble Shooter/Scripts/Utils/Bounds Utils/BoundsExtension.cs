using BubbleShooter.Scripts.Utils.Comparers;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BubbleShooter.Scripts.Utils.BoundsUtils
{
    public static class BoundsExtension
    {
        public static BoundsInt GetBounds2D(this Vector3Int position, int range)
        {
            return new BoundsInt
            {
                xMin = position.x - range / 2,
                xMax = position.x + range / 2,
                yMin = position.y - range / 2,
                yMax = position.y + range / 2,
            };
        }

        public static BoundsInt Expand2D(this BoundsInt boundsInt, int range)
        {
            BoundsInt bounds = new BoundsInt
            {
                position = boundsInt.position,
                size = new Vector3Int(range, range)
            };

            return bounds;
        }

        public static BoundsInt Encapsulate(List<Vector3Int> positions)
        {
            List<Vector3Int> sortedPosition = new(positions);
            sortedPosition.Sort(new Vector3IntComparer());

            Vector3Int firstPosition = sortedPosition.First();
            Vector3Int lastPosition = sortedPosition.Last();

            return new BoundsInt
            {
                xMin = firstPosition.x,
                xMax = lastPosition.x,
                yMin = firstPosition.y,
                yMax = lastPosition.y
            };
        }

        public static IEnumerable<Vector3Int> GetRow(this BoundsInt boundsInt, Vector3Int checkPosition)
        {
            for (int x = boundsInt.xMin; x <= boundsInt.xMax; x++)
            {
                yield return new Vector3Int(x, checkPosition.y);
            }
        }

        public static IEnumerable<Vector3Int> GetColumn(this BoundsInt boundsInt, Vector3Int checkPosition)
        {
            for (int y = boundsInt.yMin; y <= boundsInt.yMax; y++)
            {
                yield return new Vector3Int(checkPosition.x, y);
            }
        }

        public static void ForEach(this BoundsInt boundsInt, Action<Vector3Int> callback)
        {
            for (int x = boundsInt.xMin; x <= boundsInt.xMax; x++)
            {
                for (int y = boundsInt.yMin; y <= boundsInt.yMax; y++)
                {
                    callback.Invoke(new Vector3Int(x, y));
                }
            }
        }

        public static IEnumerable<Vector3Int> Iterator(this BoundsInt boundsInt)
        {
            for (int x = boundsInt.xMin; x <= boundsInt.xMax; x++)
            {
                for (int y = boundsInt.yMin; y <= boundsInt.yMax; y++)
                {
                    yield return new Vector3Int(x, y);
                }
            }
        }
    }
}
