using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class Draw : MonoBehaviour
{
    public Camera cam;
    public RectTransform drawArea;
    public LineRenderer linePrefab;

    private LineRenderer currentLine;
    private TouchInputActions touchInputActions;

    private void Awake()
    {
        touchInputActions = new TouchInputActions();
        touchInputActions.Touch.Enable();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        touchInputActions.Touch.PrimaryTouch.performed += OnTouch;
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        touchInputActions.Touch.PrimaryTouch.performed -= OnTouch;
    }

    private void OnTouch(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = context.ReadValue<Vector2>();

        Debug.Log(touchPosition);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(drawArea, touchPosition, cam, out Vector2 localPoint))
        {
            Vector3 worldPosition = cam.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, cam.nearClipPlane));

            var t = Touch.activeTouches[0].phase;
            TouchPhase touchPhase = t;
            switch (touchPhase)
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                    StartDrawing(worldPosition);
                    break;
                case UnityEngine.InputSystem.TouchPhase.Moved:
                    DrawLine(worldPosition);
                    break;
                case UnityEngine.InputSystem.TouchPhase.Ended:
                    EndDrawing();
                    break;
            }
        }
    }

    private void StartDrawing(Vector3 startPoint)
    {
        currentLine = Instantiate(linePrefab, transform);
        currentLine.positionCount = 1;
        currentLine.SetPosition(0, startPoint);
    }

    private void DrawLine(Vector3 newPoint)
    {
        if (currentLine != null)
        {
            currentLine.positionCount++;
            currentLine.SetPosition(currentLine.positionCount - 1, newPoint);
        }
    }

    private void EndDrawing()
    {
        currentLine = null;
    }
}
