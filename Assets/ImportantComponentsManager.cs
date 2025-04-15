using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportantComponentsManager : MonoBehaviour
{
    static ImportantComponentsManager _instance;
    public static ImportantComponentsManager Instance { get { return _instance; } }

    public ThirdPersonMovement thirdPersonMovement;
    public InvetoryUIManager invetoryUIManager;
    public PowerupManager powerupManager;
    public DialogueBox dialogueBox;


    // Start is called before the first frame update
    private void Awake()
    {
        _instance = this;
    }
}
