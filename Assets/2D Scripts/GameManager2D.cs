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
    private BattleSystem battleSystem;

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

        Debug.Log($"[GameManager2D] Updating state to: {newState}");
        Debug.Log($"[GameManager2D] Checking event subscriptions... (Subscribers: {OnBattleStateChanged?.GetInvocationList().Length ?? 0})");

        State = newState;

        switch(newState) 
        {
            case BattleState.START:
                Debug.Log("[GameManager2D] Game Started");
                break;
            case BattleState.PLAYERTURN:
                System.Threading.Thread.Sleep(1000);
                Debug.Log("[GameManager2D] Player's Turn");
                break;
            case BattleState.ENEMYTURN:
                Debug.Log("[GameManager2D] Enemy's Turn");
                break;
            case BattleState.WON:
                Debug.Log("[GameManager2D] You Won!");
                break;
            case BattleState.LOST:
                Debug.Log("[GameManager2D] You Lost!");
                break;
            default:
                Debug.Log("[GameManager2D] Invalid Game State");
                break;
        }
        OnBattleStateChanged?.Invoke(newState);
    }
}