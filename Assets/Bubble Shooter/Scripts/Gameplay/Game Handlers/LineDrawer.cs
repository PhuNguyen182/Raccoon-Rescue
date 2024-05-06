using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer line;

    public void ShowPath(Vector3[] pathNodes)
    {
        if (pathNodes.Length <= 0 || pathNodes == null)
        {
            HidePath();
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
