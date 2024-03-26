using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Utils.Comparers
{
    public class Vector3IntComparer : IComparer<Vector3Int>
    {
        public int Compare(Vector3Int a, Vector3Int b)
        {
            return a.x.CompareTo(b.x) + a.y.CompareTo(b.y);
        }
    }
}
