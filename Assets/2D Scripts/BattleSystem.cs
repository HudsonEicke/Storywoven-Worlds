using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameBattle;
using System;

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public BattleState state;


    Unit playerUnit, enemyUnit;

    public Text enemyHud, Player1Hud, Player2Hud, Player3Hud, Player4Hud;
    void Start()
    {
        //state = BattleState.START; // Taken care of by the GameManager
        GameManager2D.instance.playAudio(); // play audio
        GameManager2D.OnBattleStateChanged += OnBattleStateChanged; // subscribe to the event
    }

    // Called when battle state changes
    private void OnBattleStateChanged(BattleState newState)
    {
        state = newState;

        switch (state)
        {
            case BattleState.START:
                // Setup the battle
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
                Debug.Log("You won the battle!");
                break;
            case BattleState.LOST:
                // Handle losing logic
                Debug.Log("You lost the battle!");
                break;
        }
    }


    // BattleSetup instantiates the player and enemy prefabs
    // BattleSetup will load in the prefabs and UIs for the battle and ends with updating it to either player turn or enemy turn
    void BattleSetup()
    {
        GameObject player = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = player.GetComponent<Unit>();
        GameObject enemy = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemy.GetComponent<Unit>();

        // Display unitname to player
        enemyHud.text = enemyUnit.unitName;
        Player1Hud.text = playerUnit.unitName;
        Player2Hud.text = playerUnit.unitName;
        Player3Hud.text = playerUnit.unitName;
        Player4Hud.text = playerUnit.unitName;

        // starting player's turn
        GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
    }
}
