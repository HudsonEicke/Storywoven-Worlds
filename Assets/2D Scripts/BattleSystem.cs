using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameBattle;
using System;

// My plan is to read from a JSON, did some research and it seems like Unity has support for JSON as oppose to a normal CSV file

public class BattleSystem : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField] public List<Transform> playerBattleStations;
    [SerializeField] public List<GameObject> playerPrefab;
    public List<GameObject> player;
    public Transform enemyBattleStation;
    public BattleState state;

    // variables for buttons
    public GameObject buttonPrefabAlly1, buttonPrefabAlly2, buttonPrefabAlly3, buttonPrefabAlly4;
    public Transform buttonPanel;


    Unit enemyUnit;

    public Text enemyHud;
    [SerializeField] public List<Text> playerHuds;
    public List<Unit> playerUnits;

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
                playerTurnSetup();
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
        // instantiate players
        for (int i = 0; i < 2; i++) {
            player.Add(Instantiate(playerPrefab[i], playerBattleStations[i]));
            playerUnits.Add(player[i].GetComponent<Unit>());
            Character firstCharacter = GameManager2D.instance.characterList.characters[i];
            playerUnits[i].SetStats(firstCharacter.health, firstCharacter.attack, firstCharacter.defense, 100, firstCharacter.name, 1, firstCharacter.energy);
        }

        // instantiate enemies
        GameObject enemy = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemy.GetComponent<Unit>();
        enemyUnit.SetStats(100, 10, 5, 100, "Enemy", 1, 100);

        // Display unitname to player
        for (int i = 0; i < 4; i++) {
           if (i < playerUnits.Count) {
               playerHuds[i].text = playerUnits[i].unitName;
           }
           else {
               playerHuds[i].text = "Empty";
           }
        }
        enemyHud.text = enemyUnit.unitName;

        // Probably do health bar here

        // starting player's turn
        // we can probably have a boolean to decide who starts first
        if (true) // I'll change this later
            GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
        //else
            //GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
    }

    void playerTurnSetup() {
        // spawn the buttons to select which character to use
        GameObject button1 = Instantiate(buttonPrefabAlly1, buttonPanel);
        GameObject button2 = Instantiate(buttonPrefabAlly2, buttonPanel);
        GameObject button3 = Instantiate(buttonPrefabAlly3, buttonPanel);
        GameObject button4 = Instantiate(buttonPrefabAlly4, buttonPanel);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(button1);

    }
}
