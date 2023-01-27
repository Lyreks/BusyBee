using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    enum State
    {
        Idle = 0,
        Wandering = 1,
        Honing = 2,
        Gathering = 3
    }

    public float smoothTime = 0.3f;
    public float maxMoveSpeed = 1.0f;
    public float wanderingSpeed = 50f;
    public float magnitudeMin = 0.25f;
    public float magnitudeMax = 0.75f;
    public float intervalMin = 1.0f;
    public float intervalMax = 3.0f;

    private Vector2 circleCenter;
    private int direction;
    private float magnitude;
    private float interval;
    private float tolerance = 0.9f;
    private float lastDirectionChange;
    private Vector2 translationFromParent;


    private Vector3 velocity = Vector3.zero;
    private State currentState;

    private void Start()
    {
        direction = Random.Range(0f, 1f) >= 0.5f ? 1 : -1;
        UpdateCircle();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            setCurrentState(State.Honing);
        }
        else
        {
            setCurrentState(State.Wandering);
        }

        switch (currentState) {
            case State.Idle:
                break;
            case State.Wandering:
                MoveAlongCircle();
                break;
            case State.Honing:
                MoveToCursor();
                break;
            case State.Gathering:
                break;
            default:
                break;
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

    void MoveToCursor()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //transform.position = Vector2.MoveTowards(transform.position, mousePosition, speed * Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, mousePosition, ref velocity, smoothTime, maxMoveSpeed);
        transform.up = mousePosition - (Vector2) transform.position;
    }

    void MoveAlongCircle()
    {
        float distanceToCurrentCircleCenter = Vector2.Distance((Vector2)transform.position, circleCenter);
        float lastDistanceToCircleCenter = Vector2.Distance(translationFromParent + (Vector2)transform.position, transform.position);

        if (distanceToCurrentCircleCenter * tolerance >= lastDistanceToCircleCenter ||
            distanceToCurrentCircleCenter - lastDistanceToCircleCenter > magnitudeMax)
        {
            RehomeCircle();
        }

        transform.GetChild(0).position = circleCenter;
        transform.RotateAround(transform.GetChild(0).position, direction * Vector3.back, wanderingSpeed * Time.deltaTime / magnitude);
        transform.right = direction * magnitude * ( circleCenter - (Vector2) transform.position );

        if (Time.time - lastDirectionChange > interval)
        {
            UpdateCircle();
        }

        translationFromParent = circleCenter - (Vector2)transform.position;
    }

    void UpdateCircle()
    {
        direction *= -1;
        magnitude = Random.Range(magnitudeMin, magnitudeMax);
        circleCenter = transform.position + direction * magnitude * transform.right;
        transform.GetChild(0).position = circleCenter;
        transform.GetChild(0).localScale = Vector3.one * magnitude * 2 / transform.localScale.x;
        interval = Random.Range(intervalMin, intervalMax);
        lastDirectionChange = Time.time;
    }

    void RehomeCircle()
    {
        circleCenter = (Vector2) transform.position + translationFromParent;
        translationFromParent = circleCenter - (Vector2)transform.position;
    }
}
