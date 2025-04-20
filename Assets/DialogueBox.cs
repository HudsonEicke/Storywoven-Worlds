using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class DialogueBox : MonoBehaviour
{
    private bool displayingText = false;
    private float timeRemaining = 0;
    public TMP_Text textBox;
    public GameObject background;

    private void Update()
    {
        if (displayingText)
        {
            timeRemaining -= Time.deltaTime;

            if(timeRemaining <= 0)
            {
                textBox.text = string.Empty;
                displayingText = false;
                background.SetActive(false);
            }
        }
    }

    public void DisplayText(string text, float time)
    {
        background.SetActive(true);
        textBox.text = text;
        timeRemaining = time;
        displayingText = true;
    }
}
