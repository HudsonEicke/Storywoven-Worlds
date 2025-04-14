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
    [Space]
    [Header("If you want the platform after hitting the last point to go to the first")]
    public bool cycle = false;

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


        if (Vector3.Distance(transform.position, nextPos) < 0.01f)
        {
            if (cycle)
            {
                GetNextPosCycle();
            }
            else
            {
                GetNextPos();
            }
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

    private void GetNextPosCycle()
    {
        nextPointIdx = (nextPointIdx + 1) % points.Count;
        nextPos = points[nextPointIdx].position;
    }

    private void OnDrawGizmos()
    {
        if (!points.Contains(null))
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                Gizmos.DrawLine(points[i].transform.position, points[i + 1].transform.position);
            }

            if(cycle)
            {
                Gizmos.DrawLine(points[points.Count - 1].position, points[0].position);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ImportantComponentsManager.Instance.thirdPersonMovement.MoveWithPlatform(platformVelocity);
        }
    }
}
