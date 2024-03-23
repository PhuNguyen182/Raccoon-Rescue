using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajactoryLine : MonoBehaviour
{
    [SerializeField] private LineRenderer line;

    public void ShowPath(Vector3[] pathNodes)
    {
        if (pathNodes.Length <= 0 || pathNodes == null)
        {
            line.positionCount = 0;
            return;
        }

        line.positionCount = pathNodes.Length;
        line.SetPositions(pathNodes);
    }

    public void HidePath()
    {
        line.positionCount = 0;
    }
}
