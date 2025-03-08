using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameBattle;
using System;
using Unity.VisualScripting;

public class BattleSystem : MonoBehaviour
{
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

    // player skills
    [SerializeField] public List<Text> player1SkillOptions;
    [SerializeField] public List<GameObject> player1SkillButtons;
    [SerializeField] public List<Text> player2SkillOptions;
    [SerializeField] public List<GameObject> player2SkillButtons;

    List<GameObject> player1SkillButtonsSelect = new List<GameObject>();
    List<GameObject> player2SkillButtonsSelect = new List<GameObject>();

    public List<skill> playerOneSkills = new List<skill>();
    public List<skill> playerTwoSkills = new List<skill>();

    int PlayerCountTurn = 0;

    public List<EnemySystem.EnemyHealthAndInfo> enemyList;
    public CharacterList characterList;

    public List<GameObject> enemySelectButtons = new List<GameObject>();
    [SerializeField] public GameObject enemySelectPrefab;
    public int currentPlayerSelected;
    public int currentEnemyCount;

    [SerializeField] Text statusText;

    void Awake()
    {
        Debug.Log("[BattleSystem] Subscribing to event");
        GameManager2D.OnBattleStateChanged += OnBattleStateChanged; 
    }

    private void OnBattleStateChanged(BattleState newState) {
        StartCoroutine(HandleBattleStateChanged(newState));
    }

    private IEnumerator HandleBattleStateChanged(BattleState newState)
    {
        state = newState;

        switch (state)
        {
            case BattleState.START:
                statusText.text = "Start of Battle";
                Debug.Log("[BattleSystem] Setting up battle system!");
                // GameManager2D.instance.playAudio();
                characterList = GameManager2D.instance.characterList; // easier to reference
                enemyList = GameManager2D.instance.enemyList; // easier to reference
                currentEnemyCount = enemyList.Count;
                playerTurnSetup();
                playerSelectSetup();
                player1SkillSetup();
                player2SkillSetup();
                yield return new WaitForSeconds(1);
                GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
                break;
            case BattleState.PLAYERTURN:
                statusText.text = "Player's Turn";
                for (int i = 0; i < characterList.characters.Count; i++) {
                    characterList.characters[i].playerHudAttack.gameObject.SetActive(false);
                    characterList.characters[i].playerHudSkill.gameObject.SetActive(false);
                }
                for (int i = 0; i < enemySelectButtons.Count; i++) {
                    if (enemySelectButtons[i] == null) continue;
                    enemySelectButtons[i].SetActive(false);
                }
                OnButtonClicked(PlayerCountTurn);
                break;
            case BattleState.ENEMYTURN:
                statusText.text = "Enemie's Turn";
                foreach (var btn in buttons) btn.SetActive(false);
                for (int i = 0; i < characterList.characters.Count; i++) {
                    characterList.characters[i].playerHudAttack.gameObject.SetActive(false);
                    characterList.characters[i].playerHudSkill.gameObject.SetActive(false);
                }
                for (int i = 0; i < enemySelectButtons.Count; i++) {
                    if (enemySelectButtons[i] == null) continue;
                        enemySelectButtons[i].SetActive(false);
                }
                if (currentEnemyCount <= 0)
                    GameManager2D.instance.UpdateBattleState(BattleState.WON);

                yield return new WaitForSeconds(2);
                GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
                break;
            case BattleState.WON:
                Debug.Log("[BattleSystem] You won the battle!");
                statusText.text = "YOU WON!!!";
                //yield return new WaitForSeconds(2);
                // UnityEditor.EditorApplication.isPlaying = false;
                break;
            case BattleState.LOST:
                Debug.Log("[BattleSystem] You lost the battle!");
                statusText.text = "YOU Lost!!!";
                //yield return new WaitForSeconds(2);
                // UnityEditor.EditorApplication.isPlaying = false;
                break;
        }
    }

    // function to set up the button for healing skills
    void playerTurnSetup()
    {
        for (int i = 0; i < characterList.characters.Count; i++)
        {
            int index = i;
            GameObject button = Instantiate(buttonPrefabAllies[i], buttonPanel);
            button.GetComponent<Button>().onClick.AddListener(() => HealingDone(index));
            buttons.Add(button);
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            int index = i;
            GameObject enemySelect = Instantiate(enemySelectPrefab, enemyList[i].healthPanel);
            enemySelect.GetComponent<Button>().onClick.AddListener(() => ApplyDamage(index));
            enemySelectButtons.Add(enemySelect);
            enemySelectButtons[i].SetActive(false);
        }
    }

