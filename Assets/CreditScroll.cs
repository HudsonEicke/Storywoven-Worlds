using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScroll : MonoBehaviour
{
    public float amountToMovePerSecond;
    public RectTransform rectTransform;
    public bool start = false;
    public bool atDest = false;
    public float timeToDisplay = 5f;
    private float timeRemaining = 0f;

    // Update is called once per frame
    void Update()
    {
        if (!start)
            return;

        if(atDest)
        {
            timeToDisplay -= Time.deltaTime;

            if(timeToDisplay <= 0f)
            {
                SceneManager.LoadScene(0);
            }

            return;
        }

        Vector3 currentPos = rectTransform.localPosition;
        currentPos.y += amountToMovePerSecond * Time.deltaTime;
        if (currentPos.y >= 0)
        {
            timeRemaining = timeToDisplay;
            currentPos.y = 0;
            atDest = true;
        }
        rectTransform.localPosition = currentPos;
    }
}
