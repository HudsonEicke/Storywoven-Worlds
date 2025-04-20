using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStartDetection : MonoBehaviour
{
    public Collider detection;
    public int enemyCount = 2;
    public bool setAmount = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!setAmount)
            {
                enemyCount = Random.Range(1, 4);
            }

            ImportantComponentsManager.Instance.thirdPersonMovement.QueueCombat(enemyCount, gameObject.transform.parent.gameObject);
        }
    }

    public void shutDownCombatDetection()
    {
        detection.enabled = false;
    }

    public void startDownCombatDetection()
    {
        detection.enabled = true;
    }
}
