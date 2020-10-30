using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Vector3[] positions = new Vector3[3] { new Vector3(0, 0, 0), new Vector3(-5, 5, 0), new Vector3(5, 5, 0) };
        DrawTriangle(positions);
    }

    void DrawTriangle(Vector3[] vertexPositions)
    {

        lineRenderer.positionCount = 3;
        lineRenderer.SetPositions(vertexPositions);
    }
}
