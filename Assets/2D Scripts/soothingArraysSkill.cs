using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class soothingArraysSkill : skill
{
    [SerializeField] public GameObject minigamebackground;
    [SerializeField] public GameObject slash;
    [SerializeField] public GameObject target;
    [SerializeField] public GameObject text;
    private onCollissionHit collisionComponent;
    // private bool miniGameStart = false; // This is to check if the minigame has started

    private IEnumerator MinigameCoroutine(Action<int> onComplete)
    {
        int result;
        
        // Move slash across the screen
        yield return StartCoroutine(MoveSlash());

 
        result = 1;

        // setup(); // Disable UI stuff
        onComplete?.Invoke(result); // when its done we just gonna return the result
    }

    public override int skillInflict()
    {
        return base.skillInflict(); // dw bout this for now
    }

    public void setup() 
    {
        if (minigamebackground != null) minigamebackground.SetActive(false);
        if (slash != null) slash.SetActive(false);
        if (target != null) target.SetActive(false);
        if (text != null) text.SetActive(false); 
    }

    private IEnumerator MoveSlash()
    {
        yield return new WaitForSeconds(1);
        // miniGameStart = true; // Set this to true when the minigame starts
        
        // miniGameStart = false; // Reset this to false after the minigame ends
        
    }
}
