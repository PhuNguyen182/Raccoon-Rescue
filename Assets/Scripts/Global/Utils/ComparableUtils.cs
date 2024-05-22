using System;
using System.Collections;

public static class ComparableUtils
{
    public static bool IsInRange<T>(T value, Range<T> range) where T : IComparable<T>
    {
        int min = Comparer.Default.Compare(value, range.MinValue);
        int max = Comparer.Default.Compare(value, range.MaxValue);
        return min >= 0 && max <= 0;
    }
}
