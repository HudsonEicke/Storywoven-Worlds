using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterDisplayMessage : MonoBehaviour
{
    public bool destroyOnInteract = false;
    public string message;
    public float timeToDisplay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ImportantComponentsManager.Instance.dialogueBox.DisplayText(message, timeToDisplay);

            if(destroyOnInteract)
                Destroy(gameObject);
        }
    }
}
