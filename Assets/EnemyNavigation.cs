using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    public NavMeshAgent Agent;
    public EnemyState state = EnemyState.idle;
    public float idleTime = 5;
    public float wanderTime = 10;
    public float stateTime;
    private float timeChange = 1;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        stateTime = idleTime;
    }

    void changeState()
    {
        if(state == EnemyState.idle)
        {
            state = EnemyState.wandering;
            stateTime = wanderTime;
            timeChange = 0;
        } 

        else
        {
            state = EnemyState.idle;
            stateTime = idleTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(state != EnemyState.chasing)
        {
            stateTime -= Time.deltaTime;
            if(stateTime <= 0)
            {
                changeState();
            }
        }

        switch(state)
        {
            case EnemyState.idle:
                Agent.SetDestination(gameObject.transform.position);
                break;
            case EnemyState.wandering:
                timeChange -= Time.deltaTime;
                wandering();
                break;
            case EnemyState.chasing:
                chasing();
                break;
        }
    }

    void wandering()
    {
        if(timeChange > 0)
        {
            return;
        }
    
        float xOffset = Random.Range(-5f, 5f);
        float yOffset = Random.Range(-5f, 5f);

        Agent.SetDestination(new Vector3(gameObject.transform.position.x + xOffset, gameObject.transform.position.y + yOffset, gameObject.transform.position.z));
        timeChange = 5;
    }

    void chasing()
    {
        Agent.SetDestination(player.transform.position);

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            state = EnemyState.chasing;
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            state = EnemyState.wandering;
            changeState();
        }
    }
 
}
public enum EnemyState
    {
        idle,
        wandering,
        chasing,
    }
