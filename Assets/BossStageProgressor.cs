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
    public bool fired = false;
    public GameObject cannonBall;
    public Transform startPoint;
    public Transform firstPoint;
    public Transform boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isStarter && !doneProgressing)
            {
                fightManager.StartFight();
                doneProgressing = true;
                return;
            }
            else if (!isStarter)
            {
                progressingStage = true;
                FireCannon();
            }
        }
    }

    private void Update()
    {
        if (doneProgressing)
            return;

        if (!progressingStage)
            return;
    }

    public void DoneProgressing()
    {
        fightManager.NextStage();
        doneProgressing = true;
    }

    private void FireCannon()
    {
        if(fired)
            return;

        fired = true;

        GameObject newobj = GameObject.Instantiate(cannonBall, startPoint.position, Quaternion.identity);

        CannonBall ball = newobj.GetComponent<CannonBall>();

        ball.firstPoint = firstPoint;
        ball.secondPoint = boss;
        ball.progressor = this;
        ball.canStartMoving = true;
    }
}
