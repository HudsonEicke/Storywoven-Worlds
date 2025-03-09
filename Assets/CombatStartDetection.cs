using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStartDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager3D.Instance.StartBattle();
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