    // function to set up the player attack and skill buttons
    void playerSelectSetup()
    {
        for (int i = 0; i < characterList.characters.Count; i++)
        {
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

    void OnButtonClicked(int index)
    {
        Debug.Log("Button: " + index);
        ToggleTextFirst(index);
        ToggleTextSecond(index);
    }

    void ToggleTextFirst(int index)
    {
        characterList.characters[index].playerHud.gameObject.SetActive(false);
        characterList.characters[index].playerHealth.SetActive(false);
        characterList.characters[index].healthBarPanel.gameObject.SetActive(false);
    }

    void ToggleTextSecond(int index)
    {
        foreach (var button in buttons) button.SetActive(false);
        foreach (var button in buttonsForPlayer[index].buttonsForPlayer) button.SetActive(true);

        characterList.characters[index].playerHudAttack.gameObject.SetActive(true);
        characterList.characters[index].playerHudSkill.gameObject.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttonsForPlayer[index].buttonsForPlayer[0]);
    }

    void AttackButtonClicked(int index)
    {   
        int buttonToStart = 0;
        Debug.Log("Attack button clicked for player: " + index);
        currentPlayerSelected = index;
        buttonsForPlayer[currentPlayerSelected].buttonsForPlayer[0].SetActive(false);
        buttonsForPlayer[currentPlayerSelected].buttonsForPlayer[1].SetActive(false);
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            if (enemySelectButtons[i] == null) continue;
            enemySelectButtons[i].SetActive(true);
        }
        while (enemySelectButtons[buttonToStart] == null)
            buttonToStart++;
        if (buttonToStart < enemySelectButtons.Count)
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(enemySelectButtons[buttonToStart]);
        else
            GameManager2D.instance.UpdateBattleState(BattleState.WON);
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
        characterList.characters[index].playerHudAttack.gameObject.SetActive(false);
        characterList.characters[index].playerHudSkill.gameObject.SetActive(false);
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
        // get the enemy index to attack
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            int enemyindex = i;
            if (enemySelectButtons[i] != null) {
                enemySelectButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                enemySelectButtons[i].GetComponent<Button>().onClick.AddListener(() => commenceSlashButton(index, enemyindex));
            }
        }


        int buttonToStart = 0;
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            if (enemySelectButtons[i] == null) continue;
            enemySelectButtons[i].SetActive(true);
        }
        while (enemySelectButtons[buttonToStart] == null)
            buttonToStart++;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(enemySelectButtons[buttonToStart]);
    }

    void commenceSlashButton(int index, int enemyIndex) {

        for (int i = 0; i < enemySelectButtons.Count; i++) {
            if (enemySelectButtons[i] == null) continue;
            enemySelectButtons[i].SetActive(false);
        }

        for (int i = 0; i < enemySelectButtons.Count; i++) {
            int origionalIndex = i;
            if (enemySelectButtons[i] != null) {
                enemySelectButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                enemySelectButtons[i].GetComponent<Button>().onClick.AddListener(() => ApplyDamage(origionalIndex));
            }
        }
        // DO EVENT SYSTEM CALL OR SOMTHING HERE!!!!
        playerOneSkills[0].PlayMinigame((result) => {
            if (result == 1)    {
                Debug.Log("Player succeeded in minigame!");
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                enemyList[enemyIndex].enemyUnit.healthChange(-1 * playerOneSkills[0].skillInflict());
                enemyList[enemyIndex].enemyHealth.GetComponent<Slider>().value = enemyList[enemyIndex].enemyUnit.getCurrentHP();
                if (enemyList[enemyIndex].enemyUnit.getCurrentHP() <= 0) 
                    removeEnemy(enemyIndex);
                // goin back to the enemy turn
                if (currentEnemyCount <= 0)
                    GameManager2D.instance.UpdateBattleState(BattleState.WON);
                else {
                    checkplayerTurn();
                }
            }
            else    {
                Debug.Log("Player failed in minigame!");
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                // goin back to the enemy turn
                if (currentEnemyCount <= 0)
                    GameManager2D.instance.UpdateBattleState(BattleState.WON);
                else {
                    checkplayerTurn();
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
        for (int i = 0 ; i < characterList.characters.Count; i++)
        {
            characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
            characterList.characters[i].playerHud.gameObject.SetActive(true);
            characterList.characters[i].playerHealth.SetActive(true);
        }
    }

    void HealingDone(int index) {
        Debug.Log("HEALING PLAYER " + playerTwoSkills[0].skillHeal());

        characterList.characters[index].playerUnit.healthChange(playerTwoSkills[0].skillHeal());
        characterList.characters[index].playerHealth.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getCurrentHP();
        
        if (currentEnemyCount <= 0)
            GameManager2D.instance.UpdateBattleState(BattleState.WON);
        else {
            checkplayerTurn();
        }
    }

    void ApplyDamage(int index)
    {
        enemyList[index].enemyUnit.healthChange(-1 * characterList.characters[currentPlayerSelected].playerUnit.unitAttack());
        enemyList[index].enemyHealth.GetComponent<Slider>().value = enemyList[index].enemyUnit.getCurrentHP();

        characterList.characters[currentPlayerSelected].healthBarPanel.gameObject.SetActive(true);
        characterList.characters[currentPlayerSelected].playerHud.gameObject.SetActive(true);
        characterList.characters[currentPlayerSelected].playerHealth.SetActive(true);

        if (enemyList[index].enemyUnit.getCurrentHP() <= 0) 
            removeEnemy(index); // create a funciton to remove the enemy fully
        checkplayerTurn();
    }

    void removeEnemy(int index)
    {
        // TODO: check nulls and make sure to remove the enemy from the list correctly
        // try setting the empty space to NULL instead but we need to check for nulls
        Debug.Log("Removing enemy at index: " + index);
        enemyList[index].enemy.SetActive(false);
        GameObject buttonToRemove = enemySelectButtons[index];
        enemySelectButtons[index] = null;
        Destroy(buttonToRemove);
        currentEnemyCount--;

        // check if all enemies are gone, if so, battle won
        if (currentEnemyCount <= 0)
            GameManager2D.instance.UpdateBattleState(BattleState.WON);
    }

    void checkplayerTurn() {
        if (PlayerCountTurn < characterList.characters.Count - 1 && currentEnemyCount > 0) {
                PlayerCountTurn++;
                GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
        }
        else if (currentEnemyCount > 0) {
                PlayerCountTurn = 0;
                GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
        }
        else {
            GameManager2D.instance.UpdateBattleState(BattleState.WON);
        }
    }
}
