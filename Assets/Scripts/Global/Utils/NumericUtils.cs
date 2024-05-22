using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NumericUtils
{
    public static bool IsInRange(int number, Range<int> range)
    {
        return number >= range.MinValue && number <= range.MaxValue;
    }

    public static bool IsInRange(float number, Range<float> range)
    {
        return number >= range.MinValue && number <= range.MaxValue;
    }
}
