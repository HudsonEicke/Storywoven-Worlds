using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameBattle;
using System;
using Unity.VisualScripting;

public class BattleSystem : MonoBehaviour
{
    // Prefabs for enemy and players
    public GameObject enemyPrefab;
    [SerializeField] public List<Transform> playerBattleStations;
    [SerializeField] public List<GameObject> playerPrefab;

    // Lists to store instantiated players and UI elements
    public List<GameObject> player = new List<GameObject>();
    public List<GameObject> buttons = new List<GameObject>();
    public Transform enemyBattleStation;
    public BattleState state;

    // UI button prefabs for player actions
    [SerializeField] public List<GameObject> buttonPrefabAllies;
    [SerializeField] public List<GameObject> buttonPrefabsAttack;
    [SerializeField] public List<GameObject> buttonPrefabsSkill;

    // Class to hold player-specific button groups
    public class ButtonsForPlayers
    {
        public List<GameObject> buttonsForPlayer = new List<GameObject>();
    }

    public List<ButtonsForPlayers> buttonsForPlayer = new List<ButtonsForPlayers>();

    public Transform buttonPanel;

    Unit enemyUnit;
    public Text enemyHud;

    // Enemy and player UI stuff
    [SerializeField] public List<Text> playerHuds;
    [SerializeField] public List<Text> playerHudsAttack;
    [SerializeField] public List<Text> playerHudsSkill;

    // player skills
    [SerializeField] public List<Text> player1SkillOptions;
    [SerializeField] public List<GameObject> player1SkillButtons;

    // Health bar UI stuff
    [SerializeField] public List<Transform> healthBarPanels;
    [SerializeField] public List<GameObject> healthBarsAllies; 
    [SerializeField] public List<Transform> healthBarEnemyPanels;
    [SerializeField] public List<GameObject> healthBarsEnemies; 
    public List<GameObject> allyHealthBars = new List<GameObject>();
    GameObject enemyHealth;

    // TEMP
    GameObject button;

    public List<Unit> playerUnits = new List<Unit>();
    public List<skill> playerOneSkills = new List<skill>();

    void Awake()
    {
        Debug.Log("[BattleSystem] Subscribing to event");
        GameManager2D.OnBattleStateChanged += OnBattleStateChanged; 

        foreach (var hud in playerHudsAttack)
        {
            hud.gameObject.SetActive(false);
        }
    }

