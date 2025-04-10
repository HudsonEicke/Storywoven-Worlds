using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameBattle;
using Random = UnityEngine.Random;
using System.Collections;
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
    [SerializeField] public List<Text> player3SkillOptions;
    [SerializeField] public List<GameObject> player3SkillButtons;

    List<GameObject> player1SkillButtonsSelect = new List<GameObject>();
    List<GameObject> player2SkillButtonsSelect = new List<GameObject>();
    List<GameObject> player3SkillButtonsSelect = new List<GameObject>();

    public List<skill> playerOneSkills = new List<skill>();
    public List<skill> playerTwoSkills = new List<skill>();
    public List<skill> playerThreeSkills = new List<skill>();

    int PlayerCountTurn = 0;

    public List<EnemySystem.EnemyHealthAndInfo> enemyList;
    public CharacterList characterList;

    public List<GameObject> enemySelectButtons = new List<GameObject>();
    [SerializeField] public GameObject enemySelectPrefab;
    public int currentPlayerSelected;
    public int currentEnemyCount;
    public int currentPlayerCount;
    public int totalEnemyCount;
    public static int first = 0;
    public bool gamestart = false;
    int currentPlayerForMouse = 0;
    GameObject lastSelected = null;
    int taunt = 0;

    int healSwitch = 0; // inefficient, I know...

    [SerializeField] Text statusText;

    void Awake()
    {
        Debug.Log("[BattleSystem] Subscribing to event");
        GameManager2D.OnBattleStateChanged += OnBattleStateChanged; 
    }
    
    void Update()
    {
        if (gamestart)
        {
            Cursor.lockState = CursorLockMode.Locked; // Hide and disable mouse movement
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; // Restore mouse control
            Cursor.visible = true;
        }

        if (gamestart)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(lastSelected); // take care of mouse clicking
                return; 
            }
        }
    }


    private void OnBattleStateChanged(BattleState newState)
{
    state = newState;
    
    switch (state)
    {
        case BattleState.SETUP:
            characterList = GameManager2D.instance.characterList;
            enemyList = GameManager2D.instance.enemyList;
            playerTurnSetup();
            playerSelectSetup();
            break;
        
        case BattleState.PREPARE:
            Debug.Log("[BattleSystem] Preparing battle system!");
            //playerTurnSetup();
            //playerSelectSetup();
            return;
        
        case BattleState.START:
            gamestart = true;
            if (first == 0)
            {
                player1SkillSetup();
                player2SkillSetup();
                player3SkillSetup();
            }
            first = 1;
            currentEnemyCount = GameManager2D.enemyCount;
            totalEnemyCount = currentEnemyCount;
            currentPlayerCount = GameManager2D.characterCount;
            PlayerCountTurn = 0;
            statusText.text = "Start of Battle";
            Debug.Log("[BattleSystem] Setting up battle system!");
            for (int i = 0; i < currentPlayerCount; i++) {
                //characterList.characters[i].playerUnit.revive();
                characterList.characters[i].playerHealth.GetComponent<Slider>().value = characterList.characters[i].playerUnit.getCurrentHP();
            }

            for (int i = 0; i < currentPlayerCount; i++)
                if (!characterList.characters[i].playerUnit.getDead())
                {
                    Debug.Log("Setting up player: " + i);
                    characterList.characters[i].playerUnit.gameObject.SetActive(true);
                    characterList.characters[i].playerHealth.gameObject.SetActive(true);
                    characterList.characters[i].healthBarPanel.gameObject.SetActive(true);
                    characterList.characters[i].playerHud.gameObject.SetActive(true);
                    characterList.characters[i].manaBarPanel.gameObject.SetActive(true);
                    characterList.characters[i].playerMana.gameObject.SetActive(true);
                }
            for (int i = 0; i < currentEnemyCount; i++)
            {
                Debug.Log("TEST ENEMY: " + i);
                enemyList[i].enemyUnit.revive();
                enemyList[i].enemyHealth.GetComponent<Slider>().value = enemyList[i].enemyUnit.getCurrentHP();
                enemyList[i].enemyUnit.gameObject.SetActive(true);
                enemyList[i].enemyHealth.gameObject.SetActive(true);
                enemyList[i].healthPanel.gameObject.SetActive(true);
                enemyList[i].enemyHud.gameObject.SetActive(true);
            }
            GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
            break;
        
        case BattleState.PLAYERTURN:
            if (PlayerCountTurn >= currentPlayerCount) {
                    PlayerCountTurn = 0;
                    GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
                }
                else {
                    statusText.text = "Player's Turn";
                    for (int i = 0; i < currentPlayerCount; i++) {
                        Debug.Log("Setting up player: " + i);

                        if (!characterList.characters[i].playerUnit.getDead())
                            continue;
                        characterList.characters[i].playerHudAttack.gameObject.SetActive(false);
                        characterList.characters[i].playerHudSkill.gameObject.SetActive(false);
                    }
                    Debug.Log("Setting up enemy buttons: " + currentEnemyCount);
                    for (int i = 0; i < totalEnemyCount; i++) {
                        if (enemyList[i].enemyUnit.getDead()) continue;
                        Debug.Log("Setting up enemy button: " + i);
                        enemySelectButtons[i].SetActive(false);
                    }
                    while (characterList.characters[PlayerCountTurn].playerUnit.getDead()) {
                        PlayerCountTurn++;
                        if (PlayerCountTurn >= currentPlayerCount)
                            GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
                    }
                    OnButtonClicked(PlayerCountTurn);
                }
                break;
        
        case BattleState.ENEMYTURN:
                statusText.text = "Enemie's Turn";
                foreach (var btn in buttons) btn.SetActive(false);
                for (int i = 0; i < characterList.characters.Count; i++) {
                    if (characterList.characters[i].playerUnit.getDead()) 
                        continue;
                    characterList.characters[i].playerHudAttack.gameObject.SetActive(false);
                    characterList.characters[i].playerHudSkill.gameObject.SetActive(false);
                }
                for (int i = 0; i < enemySelectButtons.Count; i++) {
                    if (enemyList[i].enemyUnit.getDead()) continue;
                        enemySelectButtons[i].SetActive(false);
                }
                if (currentEnemyCount <= 0)
                    GameManager2D.instance.UpdateBattleState(BattleState.WON);
                
                // Start enemy attack sequence
                StartCoroutine(EnemyAttackSequence());
                break;
        
        case BattleState.WON:
            gamestart = false;
            Debug.Log("[BattleSystem] You won the battle!");
            GameManager2D.instance.UpdateBattleState(BattleState.PREPARE);
            return;
        
        case BattleState.LOST:
            gamestart = false;
            Debug.Log("[BattleSystem] You lost the battle!");
            GameManager2D.instance.UpdateBattleState(BattleState.PREPARE);
            return;
    }
}

