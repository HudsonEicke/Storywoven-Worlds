using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    private bool displayingText = false;
    private float timeRemaining = 0;
    public TMP_Text textBox;

    private void Update()
    {
        if (displayingText)
        {
            timeRemaining -= Time.deltaTime;

            if(timeRemaining <= 0)
            {
                textBox.text = string.Empty;
                displayingText = false;
            }
        }
    }

    public void DisplayText(string text, float time)
    {
        textBox.text = text;
        timeRemaining = time;
        displayingText = true;
    }
}
