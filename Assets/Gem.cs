using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int gemID;
    public string gemMessage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ImportantComponentsManager.Instance.powerupManager.GivePowerup(gemID, gemMessage);
        }
    }
}
