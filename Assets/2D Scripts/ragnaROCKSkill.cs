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
    // [SerializeField] public GameObject missTarget;
    [SerializeField] public GameObject text;
    private onCollissionHit collisionComponent;

    private bool miniGameStart = false; // This is to check if the minigame has started
    private bool spaceBarPressed = false; 
    private onCollissionHit collisionComponent1;
    // private onCollissionHit collisionComponent2;

    private bool isTriggerActive = false;
    bool fail = false;
    // private bool isTriggerActive2 = false;
    private int count = 0;

    private void Start()
    {
        
        if (hitTarget == null)
        {
            Debug.LogError("Target is NULL!");
            return;
        }

        collisionComponent1 = hitTarget.GetComponent<onCollissionHit>();
        // collisionComponent2 = missTarget.GetComponent<onCollissionHit>();
        
        if (collisionComponent1 == null)
        {
            Debug.LogError("No onCollissionHit found on target!");
            return;
        }

        // Prevent duplicate subscriptions
        collisionComponent1.OnTriggerChanged -= HandleTriggerChanged;
        collisionComponent1.OnTriggerChanged += HandleTriggerChanged;
        // collisionComponent2.OnTriggerChanged -= HandleTriggerChanged2;
        // collisionComponent2.OnTriggerChanged += HandleTriggerChanged2;
        
    }

    private void OnDestroy()
    {
        if (collisionComponent1 != null )
        {
            collisionComponent1.OnTriggerChanged -= HandleTriggerChanged;
        }
        /*
        if (collisionComponent2 != null )
        {
            collisionComponent2.OnTriggerChanged -= HandleTriggerChanged;
        }
        */
    }

    // Event handler for the OnTriggerChanged event
    private void HandleTriggerChanged(bool isTriggered)
    {
        isTriggerActive = isTriggered;
        Debug.Log($"Trigger status changed: {isTriggerActive}");
    }

    /*
    private void HandleTriggerChanged2(bool isTriggered)
    {
        isTriggerActive2 = isTriggered;
        // Debug.Log($"Trigger2 status changed: {isTriggerActive2}");
    }
    */

    
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
        // missTarget.SetActive(true);
        text.SetActive(true);
        count = 0;
        int result;
        
        // Move slash across the screen
        yield return StartCoroutine(RagnaROCK());
        if (count > 0)
            result = 1;
        else
            result = 0;
        count = 0;

        setup(); // Disable UI stuff
        onComplete?.Invoke(result); // when its done we just gonna return the result
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && miniGameStart)
        {   // Debug.Log(count);
            spaceBarPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) && miniGameStart)
        {
            // Debug.Log(count);
            spaceBarPressed = false;
            // count++;
        }

        if (isTriggerActive){
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
        // if (missTarget != null) missTarget.SetActive(false);
    }

    private IEnumerator RagnaROCK()
    {
        yield return new WaitForSeconds(1);
        
        miniGameStart = true; // Set this to true when the minigame starts
        float duration = 1.0f;
        float elapsedTime = 0f;

        float fistChargeduration = 1.0f;
        float fistCharageElapsed = .0f;

        // for target
        Vector3 origionalPos = hitTarget.transform.position;
        Vector3 startPos = hitTarget.transform.position;
        int travel = 290;
        Vector3 endPost = new Vector3(startPos.x + travel, startPos.y, startPos.z);

        // for fist
        Vector3 fistOrigional = fist.transform.position;

        for (int i = 0 ; i < 4; i++) {
            if (count > 0 || fail) {
                    break;
            }
            while (elapsedTime < duration) {
                elapsedTime += Time.deltaTime;
                hitTarget.transform.position = Vector3.Lerp(startPos, endPost, elapsedTime / duration);
                if (spaceBarPressed) {
                    Vector3 startPosFist = fist.transform.position;
                    Vector3 endPosFist = new Vector3(startPosFist.x, startPosFist.y - 400, startPosFist.z);
                    while (fistCharageElapsed < fistChargeduration) {
                        fistCharageElapsed += Time.deltaTime;
                        fist.transform.position = Vector3.Lerp(startPosFist, endPosFist, fistCharageElapsed / fistChargeduration);
                        if (count > 0) {
                            break;
                        }
                        fail = true;
                        yield return null;
                    }
                    fistCharageElapsed = .0f;
                    break;
                }
                if (count > 0 || fail) {
                    break;
                }
                yield return null;
            }
            elapsedTime = 0f; // Reset elapsed time for the next iterations
            travel *= -1;
            startPos = hitTarget.transform.position;
            endPost = new Vector3(startPos.x + travel, startPos.y, startPos.z);
        }
        elapsedTime = 0f;
        fistCharageElapsed = 0f;
        fail = false;
        // Debug.Log(count);
        miniGameStart = false; // Reset this to false after the minigame ends
        hitTarget.transform.position = origionalPos; 
        fist.transform.position = fistOrigional;
    }
}
