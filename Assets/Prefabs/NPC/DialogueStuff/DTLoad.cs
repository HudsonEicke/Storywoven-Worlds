using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DTLoad : MonoBehaviour
{
    public Dialogue[] dialogues;
    public Actor[] characters;
    
    private Image characterImage;
    private TextMeshProUGUI characterName;
    private TextMeshProUGUI dialogueText;

    private Image dialogueBox;
    
    public void TriggerDialogue()
    {
        characterImage.enabled = true;
        characterName.enabled = true;
        dialogueText.enabled = true;
        dialogueBox.enabled = true;
        FindObjectOfType<DialogueManager>().StartDialogue(dialogues, characters);
    }

    void Start()
    {
    //   characterImage = GameObject.Find("CharacterImage").GetComponent<Image>();  
    //   characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
    //   dialogueText = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
    //   dialogueBox = GameObject.Find("DialogueBox").GetComponent<RectTransform>().GetComponent<Image>();
    //   characterImage.enabled = false;
    //   characterName.enabled = false;
    //   dialogueText.enabled = false;
    //   dialogueBox.enabled = false;
      FindObjectOfType<DialogueManager>().StartDialogue(dialogues, characters);
    } 
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