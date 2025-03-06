using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    public Image characterImage;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueText;

    public RectTransform dialogueBox;

    private Image charImageActiv;
    private TextMeshProUGUI charNameActiv;
    private TextMeshProUGUI dialogueTextActiv;

    private Image dialogueBoxActiv;


    
    Dialogue[] dialogueArr;
    Actor[] charArr;
    int index = 0;

    public static bool isActive = false;

    public void StartDialogue(Dialogue[] dialogues, Actor[] characters)
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
        Dialogue currentDialogue = dialogueArr[index];
        dialogueText.text = currentDialogue.message;
        Actor currentActor = charArr[currentDialogue.characterID];
        characterImage.sprite = currentActor.characterSprite;
        characterName.text = currentActor.name;
    }

    public void Activate(){

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
            charImageActiv = GameObject.Find("CharacterImage").GetComponent<Image>();  
            charNameActiv = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
            dialogueTextActiv = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
            dialogueBoxActiv = GameObject.Find("DialogueBox").GetComponent<RectTransform>().GetComponent<Image>();
            charImageActiv.enabled = false;
            charNameActiv.enabled = false;
            dialogueTextActiv.enabled = false;
            dialogueBoxActiv.enabled = false;
            isActive = false;
            Debug.Log("End of Dialogue");
        }
    }

    void Start()
    {

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
