using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class rumbleAndTumbleSkill : skill
{
    [SerializeField] public GameObject minigamebackground;
    [SerializeField] public GameObject fist;
    [SerializeField] public GameObject fillBar;
    [SerializeField] public Slider fillBarSlider;
    [SerializeField] public GameObject text;
    private onCollissionHit collisionComponent;

    private bool spaceBarPressed = false; 
    private bool isTriggerActive = false;
    private bool miniGameStart = false; // This is to check if the minigame has started
    int count = 0;

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
*/

    public override void PlayMinigame(Action<int> onComplete)
    {
        Debug.Log("Playing Rumble minigame...");
        spaceBarPressed = false; // Reset input
        StartCoroutine(MinigameCoroutine(onComplete));
    }


    private IEnumerator MinigameCoroutine(Action<int> onComplete)
    {
        minigamebackground.SetActive(true);
        fist.SetActive(true);
        fillBar.SetActive(true);
        // fillBarSlider.gameObject.SetActive(true);
        text.SetActive(true);
        int result;
        
        // Move slash across the screen
        yield return StartCoroutine(Rumble());

        if (count >= 50)
            result = 1;
        else
            result = 0;
        
        count = 0;
        fillBar.GetComponent<Slider>().value = count;
        setup();
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
        }
        if (Input.GetKeyUp(KeyCode.Space) && miniGameStart && spaceBarPressed)
        {
            spaceBarPressed = false;
            count++;
        }
    }

    public void setup() 
    {
        if (minigamebackground != null) minigamebackground.SetActive(false);
        if (fist != null) fist.SetActive(false);
        if (fillBar != null) fillBar.SetActive(false);
        // if (fillBarSlider != null) fillBarSlider.gameObject.SetActive(false);
        if (text != null) text.SetActive(false); 
    }

    private IEnumerator Rumble()
    {
        yield return new WaitForSeconds(1);
        UnityEngine.Vector3 startPos = fist.transform.position;
        UnityEngine.Vector3 endPos = new Vector3(startPos.x, startPos.y + 400, startPos.z);
        UnityEngine.Vector3 chargePosLeft = startPos + new UnityEngine.Vector3(-10, 0, 0); 
        UnityEngine.Vector3 chargePosRight = startPos + new UnityEngine.Vector3(+10, 0, 0);
        bool left = false; 
        float duration = 7.0f;
        float elapsedTime = 0f;
        miniGameStart = true; // Set this to true when the minigame starts
        while (elapsedTime < duration) {
            if (spaceBarPressed) {
                if (!left) {
                    fist.transform.position = chargePosLeft;
                    left = true;
                }
                else {
                    fist.transform.position = chargePosRight;
                    left = false;
                }
            }
            else 
                fist.transform.position = startPos;
            elapsedTime += Time.deltaTime;
            fillBar.GetComponent<Slider>().value = count;
            yield return null;
        }
        elapsedTime = 0.0f;
        duration = 0.3f;
        if (count >=50) {
            while (elapsedTime < duration) {
                fist.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        fist.transform.position = startPos;
        miniGameStart = false; // Reset this to false after the minigame ends
        
    }
}
