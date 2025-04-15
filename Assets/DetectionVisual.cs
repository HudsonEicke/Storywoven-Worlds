using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionVisual : MonoBehaviour
{
    bool displayingVisual = false;
    public float timeToDisplay = 0.75f;
    float timeRemaining = 0f;
    public GameObject visual;

    // Update is called once per frame
    void Update()
    {
        if (displayingVisual)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                displayingVisual = false;
                visual.SetActive(false);
            }
        }
    }

    public void DisplayVisual()
    {
        displayingVisual = true;
        timeRemaining = timeToDisplay;
        visual.SetActive(true);
    }
}
