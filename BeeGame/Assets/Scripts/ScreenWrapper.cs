using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    public Camera camera;

    private float xMin = -1.78f;
    private float xMax = 1.78f;
    private float yMin = -1.00f;
    private float yMax = 1.00f;

    void Update()
    {
        ScreenWrap();
    }

    void ScreenWrap()
    {
        Vector3 newPosition;

        newPosition = transform.position;
        if (newPosition.x > xMax)
        {
            newPosition.x = xMin;
        }
        if (newPosition.x < xMin)
        {
            newPosition.x = xMax;
        }
        if (newPosition.y > yMax)
        {
            newPosition.y = yMin;
        }
        if (newPosition.y < yMin)
        {
            newPosition.y = yMax;
        }

        transform.position = newPosition;
    }
}
