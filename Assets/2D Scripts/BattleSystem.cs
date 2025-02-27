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
    public List<GameObject> buttons;
    public List<GameObject> buttonsForPlayer;
    public Transform enemyBattleStation;
    public BattleState state;


    // variables for buttons
    [SerializeField] public List<GameObject> buttonPrefabAllies;
    [SerializeField] public List<GameObject> buttonPrefabsAttack;
    public Transform buttonPanel;


    Unit enemyUnit;

    public Text enemyHud;
    [SerializeField] public List<Text> playerHuds;
    [SerializeField] public List<Text> playerHudsAttack;
    [SerializeField] public List<Transform> healthBarPanels;
    [SerializeField] public List<GameObject> healthBarsAllies; 
    [SerializeField] public List<Transform> healthBarEnemyPanels;
    [SerializeField] public List<GameObject> healthBarsEnemies; 
    GameObject enemyHealth;
    public List<Unit> playerUnits;

    void Awake()
    {
        // We want to do this during awake since Start() does it in a different order sometimes
        Debug.Log("[BattleSystem]Subscribing to event");
        GameManager2D.OnBattleStateChanged += OnBattleStateChanged; 
        foreach (var hud in playerHudsAttack)
        {
            hud.gameObject.SetActive(false);
        }
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
                // loading in all the buttons then disaible the ones we dont need at the start
                BattleSetup();
                playerTurnSetup();
                playerSelectSetup();
                GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
                break;
            case BattleState.PLAYERTURN:
                // Handle player's turn logic here

                // setting the buttons up for the player
                for (int i = 0; i < buttonsForPlayer.Count; i++) {
                    buttonsForPlayer[i].SetActive(false);
                    playerHudsAttack[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < buttons.Count; i++) {
                    Debug.Log("Button " + i);
                    buttons[i].SetActive(true); // activate the select character button
                }
                if (buttons.Count > 0) { // aparently it crashes at the start because it tries to laod in when buttons do not exist, lets not do that
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttons[0]);
                }
                break;
            case BattleState.ENEMYTURN:
                // Handle enemy's turn logic here
                GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
                break;
            case BattleState.WON:
                // Handle winning logic
                Debug.Log("[BattleSystem] You won the battle!");
                UnityEditor.EditorApplication.isPlaying = false;
                break;
            case BattleState.LOST:
                // Handle losing logic
                Debug.Log("[BattleSystem] You lost the battle!");
                UnityEditor.EditorApplication.isPlaying = false;
                break;
        }
    }


    // BattleSetup instantiates the player and enemy prefabs
    // BattleSetup will load in the prefabs and UIs for the battle and ends with updating it to either player turn or enemy turn
    void BattleSetup()
    {
        instantiatePlayers();
        // instantiate enemies (I'll change enemies to a list later)
        GameObject enemy = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemy.GetComponent<Unit>();
        enemyUnit.SetStats(100, 10, 5, 100, "Enemy", 1, 100);
        enemyHud.text = enemyUnit.unitName;
        enemyHealth = Instantiate(healthBarsEnemies[0], healthBarEnemyPanels[0]);
        enemyHealth.GetComponent<Slider>().maxValue = enemyUnit.maxHP;
        enemyHealth.GetComponent<Slider>().value = enemyUnit.currentHP;
    }

    void playerTurnSetup() {
        // spawn the buttons to select which character to use
        for (int i = 0; i < playerUnits.Count; i++) {
            int index = i; // create local copy of i for the lambda function
            buttons.Add(Instantiate(buttonPrefabAllies[i], buttonPanel));
            buttons[i].GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(index));

            // Probably do health bar here
            Instantiate(healthBarsAllies[i], healthBarPanels[i]);
            healthBarsAllies[i].GetComponent<Slider>().maxValue = playerUnits[i].maxHP;
            healthBarsAllies[i].GetComponent<Slider>().value = playerUnits[i].currentHP;
        }
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttons[0]);
    }

    
    void playerSelectSetup() {
        for (int i = 0; i < playerUnits.Count; i++) {
            playerHudsAttack[i].text = "Attack";
            playerHudsAttack[i].gameObject.SetActive(false);
            buttonsForPlayer.Add(Instantiate(buttonPrefabsAttack[i], buttonPanel));
            buttonsForPlayer[i].SetActive(false);
            int index = i;
            buttonsForPlayer[i].GetComponent<Button>().onClick.AddListener(() => AttackButtonClicked(index));
        }
    }

    void instantiatePlayers() {

        // instantiate players
        // would like to make it to where the 3D game manager will give some variables for current
        // HP and other current stats for the player. The file will hold the overal stats
        for (int i = 0; i < playerPrefab.Count; i++) {
            player.Add(Instantiate(playerPrefab[i], playerBattleStations[i]));
            playerUnits.Add(player[i].GetComponent<Unit>());
            Character firstCharacter = GameManager2D.instance.characterList.characters[i];
            playerUnits[i].SetStats(firstCharacter.health, firstCharacter.attack, firstCharacter.defense, 20, firstCharacter.name, 0, firstCharacter.energy);
        }

        // Display unitname to player
        for (int i = 0; i < 4; i++) {
           if (i < playerUnits.Count) {
               playerHuds[i].text = playerUnits[i].unitName;
           }
           else {
               playerHuds[i].text = "Empty";
           }
        }
    }

    void ToggleTextFirst(int index) {
        playerHuds[index].gameObject.SetActive(false);
    }

    void ToggleTextSecond(int index) {

        for (int i = 0; i < player.Count; i++) {
            buttons[i].SetActive(false);
        }

        buttonsForPlayer[index].SetActive(true);
        playerHudsAttack[index].gameObject.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttonsForPlayer[index]);
    }

    void OnButtonClicked(int index) {
        Debug.Log("Button " + index );
        ToggleTextFirst(index); // get rid of the text
        ToggleTextSecond(index); // spawn in next buttons and text for decisions
    }

    void AttackButtonClicked(int index) {
        buttonsForPlayer[index].SetActive(false);
        playerHudsAttack[index].gameObject.SetActive(false);
        playerHuds[index].gameObject.SetActive(true);
        //enemyUnit.healthChange(-playerUnits[index].unitAttack());
        enemyLogic(index);
        GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
    }

    void enemyLogic(int index) {
        enemyUnit.healthChange(-playerUnits[index].unitAttack());
        enemyHealth.GetComponent<Slider>().value = enemyUnit.currentHP;
        if (enemyUnit.currentHP <= 0) {
            GameManager2D.instance.UpdateBattleState(BattleState.WON);
        }
    }
}
