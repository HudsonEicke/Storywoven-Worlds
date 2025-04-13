using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
 
    bool playerDetected = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            playerDetected = true;
            Debug.Log("Player detected!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            playerDetected = false;
            Debug.Log("Player left the area!");
        }
    }
}
