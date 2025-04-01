using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
public class fireballSkill : skill 
{
    [SerializeField] public GameObject fireball;
    [SerializeField] public GameObject fireballBackground;
    [SerializeField] public GameObject fireballFill;
    [SerializeField] public Slider fireballSlider;

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
        fireballBackground.SetActive(true);
        fireballFill.SetActive(true);
        count = 0;
        

        yield return StartCoroutine(Fireball2());
    

        if (count >= 40)
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
            count++;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            count++;
        }
        
    }

    public void setup() 
    {
        if (fireball != null) fireball.SetActive(false);
        if (fireballBackground != null) fireballBackground.SetActive(false);
        if (fireballFill != null) fireballFill.SetActive(false);
    }

    private IEnumerator Fireball2()
    {
        
        float duration = 4.0f;
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

            fireballFill.GetComponent<Slider>().value = count;
            
            yield return null;
        }

        // Ensure final position is exact
        fireballFill.GetComponent<Slider>().value = 0;
        fireball.transform.position = startPos;
        fireball.transform.localScale = startScale;
        
    }
}