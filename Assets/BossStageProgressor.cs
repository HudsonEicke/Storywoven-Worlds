using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageProgressor : MonoBehaviour
{
    public BossFightManager fightManager;

    private void ProgessBossStage()
    {
        fightManager.NextStage();
    }
}
