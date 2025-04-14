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

    private float currentCooldown = 7.5f;

    //Attack point stuff
    public List<AttackPoint> attackPoints;
    public AttackPoint currentPlayerLocation;

    //Attack point pool
    [Space]
    [Header("MUST BE A MULTIPLE OF 3")]
    public int startingSwipePool;
    public int startingFireballPool;
    public int startingAdjacentFireballPool;
    public int startingWipeFireballPool;

    private int totalPool;
    private int currentSwipePool;
    private int currentFireballPool;
    private int currentAdjacentFireballPool;
    private int currentWipeFireballPool;
    public float swipeTime = 5f;
    public float fireballTime = 5f;

    //Fireball spawn point stuff
    public Transform fireBallSpawnPoint;
    public GameObject fireballPrefab;

    //Boss platform
    public MovingPlatformActivationBased bossPlatform;

    //Stage transition stuff
    public bool isInStageTransition = false;

    private void Start()
    {
        totalPool = startingSwipePool + startingFireballPool + startingWipeFireballPool + startingAdjacentFireballPool;
        currentSwipePool = startingSwipePool;
        currentFireballPool = startingFireballPool;
        currentAdjacentFireballPool = startingAdjacentFireballPool;
        startingWipeFireballPool = currentWipeFireballPool;
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

        if (!isAttacking && currentCooldown < 0)
        {
            StartAttack();
        }
        else if (currentCooldown < 0)
        {
            isAttacking = false;
            currentCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
        }
    }

    private void StartAttack()
    {
        isAttacking = true;

        int chosenAttack = Random.Range(0, totalPool);

        if (chosenAttack < currentSwipePool)
        {
            //SWIPE
            if (currentSwipePool < 3)
            {
                switch(currentSwipePool % 3)
                {
                    case 2:
                        currentAdjacentFireballPool++;
                        currentFireballPool++;
                        break;
                    case 1:
                        currentFireballPool++;
                        break;
                }
                currentSwipePool = 0;
            }
            else
            {
                currentAdjacentFireballPool++;
                currentFireballPool++;
                currentWipeFireballPool++;
                currentSwipePool -= 3;
            }
            currentCooldown = swipeTime;
        }
        else if (currentSwipePool <= chosenAttack && chosenAttack < currentFireballPool + currentSwipePool)
        {
            //FIREBALL
            if (currentFireballPool < 3)
            {
                switch (currentFireballPool % 3)
                {
                    case 2:
                        currentAdjacentFireballPool++;
                        currentSwipePool++;
                        break;
                    case 1:
                        currentSwipePool++;
                        break;
                }
                currentFireballPool = 0;
            }
            else
            {
                currentAdjacentFireballPool++;
                currentSwipePool++;
                currentWipeFireballPool++;
                currentSwipePool -= 3;
            }
            currentCooldown = fireballTime;

            FireballAttack(currentPlayerLocation);
        }
        else if (currentFireballPool + currentSwipePool <= chosenAttack && chosenAttack < currentFireballPool + currentSwipePool + currentAdjacentFireballPool)
        {
            //ADJACENT FIREBALL

            if (currentAdjacentFireballPool < 3)
            {
                switch (currentAdjacentFireballPool % 3)
                {
                    case 2:
                        currentFireballPool++;
                        currentSwipePool++;
                        break;
                    case 1:
                        currentSwipePool++;
                        break;
                }
                currentAdjacentFireballPool = 0;
            }
            else
            {
                currentFireballPool++;
                currentSwipePool++;
                currentWipeFireballPool++;
                currentSwipePool -= 3;
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
                        currentSwipePool++;
                        break;
                    case 1:
                        currentSwipePool++;
                        break;
                }
                currentAdjacentFireballPool = 0;
            }
            else
            {
                currentFireballPool++;
                currentSwipePool++;
                currentAdjacentFireballPool++;
                currentSwipePool -= 3;
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

        if(currentStage >= bossFightStages.Count)
        {
            Debug.Log("ENDFIGHT");
            fightStarted = false;
            return;
        }

        if (currentStage > 0)
        {
            foreach(AttackPoint attackPoint in attackPoints)
            {
                attackPoint.FlashZone();
            }
        }

        attackPoints = bossFightStages[currentStage].attackPoints;
        currentPlayerLocation = attackPoints[0];

        bossPlatform.newPos(bossFightStages[currentStage].newBossPlatformLocation);

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
