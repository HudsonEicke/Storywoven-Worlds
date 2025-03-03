using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameBattle;
using System;
using Unity.VisualScripting;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] public List<Transform> playerBattleStations;
    [SerializeField] public List<GameObject> playerPrefab;

    // Lists to store instantiated players and UI elements
    public List<GameObject> player = new List<GameObject>();
    public List<GameObject> buttons = new List<GameObject>();
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
    // Enemy and player UI stuff
    [SerializeField] public List<Text> playerHuds;
    [SerializeField] public List<Text> playerHudsAttack;
    [SerializeField] public List<Text> playerHudsSkill;

    // player skills
    [SerializeField] public List<Text> player1SkillOptions;
    [SerializeField] public List<GameObject> player1SkillButtons;
    [SerializeField] public List<Text> player2SkillOptions;
    [SerializeField] public List<GameObject> player2SkillButtons;

    // Health bar UI stuff
    [SerializeField] public List<Transform> healthBarPanels;
    [SerializeField] public List<GameObject> healthBarsAllies; 
    public List<GameObject> allyHealthBars = new List<GameObject>();

    List<GameObject> player1SkillButtonsSelect = new List<GameObject>();
    List<GameObject> player2SkillButtonsSelect = new List<GameObject>();

    public List<Unit> playerUnits = new List<Unit>();
    public List<skill> playerOneSkills = new List<skill>();
    public List<skill> playerTwoSkills = new List<skill>();

    int PlayerCountTurn = 0;

    public List<EnemySystem.EnemyHealthAndInfo> enemyList;

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
                player2SkillSetup();
                enemyList = GameManager2D.instance.enemyList; // easier to reference
                GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
                break;
            case BattleState.PLAYERTURN:
                foreach (var hud in playerHudsAttack) hud.gameObject.SetActive(false);
                foreach (var hud in playerHudsSkill) hud.gameObject.SetActive(false);
                OnButtonClicked(PlayerCountTurn);
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
    }

    // function to set up the UI for each ally character
    void playerTurnSetup()
    {
        for (int i = 0; i < playerUnits.Count; i++)
        {
            int index = i;
            GameObject button = Instantiate(buttonPrefabAllies[i], buttonPanel);
            button.GetComponent<Button>().onClick.AddListener(() => HealingDone(index));
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
        player1SkillButtonsSelect.Add(Instantiate(player1SkillButtons[0], buttonPanel));
        player1SkillButtonsSelect[0].GetComponent<Button>().onClick.AddListener(() => SlashButtonClicked(0));
        player1SkillButtonsSelect[0].SetActive(false);
    }

    void player2SkillSetup() {
        GameObject healObject = new GameObject("HealSkill");
        healSkill newSkill = healObject.AddComponent<healSkill>();
        Skill firstSkill = GameManager2D.instance.skillListPlayer2.P2Skills[0];
        newSkill.Setskill(firstSkill.name, firstSkill.description, firstSkill.attack, firstSkill.cost, firstSkill.type, firstSkill.healAmt);
        playerTwoSkills.Add(newSkill);

        // set up the button and text
        player2SkillOptions[0].text = firstSkill.name;
        player2SkillOptions[0].gameObject.SetActive(false);
        player2SkillButtonsSelect.Add(Instantiate(player2SkillButtons[0], buttonPanel));
        player2SkillButtonsSelect[0].GetComponent<Button>().onClick.AddListener(() => healButtonClicked(1));
        player2SkillButtonsSelect[0].SetActive(false);
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
        for (int i = 0; i < 2; i++) {
            foreach (var button in buttonsForPlayer[i].buttonsForPlayer)
            {
                button.SetActive(false);
            }
        }
        Debug.Log("Skill button clicked for player: " + index);
        // doing skill stuff here
        ToggleTextFirst(index);
        foreach (var button in buttons) button.SetActive(false);
        if (index == 0)
            player1SkillOptions[0].gameObject.SetActive(true);
        else
            player2SkillOptions[0].gameObject.SetActive(true);
        playerHudsAttack[index].gameObject.SetActive(false);
        playerHudsSkill[index].gameObject.SetActive(false);
        for (int i = 0; i < 2; i++)
        {
            buttonsForPlayer[index].buttonsForPlayer[i].SetActive(false);
        }
        if (index == 0) {
            player1SkillButtonsSelect[0].SetActive(true);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(player1SkillButtonsSelect[0]);
        }
        else {
            player2SkillButtonsSelect[0].SetActive(true);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(player2SkillButtonsSelect[0]);
        }
    }

    void SlashButtonClicked(int index) {
        Debug.Log("Slash button clicked");
        player1SkillOptions[0].gameObject.SetActive(false);
        player1SkillButtonsSelect[index].SetActive(false);

        // DO EVENT SYSTEM CALL OR SOMTHING HERE!!!!
        playerOneSkills[0].PlayMinigame((result) => {
            if (result == 1)    {
                Debug.Log("Player succeeded in minigame!");
                playerHuds[index].gameObject.SetActive(true);
                allyHealthBars[index].SetActive(true);
                healthBarPanels[index].gameObject.SetActive(true);
                enemyList[2].enemyUnit.healthChange(-1 * playerOneSkills[0].skillInflict());
                enemyList[2].enemyHealth.GetComponent<Slider>().value = enemyList[2].enemyUnit.getCurrentHP();

                // goin back to the enemy turn
                if (enemyList[2].enemyUnit.getCurrentHP() <= 0)
                    GameManager2D.instance.UpdateBattleState(BattleState.WON);
                else {
                    if (PlayerCountTurn < playerUnits.Count - 1)
                    {
                        PlayerCountTurn++;
                        GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
                    }
                    else
                    {
                        PlayerCountTurn = 0;
                        GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
                    }
                }
            }
            else    {
                Debug.Log("Player failed in minigame!");
                playerHuds[index].gameObject.SetActive(true);
                allyHealthBars[index].SetActive(true);
                healthBarPanels[index].gameObject.SetActive(true);
                // goin back to the enemy turn
                if (enemyList[2].enemyUnit.getCurrentHP() <= 0)
                    GameManager2D.instance.UpdateBattleState(BattleState.WON);
                else {
                    if (PlayerCountTurn < playerUnits.Count - 1)
                    {
                        PlayerCountTurn++;
                        GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
                    }
                    else
                    {
                        PlayerCountTurn = 0;
                        GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
                    }
                }
            }
        });
    }

    void healButtonClicked(int index) {
        Debug.Log("Heal button clicked");
        player2SkillOptions[0].gameObject.SetActive(false);
        player2SkillButtonsSelect[0].SetActive(false);

        // DO HEAL LOGIC HERE
        foreach (var btn in buttons) btn.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttons[0]);
        for (int i = 0 ; i < playerUnits.Count; i++)
        {
            playerHuds[i].gameObject.SetActive(true);
            allyHealthBars[i].SetActive(true);
            healthBarPanels[i].gameObject.SetActive(true);
        }
    }

    void HealingDone(int index) {
        Debug.Log("HEALING PLAYER " + playerTwoSkills[0].skillHeal());
        playerUnits[index].healthChange(playerTwoSkills[0].skillHeal());
        allyHealthBars[index].GetComponent<Slider>().value = playerUnits[index].getCurrentHP();
        
        if (enemyList[2].enemyUnit.getCurrentHP() <= 0)
            GameManager2D.instance.UpdateBattleState(BattleState.WON);
        else {
            if (PlayerCountTurn < playerUnits.Count - 1)
            {
                PlayerCountTurn++;
                GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
            }
            else
            {
                PlayerCountTurn = 0;
                GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
            }
        }
    }

    void ApplyDamage(int index)
    {
        enemyList[2].enemyUnit.healthChange(-1 * playerUnits[index].unitAttack());
        enemyList[2].enemyHealth.GetComponent<Slider>().value = enemyList[2].enemyUnit.getCurrentHP();

        playerHuds[index].gameObject.SetActive(true);
        allyHealthBars[index].SetActive(true);
        healthBarPanels[index].gameObject.SetActive(true);

        if (enemyList[2].enemyUnit.getCurrentHP() <= 0)
            GameManager2D.instance.UpdateBattleState(BattleState.WON);
        else {
            if (PlayerCountTurn < playerUnits.Count - 1)
            {
                PlayerCountTurn++;
                GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
            }
            else
            {
                PlayerCountTurn = 0;
                GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
            }
        }
    }
}
