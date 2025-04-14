using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float moveSpeed = 20f;
    public bool canStartMoving = false;
    private bool firstRun = true;
    public Transform firstPoint;
    public Transform secondPoint;
    private Vector3 nextPos;
    public int currentTarget = 0;

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

        if (currentTarget >= 2)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            ImportantComponentsManager.Instance.thirdPersonMovement.moveBack = true;
            ImportantComponentsManager.Instance.thirdPersonMovement.playerHealthController.Damage(1);
        }
    }
}
