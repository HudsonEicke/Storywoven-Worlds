using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryDisplayer : MonoBehaviour
{
    public List<TMP_Text> textMeshProList;
    public float timeToChangeText = 3f;
    public float timeToDisplayText = 7.5f;

    private float timeRemainingOnText;
    private float timeRemainingOnChange;
    private float decreaseAmount;

    private int textIdx = 0;

    public string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        textMeshProList[0].color = new Color32(255, 255, 255, 255);
        timeRemainingOnText = timeToDisplayText;
        decreaseAmount = 255 / (timeToChangeText / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemainingOnText <= 0)
        {
            timeRemainingOnChange -= Time.deltaTime;

            if (timeRemainingOnChange <= 0)
            {
                timeRemainingOnText = timeToDisplayText;
            }
            if(timeRemainingOnChange <= timeToChangeText / 2)
            {
                textMeshProList[textIdx - 1].color = new Color32(255, 255, 255, 0);
                Color32 oldColor = textMeshProList[textIdx].color;
                oldColor.a += (byte)(decreaseAmount * Time.deltaTime);
                textMeshProList[textIdx].color = oldColor;
            }
            else
            {
                Color32 oldColor = textMeshProList[textIdx - 1].color;
                oldColor.a -= (byte)(decreaseAmount * Time.deltaTime);
                textMeshProList[textIdx - 1].color = oldColor;
            }
        }
        else
        {
            timeRemainingOnText -= Time.deltaTime;
            textMeshProList[textIdx].color = new Color32(255, 255, 255, 255);

            if (timeRemainingOnText <= 0)
            {
                timeRemainingOnChange = timeToChangeText;
                textIdx++;

                if(textIdx == textMeshProList.Count)
                {
                    SceneManager.LoadScene(nextScene);
                }
            }
        }
    }
}
