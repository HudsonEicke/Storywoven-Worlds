using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class slashSkill : skill
{
    [SerializeField] public GameObject minigamebackground;
    [SerializeField] public GameObject slash;
    [SerializeField] public GameObject target;
    [SerializeField] public GameObject text;
    private onCollissionHit collisionComponent;

    private bool spaceBarPressed = false; 
    private bool isTriggerActive = false;
    private bool miniGameStart = false; // This is to check if the minigame has started

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
        Debug.Log("Playing slashSkill minigame...");
        spaceBarPressed = false; // Reset input
        StartCoroutine(MinigameCoroutine(onComplete));
    }

    private IEnumerator MinigameCoroutine(Action<int> onComplete)
    {
        int result;

        // Enabling UI stuff
        minigamebackground.SetActive(true);
        slash.SetActive(true);
        target.SetActive(true);
        text.SetActive(true); // Show the text
        
        // Move slash across the screen
        yield return StartCoroutine(MoveSlash());

        // Timer countdown for minigame
        float timer = 2.0f;
        while (timer > 0)
        {
            if (spaceBarPressed) break; // Exit early if Spacebar was pressed
            timer -= Time.deltaTime;
            yield return null;
        }

        // event handler allows us to check if the trigger is active real time, this was so painful to figure out
        if (isTriggerActive && spaceBarPressed)
        {
            Debug.Log("Minigame success!");
            result = 1;
        }
        else
        {
            Debug.Log("Minigame failed!");
            result = 0;
        }

        setup(); // Disable UI stuff
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
        float duration = 1.3f;
        float elapsedTime = 0f;

        Vector3 startPos = slash.transform.position;
        Vector3 endPos = new Vector3(startPos.x + 700, startPos.y, startPos.z);

        slash.transform.position = startPos;

        while (elapsedTime < duration)
        {
            slash.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
            if(spaceBarPressed) break; // gotta break it early so we can tell if the player press the spacebar at the right time
        }
        miniGameStart = false; // Reset this to false after the minigame ends
        slash.transform.position = startPos; // Ensure it resets
    }
}
