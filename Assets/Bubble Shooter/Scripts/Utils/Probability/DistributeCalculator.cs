using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DistributeCalculator
{
    public static float GetPercentage(int number, List<int> collection)
    {
        float sum = 0;

        for (int i = 0; i < collection.Count; i++)
        {
            sum = sum + collection[i];
        }

        return 1.0f * number / sum;
    }
}
