using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStartDetection : MonoBehaviour
{
    public Collider detection;
    public int enemyCount = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager3D.Instance.StartBattle(enemyCount);
            Destroy(gameObject.transform.parent.gameObject);
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
