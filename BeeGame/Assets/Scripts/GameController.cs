using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    public GameObject UICanvas;
    public GameObject bee;
    public GameObject flower;

    public int beeCount = 1;
    public int pollenCount = 0;
    public int nectarCount = 0;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void SpawnBeeAtCenter()
    {
        Instantiate(bee, Vector3.zero, Quaternion.identity);
        beeCount++;
    }
}