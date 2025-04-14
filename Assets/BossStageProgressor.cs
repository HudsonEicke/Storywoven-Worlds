using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageProgressor : MonoBehaviour
{
    public BossFightManager fightManager;
    public float timeBeforeProgression = 2f;
    public bool progressingStage = false;
    public bool doneProgressing = false;
    public bool isStarter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(isStarter && !doneProgressing);
            if (isStarter && !doneProgressing)
            {
                fightManager.StartFight();
                doneProgressing = true;
                return;
            }
            progressingStage = true;
        }
    }

    private void Update()
    {
        if (doneProgressing)
            return;

        if (!progressingStage)
            return;

        timeBeforeProgression -= Time.deltaTime;

        if (timeBeforeProgression <= 0)
        {
            fightManager.NextStage();
            doneProgressing = true;
        }
    }
}
