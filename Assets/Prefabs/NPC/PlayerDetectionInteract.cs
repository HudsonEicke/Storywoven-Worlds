using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetectionInteract : MonoBehaviour
{
    public GameObject interactButton;
    public GameObject dialogueStuff;
    public GameObject Choices;
    private bool playerDetected = false;
    private DTLoad dialogueTrigger;

    public void Start()
    {  
        dialogueTrigger = GetComponent<DTLoad>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerDetected = true;
            if (interactButton != null)
            {
                interactButton.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Interact button not assigned");
            }
            Debug.Log("Player detected!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            playerDetected = false;
            if (interactButton != null)
            {
                interactButton.SetActive(false);
            }
            if (dialogueStuff != null)
            {
                dialogueStuff.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Interact button not assigned");
            }
            if (Choices != null)
            {
                Choices.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Choices not assigned");
            }
            if (dialogueTrigger != null)
            {
                dialogueTrigger.EndDialogue();
            }
            else
            {
                Debug.LogWarning("DialogueTrigger not assigned");
            }
            Debug.Log("Player left the area!");
        }
    }

    private void Update()
    {
        if (playerDetected && Input.GetKeyDown(KeyCode.F))
        {
            if (interactButton != null)
            {
                interactButton.SetActive(false);

            }
            else
            {
                Debug.LogWarning("Interact button not assigned");
            }

            if (dialogueStuff != null)
                {
                    dialogueStuff.SetActive(true);
                }
            else
            {
                Debug.LogWarning("Dialogue stuff not assigned");
            }

            if(dialogueTrigger != null)
            {
                dialogueTrigger.TriggerDialogue();
            }
            else
            {
                Debug.LogWarning("DialogueTrigger not assigned");
            }

            // Call the method to trigger the dialogue or any other action
            Debug.Log("Inteacted with F");

        }
    }
}
