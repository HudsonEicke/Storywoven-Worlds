using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class DialogueManager : MonoBehaviour
{   
    public GameObject dialogueStuff;
    public GameObject characterBox;
    public GameObject Panel;
    public Button Button1;
    public Button Button2;
    public Image characterImage;

    public GameObject dialogueBox;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueText;
    
    Dialogue1[] dialogueArr;
    Actor1[] charArr;
    int index = 0;

    public static bool isActive = false;

    void Start()
    {
        if (Button1 != null && Button2 != null)
        {
            Button1.onClick.AddListener(OpenShop);
            Button2.onClick.AddListener(EndDialogue);
        }
        Debug.LogWarning("Buttons not assigned");
    }
    public void StartDialogue(Dialogue1[] dialogues, Actor1[] characters)
    {
        dialogueArr = dialogues;
        charArr = characters;
        index = 0;
        Debug.Log("Dialogue Started with " + dialogueArr.Length + "lines");
        isActive = true;
        if (Panel != null)
        {
            Panel.SetActive(false);
        }
        DisplayDialogue();
    }

    public void DisplayDialogue()
    {
        Dialogue1 currentDialogue = dialogueArr[index];
        Actor1 currentActor = charArr[currentDialogue.characterID];
        dialogueText.text = currentDialogue.message;
        characterImage.sprite = currentActor.characterSprite;
        characterName.text = currentActor.name;
        characterImage.enabled = true;
        characterName.enabled = true;
        dialogueText.enabled = true;
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
            if (Panel != null)
            {
                Panel.SetActive(true);
                EventSystem.current.SetSelectedGameObject(Button1.gameObject);
            }
            else
            {
                EndDialogue();
            }
        }
    }

    void OpenShop()
    {
        dialogueStuff.SetActive(false);
        Panel.SetActive(false);
        isActive = false;
        SceneManager.LoadScene("MenuScene");
        Debug.Log("Shop Opened");
    }

    public void EndDialogue()
    {
        dialogueStuff.SetActive(false);
        if (Panel != null)
        {
            Panel.SetActive(false);
        }
        isActive = false;
        Debug.Log("Dialogue Ended");
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
