using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameBattle;
using System;

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public GameObject enemyPrefab;
    public Transform playerBattleStation;
    public Transform playerBattleStation2;
    public Transform enemyBattleStation;
    public BattleState state;


    Unit playerUnit, playerUnit2, enemyUnit;

    public Text enemyHud, Player1Hud, Player2Hud;
    // Player3Hud, Player4Hud;

    void Awake()
    {
        // We want to do this during awake since Start() does it in a different order sometimes
        Debug.Log("[BattleSystem]Subscribing to event");
        GameManager2D.OnBattleStateChanged += OnBattleStateChanged; 
    }

    // Called when battle state changes
    private void OnBattleStateChanged(BattleState newState)
    {
        state = newState;

        switch (state)
        {
            case BattleState.START:
                // Setup the battle
                Debug.Log("[BattleSystem] Setting up battle system!");
                GameManager2D.instance.playAudio(); // play audio
                BattleSetup();
                break;
            case BattleState.PLAYERTURN:
                // Handle player's turn logic here
                break;
            case BattleState.ENEMYTURN:
                // Handle enemy's turn logic here
                break;
            case BattleState.WON:
                // Handle winning logic
                Debug.Log("[BattleSystem] You won the battle!");
                break;
            case BattleState.LOST:
                // Handle losing logic
                Debug.Log("[BattleSystem] You lost the battle!");
                break;
        }
    }


    // BattleSetup instantiates the player and enemy prefabs
    // BattleSetup will load in the prefabs and UIs for the battle and ends with updating it to either player turn or enemy turn
    void BattleSetup()
    {
        // Need to find a way to have an int where I can show up characters that the player picks before the fight 
        // and spawn them in the order they want, possibly a list or hashmap.
        GameObject player = Instantiate(playerPrefab, playerBattleStation);
        GameObject player2 = Instantiate(playerPrefab2, playerBattleStation2);
        playerUnit = player.GetComponent<Unit>();
        playerUnit2 = player2.GetComponent<Unit>();
        GameObject enemy = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemy.GetComponent<Unit>();

        // Display unitname to player
        enemyHud.text = enemyUnit.unitName;
        Player1Hud.text = playerUnit.unitName;
        Player2Hud.text = playerUnit2.unitName;
        // Player3Hud.text = playerUnit.unitName;
        // Player4Hud.text = playerUnit.unitName;

        // Probably do health bar here


        // starting player's turn
        // we can probably have a boolean to decide who starts first
        if (true) // I'll change this later
            GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
        else
            GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
    }
}
