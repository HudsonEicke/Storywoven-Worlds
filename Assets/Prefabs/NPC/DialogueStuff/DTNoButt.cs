using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DTNoButt : MonoBehaviour
{
    public Dialogue1[] dialogues;
    public Actor1[] characters;
    
    public void TriggerDialogue()
    {
        DMNoButton dialogueManager = FindObjectOfType<DMNoButton>();
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
        DMNoButton dialogueManager = FindObjectOfType<DMNoButton>();
        dialogueManager.EndDialogue();
    }

    //Test
    // void Start()
    // {

    //   TriggerDialogue();
    // } 
}

[System.Serializable]
public class Dialogue3 //holds the message
{
    public int characterID;
    public string message;
}

[System.Serializable]
public class Actor3 //holds character info
{
    public string name;
    public Sprite characterSprite;
}