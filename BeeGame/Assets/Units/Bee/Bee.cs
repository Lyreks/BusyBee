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
    public float wanderingSpeed = 0.5f;

    private Vector2 circleCenter;
    private float circleRadius;
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
        circleCenter = transform.GetChild(1).position;
        circleRadius = transform.GetChild(1).localScale.x / 2;
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

        if ( distanceToCurrentCircleCenter * tolerance >= lastDistanceToCircleCenter)
        {
            RehomeCircle();
        }

        transform.GetChild(1).position = circleCenter;
        transform.RotateAround(transform.GetChild(1).position, direction * magnitude * Vector3.back, wanderingSpeed * Time.deltaTime * 100 / magnitude);
        transform.right = direction * magnitude * ( circleCenter - (Vector2) transform.position );

        if (Time.time - lastDirectionChange > interval) {
            UpdateCircle();
        }
    }

    void UpdateCircle()
    {
        direction = Random.Range( 0f, 1f ) >= 0.5f ? 1 : -1;
        magnitude = Random.Range(0.25f, 2f);
        circleCenter = transform.position + direction * magnitude * transform.right * circleRadius;
        interval = Random.Range(1f, 3f);
        lastDirectionChange = Time.time;
        translationFromParent = circleCenter - (Vector2) transform.position;
    }

    void RehomeCircle()
    {
        circleCenter = (Vector2) transform.position + translationFromParent;
    }
}
