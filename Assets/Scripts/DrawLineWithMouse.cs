using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineWithMouse : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 previousPosition;

    [SerializeField] private float minDistance = 0.1f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0;

            if (Vector3.Distance(currentPosition, previousPosition) > minDistance)
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition);
                previousPosition = currentPosition;
            }
        }
    }
}
