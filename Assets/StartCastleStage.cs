using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCastleStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ProgressManager.Instance.SetStage(CurrentStage.castle);
        }
    }
}
