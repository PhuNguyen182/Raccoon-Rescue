using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer primaryLine;
    [SerializeField] private LineRenderer secondaryLine;
    [SerializeField] private Material lineMaterial;

    private readonly int _lineColorProperty = Shader.PropertyToID("_LineColor");

    public void SetColor(Color color)
    {
        lineMaterial.SetColor(_lineColorProperty, color);
    }

    public void ShowPath(Vector3[] pathNodes)
    {
        if (pathNodes.Length <= 0 || pathNodes == null)
        {
            HidePath();
            return;
        }

        else if (pathNodes.Length == 2)
        {
            primaryLine.positionCount = 2;
            secondaryLine.positionCount = 0;
            primaryLine.SetPositions(pathNodes);
        }

        else if (pathNodes.Length == 3)
        {
            primaryLine.positionCount = 2;
            secondaryLine.positionCount = 2;

            primaryLine.SetPositions(new Vector3[] { pathNodes[0], pathNodes[1] });
            secondaryLine.SetPositions(new Vector3[] { pathNodes[1], pathNodes[2] });
        }
    }

    public void HidePath()
    {
        primaryLine.positionCount = 0;
        secondaryLine.positionCount = 0;
        SetColor(Color.white);
    }

    private void OnDestroy()
    {
        SetColor(Color.white);
    }
}
