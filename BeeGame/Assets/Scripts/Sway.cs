using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Sway : MonoBehaviour
{
    //public float speed = 1f;
    //public float angle = 1f;
    public Transform from;
    public Transform to;
    public float speed = 0.01f;
    public float timeCount = 0.0f;


    void Update()
    {
        //transform.localEulerAngles = new Vector3(0, 0, Mathf.PingPong(Time.time * speed, angle) - angle / 2);

        transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, timeCount * speed);
        timeCount += Time.deltaTime;
    }
}
