using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DTLoadStart : MonoBehaviour
{
    public Dialogue1[] dialogues;
    public Actor1[] characters;
    
    public void TriggerDialogue()
    {
        DMSceneLoad dialogueManager = FindObjectOfType<DMSceneLoad>();
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogues, characters);
        }
        else
        {
            Debug.LogWarning("DialogueManager no found");
        }
    }

    //Test
    // void Start()
    // {

    //   TriggerDialogue();
    // } 
}

[System.Serializable]
public class Dialogue2 //holds the message
{
    public int characterID;
    public string message;
}

[System.Serializable]
public class Actor2 //holds character info
{
    public string name;
    public Sprite characterSprite;
}