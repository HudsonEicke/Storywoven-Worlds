using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DTLoad : MonoBehaviour
{
    public Dialogue1[] dialogues;
    public Actor1[] characters;
    
    public void TriggerDialogue()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogues, characters);
        }
        else
        {
            Debug.LogWarning("DialogueManager no found");
        }
    }

    public void EndDialogue()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.EndDialogue();
    }

    //Test
    // void Start()
    // {

    //   TriggerDialogue();
    // } 
}

[System.Serializable]
public class Dialogue1 //holds the message
{
    public int characterID;
    public string message;
}

[System.Serializable]
public class Actor1 //holds character info
{
    public string name;
    public Sprite characterSprite;
}