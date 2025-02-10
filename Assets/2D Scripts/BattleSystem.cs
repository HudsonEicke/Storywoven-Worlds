using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// We will use an ENUM to keep track of the battle state
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public BattleState state;


    Unit playerUnit, enemyUnit;

    // text variable
    public Text dialogueText;
    void Start()
    {
        state = BattleState.START; // We want to start the battle in the START state
        BattleSetup();
    }

    // BattleSetup instantiates the player and enemy prefabs
    void BattleSetup()
    {
        GameObject player = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = player.GetComponent<Unit>();
        GameObject enemy = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemy.GetComponent<Unit>();

        // Display unitname to player
        dialogueText.text = "NAME: " + enemyUnit.unitName + ".";
    }
}
