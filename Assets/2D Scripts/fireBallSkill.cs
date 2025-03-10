using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class fireballSkill : skill 
{
    [SerializeField] public GameObject fireball;

    private bool leftArrowPressed = false; 
    private bool rightArrowPressed = false; 

    private int count = 0;


    public override void PlayMinigame(Action<int> onComplete)
    {
        Debug.Log("Playing FireballSkill minigame...");
        StartCoroutine(MinigameCoroutine(onComplete));
    }

    private IEnumerator MinigameCoroutine(Action<int> onComplete)
    {
        int result;

        // Enabling UI stuff
        fireball.SetActive(true);
        count = 0;
        

        yield return StartCoroutine(Fireball2());
    

        if (count >= 20)
        {
            result = 1;
        }
        else
        {
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
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            leftArrowPressed = true;
            count++;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rightArrowPressed = true;
            count++;
        }
        
    }

    public void setup() 
    {
        fireball.SetActive(false);
    }

    private IEnumerator Fireball2()
    {
        
        float duration = 5.0f;
        float elapsedTime = 0f;

        UnityEngine.Vector3 startPos = fireball.transform.position;
        UnityEngine.Vector3 endPos = startPos + new UnityEngine.Vector3(-90, 0, 0); 

        UnityEngine.Vector3 startScale = fireball.transform.localScale;  // Initial scale
        UnityEngine.Vector3 endScale = startScale * 5;  // uber incrase in size

        while (elapsedTime < duration)
        {
            // Move Fireball
            fireball.transform.position = UnityEngine.Vector3.Lerp(startPos, endPos, elapsedTime / duration);
        
            // Rotate Fireball (Euler Angles)
            fireball.transform.Rotate(new UnityEngine.Vector3(0, 0, 360) * Time.deltaTime, Space.Self);

            // Scale up the fireball
            fireball.transform.localScale = UnityEngine.Vector3.Lerp(startScale, endScale, elapsedTime / duration);
        
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is exact
        fireball.transform.position = startPos;
        fireball.transform.localScale = startScale;
        
    }
}