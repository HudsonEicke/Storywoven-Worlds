using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ragnaROCKSkill : skill
{
    [SerializeField] public GameObject minigamebackground;
    [SerializeField] public GameObject fist;
    [SerializeField] public GameObject hitTarget;
    [SerializeField] public GameObject missTarget;
    [SerializeField] public GameObject text;
    private onCollissionHit collisionComponent;

    private bool miniGameStart = false; // This is to check if the minigame has started
    private bool spaceBarPressed = false; 
    private onCollissionHit collisionComponent1;
    private onCollissionHit collisionComponent2;

    private bool isTriggerActive = false;
    private bool isTriggerActive2 = false;
    private int count = 0;

    private void Start()
    {
        
        if (hitTarget == null || missTarget == null)
        {
            Debug.LogError("Target is NULL!");
            return;
        }

        collisionComponent1 = hitTarget.GetComponent<onCollissionHit>();
        collisionComponent2 = missTarget.GetComponent<onCollissionHit>();
        
        if (collisionComponent1 == null || collisionComponent2 == null)
        {
            Debug.LogError("No onCollissionHit found on target!");
            return;
        }

        // Prevent duplicate subscriptions
        collisionComponent1.OnTriggerChanged -= HandleTriggerChanged;
        collisionComponent1.OnTriggerChanged += HandleTriggerChanged;
        collisionComponent2.OnTriggerChanged -= HandleTriggerChanged2;
        collisionComponent2.OnTriggerChanged += HandleTriggerChanged2;
        
    }

    private void OnDestroy()
    {
        if (collisionComponent1 != null )
        {
            collisionComponent1.OnTriggerChanged -= HandleTriggerChanged;
        }
        if (collisionComponent2 != null )
        {
            collisionComponent2.OnTriggerChanged -= HandleTriggerChanged;
        }
    }

    // Event handler for the OnTriggerChanged event
    private void HandleTriggerChanged(bool isTriggered)
    {
        isTriggerActive = isTriggered;
        Debug.Log($"Trigger status changed: {isTriggerActive}");
    }
    private void HandleTriggerChanged2(bool isTriggered)
    {
        isTriggerActive2 = isTriggered;
        // Debug.Log($"Trigger2 status changed: {isTriggerActive2}");
    }

    
    public override void PlayMinigame(Action<int> onComplete)
    {
        Debug.Log("Playing RagnaROCK minigame...");
        spaceBarPressed = false;
        StartCoroutine(MinigameCoroutine(onComplete));
    }

    private IEnumerator MinigameCoroutine(Action<int> onComplete)
    {
        minigamebackground.SetActive(true);
        fist.SetActive(true);
        hitTarget.SetActive(true);
        missTarget.SetActive(true);
        text.SetActive(true);
        count = 0;
        int result;
        
        // Move slash across the screen
        yield return StartCoroutine(RagnaROCK());

 
        result = 1;

        setup(); // Disable UI stuff
        onComplete?.Invoke(result); // when its done we just gonna return the result
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && miniGameStart && isTriggerActive)
        {   Debug.Log(count);
            spaceBarPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) && miniGameStart && spaceBarPressed)
        {
            Debug.Log(count);
            spaceBarPressed = false;
            isTriggerActive = false;
            count++;
        }
    }

    public override int skillInflict()
    {
        return base.skillInflict(); // dw bout this for now
    }

    public void setup() 
    {
        if (minigamebackground != null) minigamebackground.SetActive(false);
        if (text != null) text.SetActive(false); 
        if (fist != null) fist.SetActive(false);
        if (hitTarget != null) hitTarget.SetActive(false);
        if (missTarget != null) missTarget.SetActive(false);
    }

    private IEnumerator RagnaROCK()
    {
        yield return new WaitForSeconds(1);
        UnityEngine.Vector3 startPos = fist.transform.position;
        UnityEngine.Vector3 endPos = new Vector3(startPos.x, startPos.y - 300, startPos.z);
        UnityEngine.Vector3 highPos1 = new Vector3(startPos.x, startPos.y + 200, startPos.z);
        UnityEngine.Vector3 highPos2 = new Vector3(startPos.x, startPos.y + 350, startPos.z);
        UnityEngine.Vector3 currentPos = fist.transform.position;
        float duration = 1.0f;
        float elapsedTime = 0f;
        miniGameStart = true; // Set this to true when the minigame starts
        while (elapsedTime < duration) {
            fist.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            currentPos = fist.transform.position;
            Debug.Log(count);
            yield return null;
        }



        elapsedTime = 0f;
        duration = 2.0f;
        while (elapsedTime < duration) {
            fist.transform.position = Vector3.Lerp(currentPos, highPos1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            currentPos = fist.transform.position;
            yield return null;
        }
        elapsedTime = 0f;
        duration = 1.0f;
        while (elapsedTime < duration) {
            fist.transform.position = Vector3.Lerp(currentPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            currentPos = fist.transform.position;
            yield return null;
        }
        elapsedTime = 0f;
        duration = 2.0f;
        while (elapsedTime < duration) {
            fist.transform.position = Vector3.Lerp(currentPos, highPos2, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            currentPos = fist.transform.position;
            yield return null;
        }
        elapsedTime = 0f;
        duration = 0.5f;
        while (elapsedTime < duration) {
            fist.transform.position = Vector3.Lerp(currentPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        miniGameStart = false; // Reset this to false after the minigame ends
        fist.transform.position = startPos;
    }
}
