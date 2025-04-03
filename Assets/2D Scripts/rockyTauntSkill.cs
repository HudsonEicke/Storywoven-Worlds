using System;
using System.Collections;
using UnityEngine;

public class RockyTauntSkill : skill
{
    [SerializeField] public GameObject minigamebackground;
    [SerializeField] public GameObject leftFoot;
    [SerializeField] public GameObject rightFoot;
    [SerializeField] public GameObject Smash;
    [SerializeField] public GameObject leftFootHit;
    [SerializeField] public GameObject rightFootHit;
    [SerializeField] public GameObject SmashHit;

    // TODO: MAKE THIS A MINIGAME
    public override int skillInflict()
    {
        return base.skillInflict();
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
        

        yield return StartCoroutine(RockyTaunt());
    

        if (true)
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

    private void Update()
    {
        
        
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
    }

    private IEnumerator RockyTaunt()
    {
        yield return new WaitForSeconds(1);
    
        yield return null;
    }
}