using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject bee;
    public GameObject flower;

    private Vector3 mousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(bee, new Vector3(mousePosition.x, mousePosition.y, 0), Quaternion.identity);
        }
    }
}
