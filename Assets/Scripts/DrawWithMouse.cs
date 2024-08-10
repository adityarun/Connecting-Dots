using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    [SerializeField] private float minDistance = 0.1f;
    [SerializeField, Range(0.1f,0.5f)] private float width;

    private LineRenderer lineRenderer;
    private Vector3 previousPosition;
    private bool IsDrawing;
    private LineRenderer line;
    private string selectedColor = "";
        
    private enum colorSelected
    {
        Red = 1,
        Green = 2,
        Blue = 3,
        Yellow = 4,
        Pink = 5
    }
    private colorSelected currentColor = colorSelected.Red;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                if(selectedColor == "")
                {
                    string[] s = hit.collider.gameObject.name.Split('_');
                    selectedColor = s[0];
                    currentColor = (colorSelected) Enum.Parse(typeof(colorSelected), s[0]);
                    LineRenderer newline = lineRenderer;
                    newline.positionCount = 1;
                    previousPosition = transform.position;
                    newline.startWidth = newline.endWidth = width;
                    newline.material = new Material(Shader.Find("Sprites/Default"));
                    newline.SetColors(SetColor((int)currentColor), SetColor((int)currentColor));
                    IsDrawing = true;
                    line = newline;
                }
                else
                {
                    selectedColor = "";
                    IsDrawing = false;
                }
                Debug.Log(selectedColor);
            }

            if (IsDrawing && line != null)
            {
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentPosition.z = 0f;

                if (Vector3.Distance(currentPosition, previousPosition) > minDistance)
                {
                    if (previousPosition == transform.position)
                    {
                        line.SetPosition(0, currentPosition);
                        line.enabled = true;
                    }
                    else
                    {
                        line.positionCount++;
                        line.SetPosition(line.positionCount - 1, currentPosition);
                    }
                    previousPosition = currentPosition;
                }
            }
        }
    }

    private Color SetColor(int index)
    {
        Color color = new Color();
        switch(index)
        {
            case 1:color = Color.red; break;
            case 2: color = Color.green; break;
            case 3: color = Color.blue; break;
            case 4: color = Color.yellow; break;
            case 5: color = new Color(255, 192, 203);break;
            default: color = Color.white; break;
        }
        return color;
    }
}
