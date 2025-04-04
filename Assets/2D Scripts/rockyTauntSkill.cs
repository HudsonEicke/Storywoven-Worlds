using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RockyTauntSkill : skill
{
    [SerializeField] public GameObject minigamebackground;
    [SerializeField] public GameObject leftFoot;
    [SerializeField] public GameObject rightFoot;
    [SerializeField] public GameObject Smash;
    [SerializeField] public GameObject leftFootHit;
    [SerializeField] public GameObject rightFootHit;
    [SerializeField] public GameObject SmashHit;
    private onCollissionHit collisionComponent1;
    private onCollissionHit collisionComponent2;
    private onCollissionHit collisionComponent3;
    private bool isTriggerActive1 = false;
    private bool isTriggerActive2 = false;
    private bool isTriggerActive3 = false;

    bool miniGameStart = false; // This is to check if the minigame has started
    int count = 0;
    bool pressed = false;
    [SerializeField] public GameObject left;
    [SerializeField] public GameObject right;
    [SerializeField] public GameObject space;
    [SerializeField] public GameObject COUNT;

    // TODO: MAKE THIS A MINIGAME

    private void Start()
    {
        
        if (leftFoot == null || rightFoot == null || Smash == null || leftFootHit == null || rightFootHit == null || SmashHit == null)
        {
            Debug.LogError("One or more GameObjects are NULL in RockyTauntSkill!");
            return;
        }

        collisionComponent1 = leftFoot.GetComponent<onCollissionHit>();
        collisionComponent2 = rightFoot.GetComponent<onCollissionHit>();
        collisionComponent3 = Smash.GetComponent<onCollissionHit>();
        if (collisionComponent1 == null || collisionComponent2 == null || collisionComponent3 == null)
        {
            Debug.LogError("No onCollissionHit found on target!");
            return;
        }


        // Prevent duplicate subscriptions
        collisionComponent1.OnTriggerChanged -= HandleTriggerChanged1;
        collisionComponent1.OnTriggerChanged += HandleTriggerChanged1;
        collisionComponent2.OnTriggerChanged -= HandleTriggerChanged2;
        collisionComponent2.OnTriggerChanged += HandleTriggerChanged2;
        collisionComponent3.OnTriggerChanged -= HandleTriggerChanged3;
        collisionComponent3.OnTriggerChanged += HandleTriggerChanged3;
        
    }

    private void OnDestroy()
    {
        if (collisionComponent1 != null)
        {
            collisionComponent1.OnTriggerChanged -= HandleTriggerChanged1;
        }
        if (collisionComponent2 != null)
        {
            collisionComponent2.OnTriggerChanged -= HandleTriggerChanged2;
        }
        if (collisionComponent3 != null)
        {
            collisionComponent3.OnTriggerChanged -= HandleTriggerChanged3;
        }
    }

    public override int skillInflict()
    {
        return base.skillInflict();
    }

    private void HandleTriggerChanged1(bool isTriggered)
    {
        isTriggerActive1 = isTriggered;
        Debug.Log($"Trigger status changed: {isTriggerActive1}");
    }
    private void HandleTriggerChanged2(bool isTriggered)
    {
        isTriggerActive2 = isTriggered;
        Debug.Log($"Trigger status changed: {isTriggerActive2}");
    }
    private void HandleTriggerChanged3(bool isTriggered)
    {
        isTriggerActive3 = isTriggered;
        Debug.Log($"Trigger status changed: {isTriggerActive3}");
    }

    public override void PlayMinigame(Action<int> onComplete)
    {
        Debug.Log("Playing Rocky Taunt minigame...");
        StartCoroutine(MinigameCoroutine(onComplete));
    }

    private IEnumerator MinigameCoroutine(Action<int> onComplete)
    {
       int result;

        // Enable UI stuff
        minigamebackground.SetActive(true); 
        leftFoot.SetActive(true);
        rightFoot.SetActive(true);
        Smash.SetActive(true);
        leftFootHit.SetActive(true);
        rightFootHit.SetActive(true);
        SmashHit.SetActive(true);
        left.SetActive(true);
        right.SetActive(true);
        space.SetActive(true);
        COUNT.SetActive(true); 
        

        yield return StartCoroutine(RockyTaunt());
        Debug.Log("Count: " + count);
        if (count >= 5) {
            result = 1; // Success
            Debug.Log("Success!");
        }
        else {
            result = 0; // Failure
            Debug.Log("Failure!");
        }

        setup(); // Disable UI stuff
        onComplete?.Invoke(result); // when its done we just gonna return the result
    }

    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Space) && miniGameStart && isTriggerActive3 && !pressed)
        {
            count++;
            Smash.GetComponent<Image>().color = Color.green;
            pressed = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && miniGameStart && isTriggerActive1 && !pressed)
        {
            count++;
            leftFoot.GetComponent<Image>().color = Color.green;
            pressed = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && miniGameStart && isTriggerActive2 && !pressed)
        {
            count++;
            rightFoot.GetComponent<Image>().color = Color.green;
            pressed = true;
        }
    }

    public void setup() 
    {
        // Disable UI stuff
        minigamebackground.SetActive(false);
        leftFoot.SetActive(false);
        rightFoot.SetActive(false);
        Smash.SetActive(false);
        leftFootHit.SetActive(false);
        rightFootHit.SetActive(false);
        SmashHit.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);
        space.SetActive(false);
        COUNT.SetActive(false);
    }

    private IEnumerator RockyTaunt()
    {
        yield return new WaitForSeconds(1);
        miniGameStart = true;
        float duration = 1f;
        float elapsedTime = 0f;

        Vector3 startPosLeft = leftFootHit.transform.position;
        Vector3 endPosLeft = new Vector3(startPosLeft.x, startPosLeft.y - 180, startPosLeft.z);

        Vector3 startPosRight = rightFootHit.transform.position;
        Vector3 endPosRight = new Vector3(startPosRight.x, startPosRight.y - 180, startPosRight.z);

        Vector3 startPosSmash = SmashHit.transform.position;
        Vector3 endPosSmash = new Vector3(startPosSmash.x, startPosSmash.y - 180, startPosSmash.z);
        for (int i = 0; i < 5; i++) {
            pressed = false; 
            Smash.GetComponent<Image>().color = Color.red;
            leftFoot.GetComponent<Image>().color = Color.red;
            rightFoot.GetComponent<Image>().color = Color.red;
            int randomint = Random.Range(0, 3);
            while (elapsedTime < duration)
            {
                COUNT.GetComponent<UnityEngine.UI.Text>().text = count.ToString(); // Update the text with the current count
                if (randomint == 0)
                    leftFootHit.transform.position = Vector3.Lerp(startPosLeft, endPosLeft, elapsedTime / duration);
                else if (randomint == 1)
                    rightFootHit.transform.position = Vector3.Lerp(startPosRight, endPosRight, elapsedTime / duration);
                else if (randomint == 2)
                    SmashHit.transform.position = Vector3.Lerp(startPosSmash, endPosSmash, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f; // Reset elapsed time for the next iteration
            leftFootHit.transform.position = startPosLeft; // Reset position
            rightFootHit.transform.position = startPosRight; // Reset position 
            SmashHit.transform.position = startPosSmash; // Reset position
        }
        Smash.GetComponent<Image>().color = Color.red;
        leftFoot.GetComponent<Image>().color = Color.red;
        rightFoot.GetComponent<Image>().color = Color.red;
        count = 0;
        miniGameStart = false;
        yield return null;
    }
}