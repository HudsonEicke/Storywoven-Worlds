using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBattle; // We want to manage the state of our game using an ENUM
using System;
public class GameManager2D : MonoBehaviour
{
    // initializing variables
    public static GameManager2D instance;
    private AudioSystem2D audiosystem2D;
    public BattleState State;
    public static event Action<BattleState> OnBattleStateChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // destroy if instance already exists
        }

        audiosystem2D = gameObject.GetComponent<AudioSystem2D>();
    }

    void Start()
    {
        UpdateBattleState(BattleState.START);
    }

    public void playAudio()
    {
        Debug.Log(audiosystem2D.ReturnAudio());
    }

    public void UpdateBattleState(BattleState newState) 
    {
        State = newState;

        switch(newState) 
        {
            case BattleState.START:
                Debug.Log("Game Started");
                break;
            case BattleState.PLAYERTURN:
                Debug.Log("Player's Turn");
                break;
            case BattleState.ENEMYTURN:
                Debug.Log("Enemy's Turn");
                break;
            case BattleState.WON:
                Debug.Log("You Won!");
                break;
            case BattleState.LOST:
                Debug.Log("You Lost!");
                break;
            default:
                Debug.Log("Invalid Game State");
                break;
        }
        OnBattleStateChanged?.Invoke(newState);
    }
}