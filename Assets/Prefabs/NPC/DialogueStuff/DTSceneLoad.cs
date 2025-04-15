using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTSceneLoad : MonoBehaviour
{
    public DTLoadStart autoDialogueTrigger;
    void Start()
    {
        if (autoDialogueTrigger != null)
        {
            autoDialogueTrigger.TriggerDialogue();
        }
        else
        {
            Debug.LogWarning("Auto dialogue trigger not assigned");
        }
    }
}
