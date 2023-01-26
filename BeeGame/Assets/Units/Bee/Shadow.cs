using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public float offset = 0.1f;

    void Update()
    {
        transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y - offset);
    }
}
