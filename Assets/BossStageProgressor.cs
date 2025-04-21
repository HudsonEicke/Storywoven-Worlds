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
    public AudioSource cannonFire;
    private string startMessage = "I better get to those cannons quick before the dragon kills me";
    private string nextStageMessage = "I need to get back to the main platform quick it looks like the lava is about to rise";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isStarter && !doneProgressing)
            {
                ImportantComponentsManager.Instance.dialogueBox.DisplayText(startMessage, 5f);
                fightManager.StartFight();
                ProgressManager.Instance.NextStage();
                doneProgressing = true;
                return;
            }
            else if (!isStarter)
            {
                if (fightManager.isInStageTransition)
                    return;

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

        cannonFire.Play();

        fired = true;
        ImportantComponentsManager.Instance.dialogueBox.DisplayText(nextStageMessage, 5f);

        GameObject newobj = GameObject.Instantiate(cannonBall, startPoint.position, Quaternion.identity);

        CannonBall ball = newobj.GetComponent<CannonBall>();

        ball.firstPoint = firstPoint;
        ball.secondPoint = boss;
        ball.progressor = this;
        ball.canStartMoving = true;
    }
}
