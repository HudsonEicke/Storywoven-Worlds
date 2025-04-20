using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class DMNoButton : MonoBehaviour
{   
    public GameObject dialogueStuff;
    public GameObject characterBox;
    public Image characterImage;
    public GameObject dialogueBox;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueText;

    public float typingSpeed;

    public static bool isActive = false;
    private bool isTyping = false;
    private bool endTyping = false;
    int index = 0;

    Dialogue1[] dialogues;
    Actor1[] Actors;

    public void StartDialogue(Dialogue1[] dialogue, Actor1[] characters)
    {
        dialogues = dialogue;
        Actors = characters;
        index = 0;
        Debug.Log("Dialogue Started with " + dialogues.Length + "lines");
        isActive = true;
        DisplayDialogue();
    }

    public void DisplayDialogue()
    {
        Dialogue1 currentDialogue = dialogues[index];
        Actor1 currentActor = Actors[currentDialogue.characterID];
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(currentDialogue.message));
        characterImage.sprite = currentActor.characterSprite;
        characterName.text = currentActor.name;
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
        if (index < dialogues.Length)
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
        Debug.Log("Dialogue Ended");
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
