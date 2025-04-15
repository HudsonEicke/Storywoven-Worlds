using System.Collections;
using System.Collections.Generic;
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
    public float typingSpeed;
    public static bool isActive = false;
    private bool isTyping = false;
    private bool endTyping = false;

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
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(currentDialogue.message));
        dialogueBox.SetActive(true);
    }

    IEnumerator TypeDialogue(string Dialogue){
        isTyping = true;
        endTyping = false;
        dialogueText.text = string.Empty;
        foreach (char letter in Dialogue.ToCharArray())
        {
            if (endTyping)
            {
                dialogueText.text = Dialogue;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        endTyping = false;
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
            if (isTyping)
            {
                endTyping = true;
            }
            else
            {
                NextMessage();
            }
        }
    }
}
