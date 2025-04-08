using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class rockyWallSkill : skill
{
    [SerializeField] public GameObject rockWall;


    public override void PlayMinigame(Action<int> onComplete)
    {
        Debug.Log("Playing Rock Wall animation...");
        StartCoroutine(MinigameCoroutine(onComplete));
    }
    private IEnumerator MinigameCoroutine(Action<int> onComplete)
    {
        Debug.Log("Playing Rocky Wall animation..");
        // Move slash across the screen
        yield return StartCoroutine(MoveWall());

        // setup(); // Disable UI stuff
        onComplete?.Invoke(1); // when its done we just gonna return the result
    }

    public override int skillInflict()
    {
        return base.skillInflict(); // dw bout this for now
    }


    public void setup() 
    {
        if (rockWall != null) rockWall.SetActive(false);
    }

    private IEnumerator MoveWall()
    {
        
        yield return new WaitForSeconds(0.3f);
        float duration = 0.5f;
        float elapsedTime = 0f;


        //start small and then grow big
        rockWall.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Vector3 startScale = rockWall.transform.localScale;
        Vector3 endScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(0.3f);
        rockWall.SetActive(true); // Show the text

        while (elapsedTime < duration)
        {
            rockWall.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        Vector3 startPos = rockWall.transform.position;
        Vector3 endPos = new Vector3(startPos.x, startPos.y - 225, startPos.z);

        rockWall.transform.position = startPos;

        while (elapsedTime < duration)
        {
            rockWall.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        rockWall.SetActive(false); // Hide the text after the animation
        rockWall.transform.position = startPos;
    }


    public override int skillHeal()
    {
        return base.skillHeal(); // dw bout this for now
    }
}
