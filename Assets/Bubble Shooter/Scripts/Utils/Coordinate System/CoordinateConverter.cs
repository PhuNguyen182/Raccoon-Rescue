using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoordinateSystem
{
    public static class CoordinateConverter
    {
        public static Vector3 FromCartesianToHexagon(Vector3Int position, float size, HexagonType hexagonType)
        {
            bool shouldOffset;
            float x, y, offset;
            float witdh, height;
            float horizontalDistance, verticalDistance;

            Vector3 hexPosition = Vector3.zero;

            if (hexagonType == HexagonType.PointedTop)
            {
                height = size * 2f;
                witdh = Mathf.Sqrt(3f) * size;

                horizontalDistance = witdh;
                verticalDistance = height * 0.75f;
                shouldOffset = position.y % 2 == 0;
                offset = shouldOffset ? witdh / 2f : 0f;

                x = (horizontalDistance * position.x) + offset;
                y = verticalDistance * position.y;
                hexPosition = new Vector3(x, -y);
            }

            else if(hexagonType == HexagonType.FlatTop)
            {
                witdh = size * 2f;
                height = Mathf.Sqrt(3f) * size;

                verticalDistance = height;
                horizontalDistance = witdh * 0.75f;
                shouldOffset = position.x % 2 == 0;
                offset = shouldOffset ? height / 2f : 0f;

                x = horizontalDistance * position.x;
                y = (verticalDistance * position.y) - offset;
                hexPosition = new Vector3(x, -y);
            }

            return hexPosition;
        }

        public static Vector3 FromHexagonToCartesian(Vector3 position, float size, HexagonType hexagonType)
        {
            float x, y, offset;
            float witdh, height;
            float horizontalDistance, verticalDistance;
            Vector3 cubePosition = Vector3.zero;

            if(hexagonType == HexagonType.PointedTop)
            {
                witdh = Mathf.Sqrt(3f) * size;
                height = size * 2f;

                horizontalDistance = witdh;
                verticalDistance = height * 0.75f;

                y = -position.y / verticalDistance;
                offset = (int)y % 2 == 0 ? witdh / 2f : 0;
                x = (position.x - offset) / horizontalDistance;

                cubePosition = new Vector3(x, y);
            }

            else if(hexagonType == HexagonType.FlatTop)
            {
                witdh = size * 2f;
                height = Mathf.Sqrt(3f) * size;

                verticalDistance = height;
                horizontalDistance = witdh * 0.75f;

                x = position.x / horizontalDistance;
                offset = (int)x % 2 == 0 ? height / 2 : 0;
                y = (-position.y + offset) / verticalDistance;

                cubePosition = new Vector3(x, y);
            }

            return cubePosition;
        }
    }
}
