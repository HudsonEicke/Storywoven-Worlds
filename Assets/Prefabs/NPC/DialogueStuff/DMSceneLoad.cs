using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DMSceneLoad : MonoBehaviour
{   
    public GameObject dialogueStuff;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    
    Dialogue1[] dialogueArr;
    Actor1[] charArr;
    int index = 0;

    public static bool isActive = false;

    public void StartDialogue(Dialogue1[] dialogues, Actor1[] characters)
    {
        dialogueArr = dialogues;
        charArr = characters;
        index = 0;
        Debug.Log("Dialogue Started with " + dialogueArr.Length + "lines");
        isActive = true;
        DisplayDialogue();
    }

    public void DisplayDialogue()
    {
        Dialogue1 currentDialogue = dialogueArr[index];
        Actor1 currentActor = charArr[currentDialogue.characterID];
        dialogueText.text = currentDialogue.message;
        dialogueText.enabled = true;
        dialogueBox.SetActive(true);
    }

    void NextMessage()
    {
        index++;
        if (index < dialogueArr.Length)
        {
            DisplayDialogue();

        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        dialogueStuff.SetActive(false);
        isActive = false;
        Debug.Log("Dialogue Over");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isActive)
        {
            NextMessage();
        }
    }
}
