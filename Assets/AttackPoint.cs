using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public BossFightManager fightManager;
    public AttackPoint leftAttackPoint;
    public AttackPoint rightAttackPoint;
    public Transform fireballPathStart;
    public Transform fireballPathEnd;
    public GameObject warningVisual;

    public int timesToFlash = 2;
    private int currentFlashesLeft = 0;
    public float timeBetweenFlash = 0.5f;
    private float timeToNextCycle = 0f;
    private bool isFlashing = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            fightManager.NewPlayerPos(this);
        }
    }

    private void Start()
    {
        warningVisual.SetActive(false);
    }

    private void Update()
    {
        if(currentFlashesLeft > 0)
        {
            timeToNextCycle -= Time.deltaTime;

            if(timeToNextCycle <= 0)
            {
                if (isFlashing)
                {
                    warningVisual.SetActive(false);
                    currentFlashesLeft -= 1;
                }
                else
                {
                    warningVisual.SetActive(true);
                }

                isFlashing = !isFlashing;
                timeToNextCycle = timeBetweenFlash;
            }
        }
    }

    public void FlashZone()
    {
        currentFlashesLeft = timesToFlash;
        timeToNextCycle = timeBetweenFlash;
    }
}
