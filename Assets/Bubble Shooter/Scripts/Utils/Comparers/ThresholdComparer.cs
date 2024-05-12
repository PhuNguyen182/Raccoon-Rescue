using System;
using System.Collections;
using System.Collections.Generic;
using BubbleShooter.Scripts.Common.PlayDatas;
using UnityEngine;

namespace BubbleShooter.Scripts.Utils.Comparers
{
    public class ThresholdComparer : IComparer<ThresholdData>
    {
        public int Compare(ThresholdData x, ThresholdData y)
        {
            return x.Position.y.CompareTo(y.Position.y);
        }
    }
}
