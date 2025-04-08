using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class rumbleAndTumbleSkill : skill
{
    [SerializeField] public GameObject minigamebackground;
    [SerializeField] public GameObject slash;
    [SerializeField] public GameObject target;
    [SerializeField] public GameObject text;
    private onCollissionHit collisionComponent;

    private bool spaceBarPressed = false; 
    private bool isTriggerActive = false;
    private bool miniGameStart = false; // This is to check if the minigame has started

/*
    private void Start()
    {
        
        if (target == null)
        {
            Debug.LogError("Target is NULL in slashSkill!");
            return;
        }

        collisionComponent = target.GetComponent<onCollissionHit>();
        if (collisionComponent == null)
        {
            Debug.LogError("No onCollissionHit found on target!");
            return;
        }

        // Prevent duplicate subscriptions
        collisionComponent.OnTriggerChanged -= HandleTriggerChanged;
        collisionComponent.OnTriggerChanged += HandleTriggerChanged;
        
    }

    private void OnDestroy()
    {
        if (collisionComponent != null)
        {
            collisionComponent.OnTriggerChanged -= HandleTriggerChanged;
        }
    }

    // Event handler for the OnTriggerChanged event
    private void HandleTriggerChanged(bool isTriggered)
    {
        isTriggerActive = isTriggered;
        Debug.Log($"Trigger status changed: {isTriggerActive}");
    }


    public override void PlayMinigame(Action<int> onComplete)
    {
        Debug.Log("Playing Flame Shower minigame...");
        spaceBarPressed = false; // Reset input
        StartCoroutine(MinigameCoroutine(onComplete));
    }
*/

    private IEnumerator MinigameCoroutine(Action<int> onComplete)
    {
        int result;
        
        // Move slash across the screen
        yield return StartCoroutine(MoveSlash());

 
        result = 1;

        // setup(); // Disable UI stuff
        onComplete?.Invoke(result); // when its done we just gonna return the result
    }

    public override int skillInflict()
    {
        return base.skillInflict(); // dw bout this for now
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && miniGameStart)
        {
            spaceBarPressed = true;
            //Debug.Log("Pressed spacebar");
        }
    }

    public void setup() 
    {
        if (minigamebackground != null) minigamebackground.SetActive(false);
        if (slash != null) slash.SetActive(false);
        if (target != null) target.SetActive(false);
        if (text != null) text.SetActive(false); 
    }

    private IEnumerator MoveSlash()
    {
        yield return new WaitForSeconds(1);
        miniGameStart = true; // Set this to true when the minigame starts
        
        miniGameStart = false; // Reset this to false after the minigame ends
        
    }
}
