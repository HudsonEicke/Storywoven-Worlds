using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Speed of the platform")]
    public float moveSpeed = 2f;
    [Space]
    [Header("Insert points in the order you want the platform to move")]
    public List<Transform> points = new List<Transform>();

    private bool forward = true;
    private int nextPointIdx = 0;
    private Vector3 nextPos;
    private Vector3 previousPosition;
    private Vector3 platformVelocity;

    private void Start()
    {
        nextPos = points[0].position;
        previousPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);

        platformVelocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;


        if (transform.position == nextPos)
        {
            GetNextPos();
        }
    }

    private void GetNextPos()
    {
        if(forward)
        {
            nextPos = points[++nextPointIdx].position;

            if(nextPointIdx == points.Count - 1)
            {
                forward = false;
            }
        }
        else
        {
            nextPos = points[--nextPointIdx].position;

            if (nextPointIdx == 0)
            {
                forward = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!points.Contains(null))
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                Gizmos.DrawLine(points[i].transform.position, points[i + 1].transform.position);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<ThirdPersonMovement>().MoveWithPlatform(platformVelocity);
        }
    }
}
