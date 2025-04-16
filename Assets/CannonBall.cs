using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float moveSpeed = 20f;
    public bool canStartMoving = false;
    private bool firstRun = true;
    public Transform firstPoint;
    public Transform secondPoint;
    private Vector3 nextPos;
    public int currentTarget = 0;
    public BossStageProgressor progressor;

    private void Update()
    {
        if (!canStartMoving)
            return;

        if (firstRun)
        {
            nextPos = firstPoint.position;
            firstRun = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, nextPos) < 0.01f)
        {
            currentTarget += 1;
            nextPos = secondPoint.position;
        }
    }

    private void OnDestroy()
    {
        progressor.DoneProgressing();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");
        if (other.gameObject.CompareTag("Boss"))
        {
            other.gameObject.GetComponent<BossHealth>().Hit();
            Destroy(gameObject);
        }
    }
}
