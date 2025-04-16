using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    public bool fightStarted = false;
    public bool isAttacking = false;

    //Boss fight stage stuff
    public int currentStage = -1;
    public List<BossFightStage> bossFightStages = new List<BossFightStage>();

    //Cool down stuff
    public float minAttackCooldown = 5f;
    public float maxAttackCooldown = 7.5f;

    public float currentCooldown = 7.5f;

    //Attack point stuff
    public List<AttackPoint> attackPoints;
    public AttackPoint currentPlayerLocation;

    //Attack point pool
    [Space]
    [Header("MUST BE A MULTIPLE OF 3")]
    public int startingEveryOtherFireballPool;
    public int startingFireballPool;
    public int startingAdjacentFireballPool;
    public int startingWipeFireballPool;

    private int totalPool;
    private int currentEveryOtherFireballPool;
    private int currentFireballPool;
    private int currentAdjacentFireballPool;
    private int currentWipeFireballPool;
    public float fireballTime = 5f;

    //Fireball spawn point stuff
    public Transform fireBallSpawnPoint;
    public GameObject fireballPrefab;

    //Boss platform
    public MovingPlatformActivationBased bossPlatform;
    public float timeBeforePlatformMove = 5f;
    public float timeBeforeFinalPlatformMove = 5f;
    public Transform finalPlatformPos;

    //Stage transition stuff
    public bool isInStageTransition = false;

    public Animator animator;
    public float delayBeforeAttack = 1f;
    public float timeToAttack = 0f;
    private bool firstRun = true;

    private void Start()
    {
        totalPool = startingEveryOtherFireballPool + startingFireballPool + startingWipeFireballPool + startingAdjacentFireballPool;
        currentEveryOtherFireballPool = startingEveryOtherFireballPool;
        currentFireballPool = startingFireballPool;
        currentAdjacentFireballPool = startingAdjacentFireballPool;
        currentWipeFireballPool = startingWipeFireballPool;
    }

    private void Update()
    {
        if (!fightStarted)
            return;

        if (isInStageTransition)
        {
            if(bossPlatform.atDest)
            {
                isInStageTransition = false;
            }
            else
            {
                return;
            }
        }

        currentCooldown -= Time.deltaTime;

        if (!isAttacking && currentCooldown < 0 && timeToAttack < 0)
        {
            StartAttack();
        }
        else if(!isAttacking && currentCooldown < 0)
        {
            if(firstRun)
            {
                Debug.Log("PLAYING ANIM");
                animator.Play("Fly Breathe Fire", 0, 0);
                firstRun = false;
            }

            timeToAttack -= Time.deltaTime;
        }
        else if (currentCooldown < 0) //if they stopped attacking
        {
            isAttacking = false;
            firstRun = true;
            timeToAttack = delayBeforeAttack;
            currentCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
        }
    }

    private void StartAttack()
    {
        isAttacking = true;

        int chosenAttack = Random.Range(0, totalPool);

        if (chosenAttack < currentEveryOtherFireballPool)
        {
            //EVERY OTHER FIREBALL
            chosenAttack = Random.Range(0, 2); //even or odd points


            if (currentEveryOtherFireballPool < 3)
            {
                switch(currentEveryOtherFireballPool % 3)
                {
                    case 2:
                        currentAdjacentFireballPool++;
                        currentFireballPool++;
                        break;
                    case 1:
                        currentFireballPool++;
                        break;
                }
                currentEveryOtherFireballPool = 0;
            }
            else
            {
                currentAdjacentFireballPool++;
                currentFireballPool++;
                currentWipeFireballPool++;
                currentEveryOtherFireballPool -= 3;
            }
            currentCooldown = fireballTime;

            for (int i = chosenAttack; i < attackPoints.Count; i += 2)
            {
                FireballAttack(attackPoints[i]);
            }
        }
        else if (currentEveryOtherFireballPool <= chosenAttack && chosenAttack < currentFireballPool + currentEveryOtherFireballPool)
        {
            //FIREBALL
            if (currentFireballPool < 3)
            {
                switch (currentFireballPool % 3)
                {
                    case 2:
                        currentAdjacentFireballPool++;
                        currentEveryOtherFireballPool++;
                        break;
                    case 1:
                        currentEveryOtherFireballPool++;
                        break;
                }
                currentFireballPool = 0;
            }
            else
            {
                currentAdjacentFireballPool++;
                currentEveryOtherFireballPool++;
                currentWipeFireballPool++;
                currentEveryOtherFireballPool -= 3;
            }
            currentCooldown = fireballTime;

            FireballAttack(currentPlayerLocation);
        }
        else if (currentFireballPool + currentEveryOtherFireballPool <= chosenAttack && chosenAttack < currentFireballPool + currentEveryOtherFireballPool + currentAdjacentFireballPool)
        {
            //ADJACENT FIREBALL

            if (currentAdjacentFireballPool < 3)
            {
                switch (currentAdjacentFireballPool % 3)
                {
                    case 2:
                        currentFireballPool++;
                        currentEveryOtherFireballPool++;
                        break;
                    case 1:
                        currentEveryOtherFireballPool++;
                        break;
                }
                currentAdjacentFireballPool = 0;
            }
            else
            {
                currentFireballPool++;
                currentEveryOtherFireballPool++;
                currentWipeFireballPool++;
                currentEveryOtherFireballPool -= 3;
            }
            currentCooldown = fireballTime;

            FireballAttack(currentPlayerLocation.leftAttackPoint);
            FireballAttack(currentPlayerLocation.rightAttackPoint);
        }
        else
        {
            //WIPE FIREBALL

            if (currentAdjacentFireballPool < 3)
            {
                switch (currentAdjacentFireballPool % 3)
                {
                    case 2:
                        currentFireballPool++;
                        currentEveryOtherFireballPool++;
                        break;
                    case 1:
                        currentEveryOtherFireballPool++;
                        break;
                }
                currentAdjacentFireballPool = 0;
            }
            else
            {
                currentFireballPool++;
                currentEveryOtherFireballPool++;
                currentAdjacentFireballPool++;
                currentEveryOtherFireballPool -= 3;
            }
            currentCooldown = fireballTime;

            FireballAttack(currentPlayerLocation.leftAttackPoint);
            FireballAttack(currentPlayerLocation);
            FireballAttack(currentPlayerLocation.rightAttackPoint);
        }
    }

    public void StartFight()
    {
        fightStarted = true;
        NextStage();
    }

    public void NextStage()
    {
        currentStage++;

        if (currentStage > 0)
        {
            foreach(AttackPoint attackPoint in attackPoints)
            {
                attackPoint.FlashZone();
            }
        }

        if (currentStage >= bossFightStages.Count)
        {
            Debug.Log("ENDFIGHT");
            fightStarted = false;
            bossPlatform.newPos(finalPlatformPos, timeBeforePlatformMove);
            return;
        }

        attackPoints = bossFightStages[currentStage].attackPoints;
        currentPlayerLocation = attackPoints[0];

        bossPlatform.newPos(bossFightStages[currentStage].newBossPlatformLocation, timeBeforePlatformMove);

        isInStageTransition = true;

        currentCooldown = minAttackCooldown;
        isAttacking = false;

        ImportantComponentsManager.Instance.thirdPersonMovement.lastGroundPosition = bossFightStages[currentStage].newSafeLocation.position;
    }

    public void NewPlayerPos(AttackPoint attackPoint)
    {
        currentPlayerLocation = attackPoint;
    }

    public void FireballAttack(AttackPoint attackPoint)
    {
        attackPoint.FlashZone();

        GameObject newFireball = Instantiate(fireballPrefab, fireBallSpawnPoint.transform.position, Quaternion.identity);

        Fireball fireball = newFireball.GetComponent<Fireball>();

        fireball.firstPoint = attackPoint.fireballPathStart;
        fireball.secondPoint = attackPoint.fireballPathEnd;
        fireball.canStartMoving = true;
    }
}