    private void OnBattleStateChanged(BattleState newState)
    {
        state = newState;

        switch (state)
        {
            case BattleState.START:
                Debug.Log("[BattleSystem] Setting up battle system!");
                GameManager2D.instance.playAudio();
                BattleSetup();
                playerTurnSetup();
                playerSelectSetup();
                player1SkillSetup();
                GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
                break;
            case BattleState.PLAYERTURN:
                foreach (var hud in playerHudsAttack) hud.gameObject.SetActive(false);
                foreach (var hud in playerHudsSkill) hud.gameObject.SetActive(false);
                foreach (var btn in buttons) btn.SetActive(true);

                if (buttons.Count > 0)
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttons[0]);
                break;
            case BattleState.ENEMYTURN:
                GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
                break;
            case BattleState.WON:
                Debug.Log("[BattleSystem] You won the battle!");
                UnityEditor.EditorApplication.isPlaying = false;
                break;
            case BattleState.LOST:
                Debug.Log("[BattleSystem] You lost the battle!");
                UnityEditor.EditorApplication.isPlaying = false;
                break;
        }
    }

    void BattleSetup()
    {
        instantiatePlayers();

        GameObject enemy = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemy.GetComponent<Unit>();
        enemyUnit.SetStats(100, 10, 5, 100, "Enemy", 1, 100);
        enemyHud.text = enemyUnit.getName();

        enemyHealth = Instantiate(healthBarsEnemies[0], healthBarEnemyPanels[0]);
        enemyHealth.GetComponent<Slider>().maxValue = enemyUnit.getMaxHP();
        enemyHealth.GetComponent<Slider>().value = enemyUnit.getCurrentHP();
    }

    // function to set up the UI for each ally character
    void playerTurnSetup()
    {
        for (int i = 0; i < playerUnits.Count; i++)
        {
            int index = i;
            GameObject button = Instantiate(buttonPrefabAllies[i], buttonPanel);
            button.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(index));
            buttons.Add(button);

            GameObject healthBar = Instantiate(healthBarsAllies[i], healthBarPanels[i]);
            healthBar.GetComponent<Slider>().maxValue = playerUnits[i].getMaxHP();
            healthBar.GetComponent<Slider>().value = playerUnits[i].getCurrentHP();
            allyHealthBars.Add(healthBar);
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttons[0]);
    }

    // function to set up the player move select
    void playerSelectSetup()
    {
        for (int i = 0; i < playerUnits.Count; i++)
        {
            playerHudsAttack[i].text = "Attack";
            playerHudsAttack[i].gameObject.SetActive(false);

            playerHudsSkill[i].text = "Skill";
            playerHudsSkill[i].gameObject.SetActive(false);

            ButtonsForPlayers buttonGroup = new ButtonsForPlayers();
            int index = i;
            buttonGroup.buttonsForPlayer.Add(Instantiate(buttonPrefabsAttack[i], buttonPanel));
            buttonGroup.buttonsForPlayer[0].GetComponent<Button>().onClick.AddListener(() => AttackButtonClicked(index));

            buttonGroup.buttonsForPlayer.Add(Instantiate(buttonPrefabsSkill[i], buttonPanel));
            buttonGroup.buttonsForPlayer[1].GetComponent<Button>().onClick.AddListener(() => SkillButtonClicked(index));

            buttonsForPlayer.Add(buttonGroup);

            foreach (var button in buttonGroup.buttonsForPlayer)
            {
                button.SetActive(false);
            }
        }
    }

    // function to set up player one skillset
    void player1SkillSetup() {
        GameObject skillObject = new GameObject("SlashSkill");
        slashSkill newSkill = skillObject.AddComponent<slashSkill>();
        Skill firstSkill = GameManager2D.instance.skillListPlayer1.P1Skills[0];
        newSkill.Setskill(firstSkill.name, firstSkill.description, firstSkill.attack, firstSkill.cost, firstSkill.type, firstSkill.healAmt);
        newSkill.minigamebackground = GameObject.Find("MiniGameBackground");
        newSkill.slash = GameObject.Find("Slash");
        newSkill.target = GameObject.Find("Target");
        newSkill.setup();
        playerOneSkills.Add(newSkill);

        // set up the button and text
        player1SkillOptions[0].text = firstSkill.name;
        player1SkillOptions[0].gameObject.SetActive(false);
        button = Instantiate(player1SkillButtons[0], buttonPanel);
        button.GetComponent<Button>().onClick.AddListener(() => SlashButtonClicked(0));
        button.SetActive(false);
    }

    void instantiatePlayers()
    {
        for (int i = 0; i < playerPrefab.Count; i++)
        {
            GameObject newPlayer = Instantiate(playerPrefab[i], playerBattleStations[i]);
            player.Add(newPlayer);

            Unit unit = newPlayer.GetComponent<Unit>();
            Character firstCharacter = GameManager2D.instance.characterList.characters[i];
            unit.SetStats(firstCharacter.health, firstCharacter.attack, firstCharacter.defense, 20, firstCharacter.name, 0, firstCharacter.energy);
            playerUnits.Add(unit);
        }

        for (int i = 0; i < playerHuds.Count; i++)
        {
            playerHuds[i].text = i < playerUnits.Count ? playerUnits[i].getName() : "Empty";
        }
    }

    void OnButtonClicked(int index)
    {
        Debug.Log("Button: " + index);
        ToggleTextFirst(index);
        ToggleTextSecond(index);
    }

    void ToggleTextFirst(int index)
    {
        playerHuds[index].gameObject.SetActive(false);
        allyHealthBars[index].SetActive(false);
        healthBarPanels[index].gameObject.SetActive(false);
    }

    void ToggleTextSecond(int index)
    {
        foreach (var button in buttons) button.SetActive(false);
        foreach (var button in buttonsForPlayer[index].buttonsForPlayer) button.SetActive(true);

        playerHudsAttack[index].gameObject.SetActive(true);
        playerHudsSkill[index].gameObject.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttonsForPlayer[index].buttonsForPlayer[0]);
    }

    void AttackButtonClicked(int index)
    {
        ApplyDamage(index);
    }

    void SkillButtonClicked(int index)
    {
        Debug.Log("Skill button clicked for player: " + index);
        // doing skill stuff here
        ToggleTextFirst(index);
        foreach (var button in buttons) button.SetActive(false);
        player1SkillOptions[0].gameObject.SetActive(true);
        playerHudsAttack[index].gameObject.SetActive(false);
        playerHudsSkill[index].gameObject.SetActive(false);
        for (int i = 0; i < 2; i++)
        {
            buttonsForPlayer[index].buttonsForPlayer[i].SetActive(false);
        }
        button.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(button);
    }

    void SlashButtonClicked(int index) {
        Debug.Log("Slash button clicked");
        player1SkillOptions[0].gameObject.SetActive(false);
        button.SetActive(false);

        // DO EVENT SYSTEM CALL OR SOMTHING HERE!!!!

        playerOneSkills[0].PlayMinigame((result) => {
            if (result == 1)
            {
                Debug.Log("Player succeeded in minigame!");
                // ApplyDamageSkill(index, playerOneSkills[0].skillInflict());
                // goin back to the enemy turn
                enemyUnit.healthChange(-1 * playerOneSkills[0].skillInflict());
                playerHuds[index].gameObject.SetActive(true);
                allyHealthBars[index].SetActive(true);
                healthBarPanels[index].gameObject.SetActive(true);
                enemyHealth.GetComponent<Slider>().value = enemyUnit.getCurrentHP();
                if (enemyUnit.getCurrentHP() <= 0)
                    GameManager2D.instance.UpdateBattleState(BattleState.WON);
                else
                    GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
            }
            else
            {
                Debug.Log("Player failed in minigame!");
                // goin back to the enemy turn
                playerHuds[index].gameObject.SetActive(true);
                allyHealthBars[index].SetActive(true);
                healthBarPanels[index].gameObject.SetActive(true);
                if (enemyUnit.getCurrentHP() <= 0)
                    GameManager2D.instance.UpdateBattleState(BattleState.WON);
                else
                    GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
            }
        });
    }

    void ApplyDamage(int index)
    {
        enemyUnit.healthChange(-1 * playerUnits[index].unitAttack());
        enemyHealth.GetComponent<Slider>().value = enemyUnit.getCurrentHP();

        playerHuds[index].gameObject.SetActive(true);
        allyHealthBars[index].SetActive(true);
        healthBarPanels[index].gameObject.SetActive(true);

        if (enemyUnit.getCurrentHP() <= 0)
            GameManager2D.instance.UpdateBattleState(BattleState.WON);
        else
            GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
    }
}
