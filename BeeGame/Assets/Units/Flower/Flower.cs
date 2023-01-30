using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flower : MonoBehaviour
{
    enum State
    {
        Closed = 0,
        Open = 1
    }

    State currentState;

    public int pollenCount = 10;
    public int pollenCapacity = 10;
    public int nectarCount = 10;
    public int nectarCapacity = 10;
    public int beeCapacity = 1;
    public int beeCount = 0;

    private float timeClosed = 0;
    private float closedDuration = 10;

    // Sprite
    public Sprite flowerOpen;
    public Sprite flowerClosed;

    private void Start()
    {
        currentState = State.Closed;
    }

    private void Update()
    {
        if (currentState == State.Open && pollenCount <= 0 && nectarCount <= 0)
        {
            currentState = State.Closed;
            timeClosed = Time.time;
        }

        switch (currentState)
        {
            case State.Closed:
                gameObject.GetComponent<SpriteRenderer>().sprite = flowerClosed;
                RefillResources();
                break;
            case State.Open:
                gameObject.GetComponent<SpriteRenderer>().sprite = flowerOpen;
                break;
        }
    }

    private void RefillResources()
    {
        if (Time.time - timeClosed > closedDuration)
        {
            pollenCount = pollenCapacity;
            nectarCount = nectarCapacity;
            currentState = State.Open;
        }
    }

    public bool IsGatherable()
    {
        return !IsDepleted() && beeCount < beeCapacity;
    }

    public bool IsDepleted()
    {
        return pollenCount <= 0 && nectarCount <= 0;
    }

    public int TakePollen(int requestAmount)
    {
        var giveAmount = 0;

        if (requestAmount > pollenCount)
        {
            giveAmount = pollenCount;
            pollenCount = 0;
        }
        else
        {
            giveAmount = requestAmount;
            pollenCount -= requestAmount;
        }

        return giveAmount;
    }

    public int TakeNectar(int requestAmount)
    {
        var giveAmount = 0;

        if (requestAmount > nectarCount)
        {
            giveAmount = nectarCount;
            nectarCount = 0;
        }
        else
        {
            giveAmount = requestAmount;
            nectarCount -= requestAmount;
        }

        return giveAmount;
    }
}
