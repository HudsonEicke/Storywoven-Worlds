using System;
using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class RockyTauntSkill : skill
{
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
 
    }

    private IEnumerator RockyTaunt()
    {
    
        yield return null;
    }
}