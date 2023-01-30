using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    enum State
    {
        Wandering = 0,
        Honing = 1,
        Gathering = 2
    }

    [SerializeField]
    private State currentState;

    // Resources
    public int pollenCount = 0;
    public int pollenCapacity = 50;
    public int nectarCount = 0;
    public int nectarCapacity = 25;

    private float lastGathered = 0f;
    private float gatherDelay = 5f;

    // Movement
    public float honingSmoothTime = 0.3f;
    public float honingSpeedMax = 1.0f;
    public float wanderingSpeed = 50f;
    public float wanderingMagnitudeMin = 0.25f;
    public float wanderingMagnitudeMax = 0.75f;
    public float wanderingIntervalMin = 1.0f;
    public float wanderingIntervalMax = 3.0f;

    private Vector2 circleCenter;
    private Vector2 translationFromParent;
    private Vector3 velocity = Vector3.zero;
    private int wanderingDirection;
    private float wanderingInterval;
    private float wanderingMagnitude;
    private float lastWanderingDirectionChange = 0;
    private float circleDistanceTolerance = 0.9f;

    // Other Objects
    private Flower flower;
    private GameObject circle;

    private void Start()
    {
        circle = transform.GetChild(0).gameObject;
        wanderingDirection = Random.Range(0f, 1f) >= 0.5f ? 1 : -1;
        UpdateCircle();
    }

    void Update()
    {
        float distanceToCurrentCircleCenter = Vector2.Distance((Vector2)transform.position, circleCenter);
        float lastDistanceToCircleCenter = Vector2.Distance(translationFromParent + (Vector2)transform.position, transform.position);

        // Circle lock
        circle.transform.position = circleCenter;
        if (distanceToCurrentCircleCenter * circleDistanceTolerance >= lastDistanceToCircleCenter ||
            distanceToCurrentCircleCenter - lastDistanceToCircleCenter > wanderingMagnitudeMax)
        {
            RehomeCircle();
        }

        // Conditions for new state
        if (currentState != State.Gathering)
        {
            if (Input.GetMouseButton(0))
            {
                setCurrentState(State.Honing);
            }
            else
            {
                setCurrentState(State.Wandering);
            }
        }
        else
        {
            if (pollenCount >= pollenCapacity && nectarCount >= nectarCapacity ||
                flower.IsDepleted())
            {
                flower.beeCount--;
                flower = null;
                setCurrentState(State.Wandering);
            }
        }

        // Actions for state
        switch (currentState) {
            case State.Wandering:
                MoveAlongCircle();
                break;
            case State.Honing:
                MoveToCursor();
                break;
            case State.Gathering:
                MoveToFlower();
                GatherResources();
                break;
            default:
                break;
        }

        // Trail
        if (currentState == State.Honing)
        {
            transform.GetComponent<TrailRenderer>().enabled = true;
        }
        else
        {
            transform.GetComponent<TrailRenderer>().enabled = false;
        }
    }

    void setCurrentState(State state)
    {
        if (!currentState.Equals(state))
        {
            UpdateCircle();
            currentState = state;
        }
    }

    void GatherResources()
    {
        if (Time.time - lastGathered > gatherDelay)
        {
            lastGathered = Time.time;
            pollenCount += flower.TakePollen(1);
            nectarCount += flower.TakeNectar(1);
        }
    }

    void MoveToFlower()
    {
        transform.position = Vector3.SmoothDamp(transform.position, flower.transform.position, ref velocity, honingSmoothTime, honingSpeedMax);
        //transform.up = flower.transform.position - transform.position;
    }

    void MoveToCursor()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //transform.position = Vector2.MoveTowards(transform.position, mousePosition, speed * Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, mousePosition, ref velocity, honingSmoothTime, honingSpeedMax);
        transform.up = mousePosition - (Vector2) transform.position;
    }

    void MoveAlongCircle()
    {
        transform.RotateAround(circle.transform.position, wanderingDirection * Vector3.back, wanderingSpeed * Time.deltaTime / wanderingMagnitude);
        transform.right = wanderingDirection * wanderingMagnitude * ( circleCenter - (Vector2) transform.position );

        if (Time.time - lastWanderingDirectionChange > wanderingInterval)
        {
            UpdateCircle();
        }

        translationFromParent = circleCenter - (Vector2)transform.position;
    }

    void UpdateCircle()
    {
        wanderingDirection *= -1;
        wanderingMagnitude = Random.Range(wanderingMagnitudeMin, wanderingMagnitudeMax);
        circleCenter = transform.position + wanderingDirection * wanderingMagnitude * transform.right;
        circle.transform.position = circleCenter;
        circle.transform.localScale = Vector3.one * wanderingMagnitude * 2 / transform.localScale.x;
        wanderingInterval = Random.Range(wanderingIntervalMin, wanderingIntervalMax);
        lastWanderingDirectionChange = Time.time;
    }

    void RehomeCircle()
    {
        circleCenter = (Vector2) transform.position + translationFromParent;
        translationFromParent = circleCenter - (Vector2)transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag) {
            case "Flower":
                flower = collision.GetComponent<Flower>();
                if (flower.IsGatherable())
                {
                    flower.beeCount++;
                    setCurrentState(State.Gathering);
                    Debug.Log($"Flower hit at ({flower.transform.position.x},{flower.transform.position.y}");
                }
                break;
            default:
                break;
        }
    }
}