private IEnumerator EnemyAttackSequence()
{
    yield return new WaitForSeconds(1.0f);
    for (int i = 0; i < totalEnemyCount; i++)
    {
        if (enemyList[i].enemyUnit.getDead())
            continue;

        // Sum weights for each player and determine the target
        int sumWeights = 0;
        foreach (var character in characterList.characters)
        {
            if (!character.playerUnit.getDead())
                sumWeights += character.playerUnit.getWeight();
        }

        int randomint = Random.Range(0, sumWeights);
        for (int j = 0; j < characterList.characters.Count; j++)
        {
            if (characterList.characters[j].playerUnit.getDead())
                continue;
            randomint -= characterList.characters[j].playerUnit.getWeight();
            if (randomint <= 0)
            {
                randomint = j;
                break;
            }
        }

        Debug.Log("Enemy: " + i + " attacking player: " + randomint);
        characterList.characters[randomint].playerUnit.healthChange(-1 * enemyList[i].enemyUnit.unitAttack());
        characterList.characters[randomint].playerHealth.GetComponent<Slider>().value = characterList.characters[randomint].playerUnit.getCurrentHP();

        // Check if player died
        if (characterList.characters[randomint].playerUnit.getCurrentHP() <= 0)
        {
            characterList.characters[randomint].playerUnit.gameObject.SetActive(false);
            characterList.characters[randomint].playerHealth.gameObject.SetActive(false);
            characterList.characters[randomint].healthBarPanel.gameObject.SetActive(false);
            characterList.characters[randomint].playerHud.gameObject.SetActive(false);
            currentPlayerCount--;
        }

        if (currentPlayerCount <= 0)
        {
            GameManager2D.instance.UpdateBattleState(BattleState.LOST);
            yield break;
        }

        yield return new WaitForSeconds(1.0f); // Delay between attacks
    }

    // Check if players are still alive, then switch turn
    if (currentPlayerCount > 0)
    {
        if (taunt > 0)
        {
            taunt--;
            if (taunt == 0)
            {
                Debug.Log("Taunt Wore Off");
                int baseWeight = characterList.characters[2].playerUnit.getWeight() - characterList.characters[2].weight;
                characterList.characters[2].playerUnit.addWeight(-1 * baseWeight);
            }
        }

        GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
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
        newSkill.text = GameObject.Find("DirectionsSlash");
        newSkill.setup();
        playerOneSkills.Add(newSkill);

        // set up the button and text
        player1SkillOptions[0].text = firstSkill.name;
        player1SkillOptions[0].gameObject.SetActive(false);
        player1SkillButtonsSelect.Add(Instantiate(player1SkillButtons[0], buttonPanel));
        player1SkillButtonsSelect[0].GetComponent<Button>().onClick.AddListener(() => SlashButtonClicked(0));
        player1SkillButtonsSelect[0].SetActive(false);

        skillObject = new GameObject("fireballSkill");
        fireballSkill newSkillF = skillObject.AddComponent<fireballSkill>();
        Skill secondSkill = GameManager2D.instance.skillListPlayer1.P1Skills[1];
        newSkillF.Setskill(secondSkill.name, secondSkill.description, secondSkill.attack, secondSkill.cost, secondSkill.type, secondSkill.healAmt);

        newSkillF.fireball = GameObject.Find("Fireball");
        newSkillF.fireballBackground = GameObject.Find("FireballBackground");
        newSkillF.fireballFill = GameObject.Find("Firefill");
        newSkillF.text = GameObject.Find("DirectionsFireball");
        //newSkillF.fireballSlider = newSkillF.fireballFill.GetComponent<Slider>();
        newSkillF.setup();
        playerOneSkills.Add(newSkillF);

        //setup buttons and text
        player1SkillOptions[1].text = secondSkill.name;
        player1SkillOptions[1].gameObject.SetActive(false);
        player1SkillButtonsSelect.Add(Instantiate(player1SkillButtons[1], buttonPanel));
        player1SkillButtonsSelect[1].GetComponent<Button>().onClick.AddListener(() => FireballButtonClicked(0));
        player1SkillButtonsSelect[1].SetActive(false);

        skillObject = new GameObject("PierceSkill");
        pierceSkill newSkillP = skillObject.AddComponent<pierceSkill>();
        Skill thirdSkill = GameManager2D.instance.skillListPlayer1.P1Skills[2];
        newSkillP.Setskill(thirdSkill.name, thirdSkill.description, thirdSkill.attack, thirdSkill.cost, thirdSkill.type, thirdSkill.healAmt);
        newSkillP.sword = GameObject.Find("swordCollider");
        newSkillP.minigamebackground = GameObject.Find("PiercingFlameMinigameBackground");
        newSkillP.text = GameObject.Find("DirectionsPierce");
        newSkillP.target = GameObject.Find("Stab");
        newSkillP.setup();
        playerOneSkills.Add(newSkillP);

        //setup buttons and text
        player1SkillOptions[2].text = thirdSkill.name;
        player1SkillOptions[2].gameObject.SetActive(false);
        player1SkillButtonsSelect.Add(Instantiate(player1SkillButtons[2], buttonPanel));
        player1SkillButtonsSelect[2].GetComponent<Button>().onClick.AddListener(() => PierceButtonClicked(0));
        player1SkillButtonsSelect[2].SetActive(false);

        skillObject = new GameObject("flameShowerSkill");
        flameShowerSkill newSkillFS = skillObject.AddComponent<flameShowerSkill>();
        Skill fourthSkill = GameManager2D.instance.skillListPlayer1.P1Skills[3];
        newSkillFS.Setskill(fourthSkill.name, fourthSkill.description, fourthSkill.attack, fourthSkill.cost, fourthSkill.type, fourthSkill.healAmt);
        newSkillFS.minigamebackground = GameObject.Find("miniGameBackgroundFlame");
        newSkillFS.slash = GameObject.Find("FSlash");
        newSkillFS.target = GameObject.Find("FTarget");
        newSkillFS.setup();
        playerOneSkills.Add(newSkillFS);

        //setup butons and text
        player1SkillOptions[3].text = fourthSkill.name;
        player1SkillOptions[3].gameObject.SetActive(false);
        player1SkillButtonsSelect.Add(Instantiate(player1SkillButtons[3], buttonPanel));
        player1SkillButtonsSelect[3].GetComponent<Button>().onClick.AddListener(() => flameShowerButtonClicked(0));
        player1SkillButtonsSelect[3].SetActive(false);
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

        GameObject healObject2 = new GameObject("lightArrowSkill");
        lightArrowSkill newSkill2 = healObject2.AddComponent<lightArrowSkill>();
        Skill secondSkill = GameManager2D.instance.skillListPlayer2.P2Skills[1];
        newSkill2.Setskill(secondSkill.name, secondSkill.description, secondSkill.attack, secondSkill.cost, secondSkill.type, secondSkill.healAmt);
        newSkill2.minigamebackground = GameObject.Find("lightArrowBackground");
        newSkill2.arrow = GameObject.Find("Arrow");
        newSkill2.target = GameObject.Find("ArrowTarget");
        newSkill2.text = GameObject.Find("DirectionsArrow");
        newSkill2.setup();
        playerTwoSkills.Add(newSkill2);

        // set up the button and text
        player2SkillOptions[1].text = secondSkill.name;
        player2SkillOptions[1].gameObject.SetActive(false);
        player2SkillButtonsSelect.Add(Instantiate(player2SkillButtons[1], buttonPanel));
        player2SkillButtonsSelect[1].GetComponent<Button>().onClick.AddListener(() => lightArrowButtonClicked(1));
        player2SkillButtonsSelect[1].SetActive(false);

        GameObject healObject3 = new GameObject("soothingArraysSkill");
        soothingArraysSkill newSkill3 = healObject3.AddComponent<soothingArraysSkill>();
        Skill thirdSkill = GameManager2D.instance.skillListPlayer2.P2Skills[2];
        newSkill3.Setskill(thirdSkill.name, thirdSkill.description, thirdSkill.attack, thirdSkill.cost, thirdSkill.type, thirdSkill.healAmt);
        playerTwoSkills.Add(newSkill3);

        // set up the button and text
        player2SkillOptions[2].text = thirdSkill.name;
        player2SkillOptions[2].gameObject.SetActive(false);
        player2SkillButtonsSelect.Add(Instantiate(player2SkillButtons[2], buttonPanel));
        player2SkillButtonsSelect[2].GetComponent<Button>().onClick.AddListener(() => soothingArraysButtonsClicked(1));
        player2SkillButtonsSelect[2].SetActive(false);

        GameObject healObject4 = new GameObject("heavensDelightSkill");
        heavensDelightSkill newSkill4 = healObject4.AddComponent<heavensDelightSkill>();
        Skill fourthSkill = GameManager2D.instance.skillListPlayer2.P2Skills[3];
        newSkill4.Setskill(fourthSkill.name, fourthSkill.description, fourthSkill.attack, fourthSkill.cost, fourthSkill.type, fourthSkill.healAmt);
        playerTwoSkills.Add(newSkill4);

        // set up the button and text
        player2SkillOptions[3].text = fourthSkill.name;
        player2SkillOptions[3].gameObject.SetActive(false);
        player2SkillButtonsSelect.Add(Instantiate(player2SkillButtons[3], buttonPanel));
        player2SkillButtonsSelect[3].GetComponent<Button>().onClick.AddListener(() => heavensDelightButtonClicked(1));
        player2SkillButtonsSelect[3].SetActive(false);
        
    }

    void player3SkillSetup() {
        GameObject skillObject = new GameObject("RockyTauntSkill");
        RockyTauntSkill newSkill = skillObject.AddComponent<RockyTauntSkill>();
        Skill firstSkill = GameManager2D.instance.skillListPlayer3.P3Skills[0];
        newSkill.Setskill(firstSkill.name, firstSkill.description, firstSkill.attack, firstSkill.cost, firstSkill.type, firstSkill.healAmt);

        newSkill.minigamebackground = GameObject.Find("MiniGameBackground2");
        newSkill.leftFoot = GameObject.Find("leftStomp");
        newSkill.rightFoot = GameObject.Find("rightStomp");
        newSkill.Smash = GameObject.Find("Smash");
        newSkill.leftFootHit = GameObject.Find("Leftfoot");
        newSkill.rightFootHit = GameObject.Find("Rightfoot");
        newSkill.SmashHit = GameObject.Find("SmashHit");
        newSkill.left = GameObject.Find("DirectionsLeft");
        newSkill.right = GameObject.Find("DirectionsRight");
        newSkill.space = GameObject.Find("DirectionsSpace");
        newSkill.COUNT = GameObject.Find("COUNT");
        newSkill.setup();
        playerThreeSkills.Add(newSkill);

        // set up the button and text
        player3SkillOptions[0].text = firstSkill.name;
        player3SkillOptions[0].gameObject.SetActive(false);
        player3SkillButtonsSelect.Add(Instantiate(player3SkillButtons[0], buttonPanel));
        player3SkillButtonsSelect[0].GetComponent<Button>().onClick.AddListener(() => rockyTauntButtonClicked(2));
        player3SkillButtonsSelect[0].SetActive(false);

        GameObject skillObject2 = new GameObject("rumbleAndTumbleSkill");
        rumbleAndTumbleSkill newSkill2 = skillObject2.AddComponent<rumbleAndTumbleSkill>();
        Skill secondSkill = GameManager2D.instance.skillListPlayer3.P3Skills[1];
        newSkill2.Setskill(secondSkill.name, secondSkill.description, secondSkill.attack, secondSkill.cost, secondSkill.type, secondSkill.healAmt);
        newSkill2.minigamebackground = GameObject.Find("RumbleBackground");
        newSkill2.fist = GameObject.Find("RockFist");
        newSkill2.fillBar = GameObject.Find("Rumblefill");
        newSkill2.text = GameObject.Find("DirectionsRumble");
        newSkill2.setup();
        playerThreeSkills.Add(newSkill2);

        // set up the button and text
        player3SkillOptions[1].text = secondSkill.name;
        player3SkillOptions[1].gameObject.SetActive(false);
        player3SkillButtonsSelect.Add(Instantiate(player3SkillButtons[1], buttonPanel));
        player3SkillButtonsSelect[1].GetComponent<Button>().onClick.AddListener(() => rumbleAndTumbleButtonClicked(2)); 
        player3SkillButtonsSelect[1].SetActive(false);

        GameObject skillObject3 = new GameObject("rockyWallSkill");
        rockyWallSkill newSkill3 = skillObject3.AddComponent<rockyWallSkill>();
        Skill thirdSkill = GameManager2D.instance.skillListPlayer3.P3Skills[2];
        newSkill3.Setskill(thirdSkill.name, thirdSkill.description, thirdSkill.attack, thirdSkill.cost, thirdSkill.type, thirdSkill.healAmt);
        newSkill3.rockWall = GameObject.Find("RockWall");
        newSkill3.setup();
        playerThreeSkills.Add(newSkill3);

        //set up the button and text
        player3SkillOptions[2].text = thirdSkill.name;
        player3SkillOptions[2].gameObject.SetActive(false);
        player3SkillButtonsSelect.Add(Instantiate(player3SkillButtons[2], buttonPanel));
        player3SkillButtonsSelect[2].GetComponent<Button>().onClick.AddListener(() => rockyWallButtonClicked(2));
        player3SkillButtonsSelect[2].SetActive(false);

        GameObject skillObject4 = new GameObject("ragnaROCKSkill");
        ragnaROCKSkill newSkill4 = skillObject4.AddComponent<ragnaROCKSkill>();
        Skill fourthSkill = GameManager2D.instance.skillListPlayer3.P3Skills[3];
        newSkill4.Setskill(fourthSkill.name, fourthSkill.description, fourthSkill.attack, fourthSkill.cost, fourthSkill.type, fourthSkill.healAmt);
        newSkill4.minigamebackground = GameObject.Find("ragnaROCKBackground");
        newSkill4.fist = GameObject.Find("RagnaFist");
        newSkill4.hitTarget = GameObject.Find("HitTarget");
        newSkill4.missTarget = GameObject.Find("MissTarget");
        newSkill4.text = GameObject.Find("DirectionsRagna");
        newSkill4.setup();
        playerThreeSkills.Add(newSkill4);

        //set up the button and text
        player3SkillOptions[3].text = fourthSkill.name;
        player3SkillOptions[3].gameObject.SetActive(false);
        player3SkillButtonsSelect.Add(Instantiate(player3SkillButtons[3], buttonPanel));
        player3SkillButtonsSelect[3].GetComponent<Button>().onClick.AddListener(() => ragnaROCKButtonClicked(2));
        player3SkillButtonsSelect[3].SetActive(false);
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
        //characterList.characters[index].playerHealth.SetActive(false);
        //characterList.characters[index].healthBarPanel.gameObject.SetActive(false);
    }

    void ToggleTextSecond(int index)
    {
        foreach (var button in buttons) button.SetActive(false);
        foreach (var button in buttonsForPlayer[index].buttonsForPlayer) button.SetActive(true);

        characterList.characters[index].playerHudAttack.gameObject.SetActive(true);
        characterList.characters[index].playerHudSkill.gameObject.SetActive(true);

        currentPlayerForMouse = index;
        lastSelected = buttonsForPlayer[index].buttonsForPlayer[0];
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttonsForPlayer[index].buttonsForPlayer[0]);
    }

    void AttackButtonClicked(int index)
    {   
        characterList.characters[index].playerHudAttack.gameObject.SetActive(false);
        characterList.characters[index].playerHudSkill.gameObject.SetActive(false);
        int buttonToStart = 0;
        Debug.Log("Attack button clicked for player: " + index);
        currentPlayerSelected = index;
        buttonsForPlayer[currentPlayerSelected].buttonsForPlayer[0].SetActive(false);
        buttonsForPlayer[currentPlayerSelected].buttonsForPlayer[1].SetActive(false);
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            if (enemyList[i].enemyUnit.getDead()) continue;
            enemySelectButtons[i].SetActive(true);
        }
        while (enemyList[buttonToStart].enemyUnit.getDead())
            buttonToStart++;
        if (buttonToStart < enemySelectButtons.Count)
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(enemySelectButtons[buttonToStart]);
        else
            GameManager2D.instance.UpdateBattleState(BattleState.WON);
        lastSelected = enemySelectButtons[buttonToStart];
    }

    void SkillButtonClicked(int index)
    {
        for (int i = 0; i < 3; i++) {
            foreach (var button in buttonsForPlayer[i].buttonsForPlayer)
            {
                button.SetActive(false);
            }
        }
        Debug.Log("Skill button clicked for player: " + index);
        // doing skill stuff here
        ToggleTextFirst(index);
        foreach (var button in buttons) button.SetActive(false);
        if (index == 0) {
            player1SkillOptions[0].gameObject.SetActive(true);
            player1SkillOptions[1].gameObject.SetActive(true);
            player1SkillOptions[2].gameObject.SetActive(true);
            player1SkillOptions[3].gameObject.SetActive(true);
        }
        else if (index == 1) {
            player2SkillOptions[0].gameObject.SetActive(true);
            player2SkillOptions[1].gameObject.SetActive(true);
            player2SkillOptions[2].gameObject.SetActive(true);
            player2SkillOptions[3].gameObject.SetActive(true);
        }
        else {
            player3SkillOptions[0].gameObject.SetActive(true);
            player3SkillOptions[1].gameObject.SetActive(true);
            player3SkillOptions[2].gameObject.SetActive(true);
            player3SkillOptions[3].gameObject.SetActive(true);
        }
        characterList.characters[index].playerHudAttack.gameObject.SetActive(false);
        characterList.characters[index].playerHudSkill.gameObject.SetActive(false);
        for (int i = 0; i < 2; i++)
        {
            buttonsForPlayer[index].buttonsForPlayer[i].SetActive(false);
        }
        if (index == 0) {
            player1SkillButtonsSelect[0].SetActive(true);
            player1SkillButtonsSelect[1].SetActive(true);
            player1SkillButtonsSelect[2].SetActive(true);
            player1SkillButtonsSelect[3].SetActive(true);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(player1SkillButtonsSelect[0]);
            lastSelected = player1SkillButtonsSelect[0];
        }
        else if (index == 1) {
            player2SkillButtonsSelect[0].SetActive(true);
            player2SkillButtonsSelect[1].SetActive(true);
            player2SkillButtonsSelect[2].SetActive(true);
            player2SkillButtonsSelect[3].SetActive(true);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(player2SkillButtonsSelect[0]);
            lastSelected = player2SkillButtonsSelect[0];
        }
        else {
            player3SkillButtonsSelect[0].SetActive(true);
            player3SkillButtonsSelect[1].SetActive(true);
            player3SkillButtonsSelect[2].SetActive(true);
            player3SkillButtonsSelect[3].SetActive(true);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(player3SkillButtonsSelect[0]);
            lastSelected = player3SkillButtonsSelect[0];
        }
    }

    void SlashButtonClicked(int index) {
        Debug.Log("Slash button clicked");
        for (int i = 0; i < player1SkillButtons.Count; i++) {
            player1SkillOptions[i].gameObject.SetActive(false);
            player1SkillButtonsSelect[i].SetActive(false);
        }
        // get the enemy index to attack
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            int enemyindex = i;
            if (!enemyList[i].enemyUnit.getDead()) {
                enemySelectButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                enemySelectButtons[i].GetComponent<Button>().onClick.AddListener(() => commenceSlashButton(index, enemyindex));
            }
        }


        int buttonToStart = 0;
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            if (enemyList[i].enemyUnit.getDead()) continue;
            enemySelectButtons[i].SetActive(true);
        }
        while (enemyList[buttonToStart].enemyUnit.getDead())
            buttonToStart++;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(enemySelectButtons[buttonToStart]);
        lastSelected = enemySelectButtons[buttonToStart];
    }

    void FireballButtonClicked(int index) {
        Debug.Log("Fireball button clicked");

        for (int i = 0; i < player1SkillButtons.Count; i++) {
            player1SkillOptions[i].gameObject.SetActive(false);
            player1SkillButtonsSelect[i].SetActive(false);
        }
        // get the enemy index to attack
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            int enemyindex = i;
            if (!enemyList[i].enemyUnit.getDead()) {
                enemySelectButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                enemySelectButtons[i].GetComponent<Button>().onClick.AddListener(() => commenceFireballButton(index, enemyindex));
            }
        }

        int buttonToStart = 0;
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            if (enemyList[i].enemyUnit.getDead()) continue;
            enemySelectButtons[i].SetActive(true);
        }
        while (enemyList[buttonToStart].enemyUnit.getDead())
            buttonToStart++;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(enemySelectButtons[buttonToStart]);
        lastSelected = enemySelectButtons[buttonToStart];

    }

    void PierceButtonClicked(int index) {
        Debug.Log("Pierce button clicked");

        for (int i = 0; i < player1SkillButtons.Count; i++) {
            player1SkillOptions[i].gameObject.SetActive(false);
            player1SkillButtonsSelect[i].SetActive(false);
        }
        // get the enemy index to attack
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            int enemyindex = i;
            if (!enemyList[i].enemyUnit.getDead()) {
                enemySelectButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                enemySelectButtons[i].GetComponent<Button>().onClick.AddListener(() => commencePierceButton(index, enemyindex));
            }
        }

        int buttonToStart = 0;
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            if (enemyList[i].enemyUnit.getDead()) continue;
            enemySelectButtons[i].SetActive(true);
        }
        while (enemyList[buttonToStart].enemyUnit.getDead())
            buttonToStart++;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(enemySelectButtons[buttonToStart]);
        lastSelected = enemySelectButtons[buttonToStart];
    }

    void commenceSlashButton(int index, int enemyIndex) {

        for (int i = 0; i < enemySelectButtons.Count; i++) {
            if (enemyList[i].enemyUnit.getDead()) continue;
            enemySelectButtons[i].SetActive(false);
        }

        for (int i = 0; i < enemySelectButtons.Count; i++) {
            int origionalIndex = i;
            if (!enemyList[i].enemyUnit.getDead()) {
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

    void commencePierceButton(int index, int enemyIndex) {
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            if (enemyList[i].enemyUnit.getDead()) continue;
            enemySelectButtons[i].SetActive(false);
        }

        for (int i = 0; i < enemySelectButtons.Count; i++) {
            int origionalIndex = i;
            if (!enemyList[i].enemyUnit.getDead()) {
                enemySelectButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                enemySelectButtons[i].GetComponent<Button>().onClick.AddListener(() => ApplyDamage(origionalIndex));
            }
        }

        // EVENT SYS CALL FOR PIERCE
        playerOneSkills[2].PlayMinigame((result) => {
            if (result == 1)    {
                Debug.Log("Player succeeded in minigame!");
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                enemyList[enemyIndex].enemyUnit.healthChange(-1 * playerOneSkills[2].skillInflict());
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

    void commenceFireballButton(int index, int enemyIndex) {
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            if (enemyList[i].enemyUnit.getDead()) continue;
            enemySelectButtons[i].SetActive(false);
        }

        for (int i = 0; i < enemySelectButtons.Count; i++) {
            int origionalIndex = i;
            if (!enemyList[i].enemyUnit.getDead()) {
                enemySelectButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                enemySelectButtons[i].GetComponent<Button>().onClick.AddListener(() => ApplyDamage(origionalIndex));
            }
        }

        // EVENT SYS CALL FOR FIREBALL
        playerOneSkills[1].PlayMinigame((result) => {
            if (result == 1)    {
                Debug.Log("Player succeeded in minigame!");
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                enemyList[enemyIndex].enemyUnit.healthChange(-1 * playerOneSkills[1].skillInflict());
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
        healSwitch = 0;
        Debug.Log("Heal button clicked");
        for (int i = 0; i < player2SkillOptions.Count; i++) {
            player2SkillOptions[i].gameObject.SetActive(false);
            player2SkillButtonsSelect[i].SetActive(false);
        }

        // DO HEAL LOGIC HERE
        foreach (var btn in buttons) btn.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttons[0]);
        lastSelected = buttons[0];
        for (int i = 0 ; i < characterList.characters.Count; i++)
        {
            characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
            characterList.characters[i].playerHud.gameObject.SetActive(true);
            characterList.characters[i].playerHealth.SetActive(true);
        }
    }

    void lightArrowButtonClicked(int index) {
        Debug.Log("Light Arrow Button Clicked!");

        for (int i = 0; i < player2SkillButtons.Count; i++) {
            player2SkillOptions[i].gameObject.SetActive(false);
            player2SkillButtonsSelect[i].SetActive(false);
        }
        // get the enemy index to attack
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            int enemyindex = i;
            if (!enemyList[i].enemyUnit.getDead()) {
                enemySelectButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                enemySelectButtons[i].GetComponent<Button>().onClick.AddListener(() => commenceLightArrowButton(index, enemyindex));
            }
        }

        int buttonToStart = 0;
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            if (enemyList[i].enemyUnit.getDead()) continue;
            enemySelectButtons[i].SetActive(true);
        }
        while (enemyList[buttonToStart].enemyUnit.getDead())
            buttonToStart++;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(enemySelectButtons[buttonToStart]);
        lastSelected = enemySelectButtons[buttonToStart];
    }

    void commenceLightArrowButton(int index, int enemyindex) {
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            if (enemyList[i].enemyUnit.getDead()) continue;
            enemySelectButtons[i].SetActive(false);
        }

        for (int i = 0; i < enemySelectButtons.Count; i++) {
            int origionalIndex = i;
            if (!enemyList[i].enemyUnit.getDead()) {
                enemySelectButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                enemySelectButtons[i].GetComponent<Button>().onClick.AddListener(() => ApplyDamage(origionalIndex));
            }
        }
        // DO EVENT SYSTEM CALL OR SOMTHING HERE!!!!
        playerTwoSkills[1].PlayMinigame((result) => {
            if (result == 1)    {
                Debug.Log("Player succeeded in minigame!");
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                enemyList[enemyindex].enemyUnit.healthChange(-1 * playerTwoSkills[1].skillInflict());
                enemyList[enemyindex].enemyHealth.GetComponent<Slider>().value = enemyList[enemyindex].enemyUnit.getCurrentHP();
                if (enemyList[enemyindex].enemyUnit.getCurrentHP() <= 0) 
                    removeEnemy(enemyindex);
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

    void soothingArraysButtonsClicked(int index) {
        healSwitch = 1;
        Debug.Log("Heal button clicked");
        for (int i = 0; i < player2SkillOptions.Count; i++) {
            player2SkillOptions[i].gameObject.SetActive(false);
            player2SkillButtonsSelect[i].SetActive(false);
        }

        // DO HEAL LOGIC HERE
        foreach (var btn in buttons) btn.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttons[0]);
        lastSelected = buttons[0];
        for (int i = 0 ; i < characterList.characters.Count; i++)
        {
            characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
            characterList.characters[i].playerHud.gameObject.SetActive(true);
            characterList.characters[i].playerHealth.SetActive(true);
        }
    }

    void heavensDelightButtonClicked(int index) {
        Debug.Log("Heavens Delight button clicked");
        for (int i = 0; i < player2SkillOptions.Count; i++) {
            player2SkillOptions[i].gameObject.SetActive(false);
            player2SkillButtonsSelect[i].SetActive(false);
        }
        for (int i = 0; i < GameManager2D.characterCount; i++) {
            if (characterList.characters[i].playerUnit.getDead()) continue;
            characterList.characters[i].playerUnit.healthChange(playerTwoSkills[3].skillHeal());
            characterList.characters[i].playerHealth.GetComponent<Slider>().value = characterList.characters[i].playerUnit.getCurrentHP();
        }
        for (int i = 0 ; i < GameManager2D.characterCount; i++)
        {
            characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
            characterList.characters[i].playerHud.gameObject.SetActive(true);
            characterList.characters[i].playerHealth.SetActive(true);
        }
        checkplayerTurn();
    }

    void ragnaROCKButtonClicked(int index) {
        Debug.Log("Rocky Taunt button clicked");
        for (int i = 0; i < player3SkillButtons.Count; i++) {
            player3SkillOptions[i].gameObject.SetActive(false);
            player3SkillButtonsSelect[i].SetActive(false);
        }
        commenceRagnaROCKBUTTON(index, 0); // Do all enemies
    }

    void commenceRagnaROCKBUTTON(int index, int enemyIndex) {
        playerThreeSkills[3].PlayMinigame((result) => {
            if (result == 1)    {
                Debug.Log("Player succeeded in minigame!");
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                enemyList[enemyIndex].enemyUnit.healthChange(-1 * playerThreeSkills[3].skillInflict());
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

    void rockyWallButtonClicked(int index) {
        Debug.Log("Rocky Wall button clicked");
        for (int i = 0; i < player3SkillButtons.Count; i++) {
            player3SkillOptions[i].gameObject.SetActive(false);
            player3SkillButtonsSelect[i].SetActive(false);
        }
        playerThreeSkills[2].PlayMinigame((result) => {
            if (result == 1)    {
                taunt = 1;
                Debug.Log("Taunt Activated!");
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                characterList.characters[2].playerUnit.addWeight(10000);
                characterList.characters[2].playerUnit.healthChange(playerThreeSkills[2].skillHeal());
                characterList.characters[2].playerHealth.GetComponent<Slider>().value = characterList.characters[2].playerUnit.getCurrentHP();
                checkplayerTurn();
            }
        });
    }

    void rumbleAndTumbleButtonClicked(int index) {
        Debug.Log("Rocky Taunt button clicked");
        for (int i = 0; i < player3SkillButtons.Count; i++) {
            player3SkillOptions[i].gameObject.SetActive(false);
            player3SkillButtonsSelect[i].SetActive(false);
        }
        commenceRumbleAndTumbleButton(index, 0); // Do all enemies
    }

    void commenceRumbleAndTumbleButton(int index, int enemyIndex) {
        playerThreeSkills[1].PlayMinigame((result) => {
            if (result == 1)    {
                Debug.Log("Player succeeded in minigame!");
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                enemyList[enemyIndex].enemyUnit.healthChange(-1 * playerThreeSkills[3].skillInflict());
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

    void rockyTauntButtonClicked(int index) {
        Debug.Log("Rocky Taunt button clicked");
        for (int i = 0; i < player3SkillButtons.Count; i++) {
            player3SkillOptions[i].gameObject.SetActive(false);
            player3SkillButtonsSelect[i].SetActive(false);
        }
        commenceRockyTauntButton(index, 0); // Do all enemies
    }

    void flameShowerButtonClicked(int index) {
        Debug.Log("Flame Shower Button Clicked");
        for (int i = 0; i < player1SkillButtons.Count; i++) {
            player1SkillOptions[i].gameObject.SetActive(false);
            player1SkillButtonsSelect[i].SetActive(false);
        }
        commenceFlameShowerButtonClicked(index, 0); // Do all enemies
    }

    void commenceFlameShowerButtonClicked(int index, int enemyIndex) {

        // EVENT SYS CALL FOR FLAME SHOWER 
        playerOneSkills[3].PlayMinigame((result) => {
            if (result == 1)    {
                // attack all enemies
                for (int i = 0; i < enemySelectButtons.Count; i++) {
                    if (!enemyList[i].enemyUnit.getDead()) {
                        enemyList[i].enemyUnit.healthChange(-1 * playerOneSkills[3].skillInflict());
                        enemyList[i].enemyHealth.GetComponent<Slider>().value = enemyList[i].enemyUnit.getCurrentHP();
                        if (enemyList[i].enemyUnit.getCurrentHP() <= 0) 
                            removeEnemy(i);
                        // goin back to the enemy turn
                        if (currentEnemyCount <= 0)
                            GameManager2D.instance.UpdateBattleState(BattleState.WON);
                    }
                }
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                checkplayerTurn();
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

    void commenceRockyTauntButton(int index, int enemyIndex) {
        playerThreeSkills[0].PlayMinigame((result) => {
            if (result == 1)    {
                Debug.Log("Player succeeded in minigame!");
                if (taunt == 0) {
                    taunt = 3;
                    Debug.Log("Taunt Activated!");
                    characterList.characters[2].playerUnit.addWeight(25);
                }
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);

                // attack all enemies
                for (int i = 0; i < enemySelectButtons.Count; i++) {
                    if (!enemyList[i].enemyUnit.getDead()) {
                        enemyList[i].enemyUnit.healthChange(-1 * playerThreeSkills[0].skillInflict());
                        enemyList[i].enemyHealth.GetComponent<Slider>().value = enemyList[i].enemyUnit.getCurrentHP();
                        if (enemyList[i].enemyUnit.getCurrentHP() <= 0) 
                            removeEnemy(i);
                        // goin back to the enemy turn
                        if (currentEnemyCount <= 0)
                            GameManager2D.instance.UpdateBattleState(BattleState.WON);
                    }
                }

                checkplayerTurn();
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

    void HealingDone(int index) {
        Debug.Log("HEALING PLAYER " + playerTwoSkills[0].skillHeal());
        if (healSwitch == 0) {
            characterList.characters[index].playerUnit.healthChange(playerTwoSkills[0].skillHeal());
            characterList.characters[index].playerHealth.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getCurrentHP();
        }
        else {
            characterList.characters[index].playerUnit.healthChange(playerTwoSkills[2].skillHeal());
            characterList.characters[index].playerHealth.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getCurrentHP();
        }
        
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
        Debug.Log("Removing enemy at index: " + index);
        enemyList[index].enemy.SetActive(false);
        enemySelectButtons[index].SetActive(false);
        enemyList[index].enemy.gameObject.SetActive(false);
        enemyList[index].enemyUnit.gameObject.SetActive(false);
        enemyList[index].enemyHealth.gameObject.SetActive(false);
        enemyList[index].healthPanel.gameObject.SetActive(false);
        enemyList[index].enemyHud.gameObject.SetActive(false);
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