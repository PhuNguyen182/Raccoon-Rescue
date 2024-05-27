using System;

[Serializable]
public struct Range<T> where T : IComparable<T>
{
    public T MinValue; 
    public T MaxValue;
}
