using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformActivationBased : MonoBehaviour
{
    [Header("Speed of the platform")]
    public float moveSpeed = 2f;
    [Space]
    [Header("Insert points in the order you want the platform to move")]
    public Transform nextDest = null;

    private bool forward = true;
    private int nextPointIdx = 0;
    private Vector3 nextPos;
    private Vector3 previousPosition;
    private Vector3 platformVelocity;

    public bool triggerBased = false;

    public bool atDest = true;

    private float timeRemaining = 0;

    private void Start()
    {
        nextPos = transform.position;
        previousPosition = transform.position;
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            if (!atDest)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);

                platformVelocity = (transform.position - previousPosition) / Time.deltaTime;
                previousPosition = transform.position;
            }

            if (transform.position == nextPos)
            {
                atDest = true;
            }
        }
    }

    private void GetNextPos()
    {
        nextPos = nextDest.position;

        atDest = false;
    }

    public void newPos(Transform newDest, float timeBeforeMove)
    {
        nextPos = newDest.position;
        atDest = false;
        timeRemaining = timeBeforeMove;
    }

    private void OnDrawGizmos()
    {
        if (nextDest != null)
        {
            Gizmos.DrawLine(transform.position, nextDest.transform.position);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!atDest && other.CompareTag("Player"))
        {
            ImportantComponentsManager.Instance.thirdPersonMovement.MoveWithPlatform(platformVelocity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerBased && other.CompareTag("Player"))
        {
            GetNextPos();
        }
    }
}
