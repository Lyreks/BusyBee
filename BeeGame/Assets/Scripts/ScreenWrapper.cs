using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    public Camera camera;

    private bool isWrappingX = false;
    private bool isWrappingY = false;

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
        //bool isVisible = WithinViewport();
        Vector3 newPosition;

        //if (isVisible)
        //{
        //    isWrappingX = false;
        //    isWrappingY = false;
        //    return;
        //}

        //if (isWrappingX && isWrappingY)
        //{
        //    return;
        //}

        newPosition = transform.position;
        if (newPosition.x > xMax)
        {
            newPosition.x = xMin;
            //isWrappingX = true;
        }
        if (newPosition.x < xMin)
        {
            newPosition.x = xMax;
            //isWrappingX = true;
        }
        if (newPosition.y > yMax)
        {
            newPosition.y = yMin;
            //isWrappingY = true;
        }
        if (newPosition.y < yMin)
        {
            newPosition.y = yMax;
            //isWrappingY = true;
        }

        transform.position = newPosition;
    }

    bool WithinViewport()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        bool withinViewport = true;

        if (pos.x < 0.0) withinViewport = false;
        if (1.0 < pos.x) withinViewport = false;
        if (pos.y < 0.0) withinViewport = false;
        if (1.0 < pos.y) withinViewport = false;

        return withinViewport;
    }
}
