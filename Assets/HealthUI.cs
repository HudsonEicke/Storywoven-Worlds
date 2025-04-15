using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public List<Image> healthIcons = new List<Image>();

    public void UpdateHealth(int newHealth)
    {
        for (int i = 0; i < healthIcons.Count; i++)
        {
            if(i < newHealth)
                healthIcons[i].gameObject.SetActive(true);
            else
                healthIcons[i].gameObject.SetActive(false);
        }
    }
}
