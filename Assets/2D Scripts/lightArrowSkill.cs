using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class lightArrowSkill : skill
{
    [SerializeField] public GameObject minigamebackground;
    [SerializeField] public GameObject arrow;
    [SerializeField] public GameObject target;
    [SerializeField] public GameObject text;
    private onCollissionHit collisionComponent;

    private bool spaceBarPressed = false; 
    private bool isTriggerActive = false;
    private bool miniGameStart = false; // This is to check if the minigame has started
    bool hit;

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
        Debug.Log("Playing Light Arrow minigame...");
        spaceBarPressed = false; // Reset input
        StartCoroutine(MinigameCoroutine(onComplete));
    }

    private IEnumerator MinigameCoroutine(Action<int> onComplete)
    {
        int result;
        hit = false;
        minigamebackground.SetActive(true);
        arrow.SetActive(true);
        target.SetActive(true);
        text.SetActive(true);
        
        yield return StartCoroutine(MoveArrow());

        if (hit)
            result = 1;
        else
            result = 0;

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
        if (arrow != null) arrow.SetActive(false);
        if (target != null) target.SetActive(false);
        if (text != null) text.SetActive(false);
    }

    private IEnumerator MoveArrow()
    {
        miniGameStart = true; // Set this to true when the minigame starts
        UnityEngine.Vector3 startPos = arrow.transform.position;
        UnityEngine.Vector3 startScale = arrow.transform.localScale;
        yield return new WaitForSeconds(1);
        float duration = 3.0f;
        float elapsedTime = 0f;

        while (!spaceBarPressed && miniGameStart)
        {
            arrow.transform.Rotate(new UnityEngine.Vector3(0, 0, 360) * Time.deltaTime / 2, Space.Self);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0f;
        while (elapsedTime < duration) {
            // Debug.Log(arrow.transform.position);
            Vector3 moveDirection = new Vector3(Mathf.Cos(arrow.transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(arrow.transform.eulerAngles.z * Mathf.Deg2Rad), 0);
            arrow.transform.position += moveDirection * 500.0f * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            if (isTriggerActive)
            {
                hit = true;
                break;
            }
            yield return null;
        }
        arrow.transform.position = startPos;
        arrow.transform.localScale = startScale;
        miniGameStart = false;
    }
}
