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
    [SerializeField] public List<GameObject> buttonPrefabsItem;

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
    [SerializeField] public List<Text> backButtonsText;
    [SerializeField] public List<GameObject> backButtons;
    [SerializeField] public List<Text> itemButtonsText;
    [SerializeField] private List<Text> itemListText;
    [SerializeField] private List<Text> itemManaListText;
    [SerializeField] private List<Text> itemSHealthListText;
    [SerializeField] private List<Text> ManaCostText;
    [SerializeField] List<GameObject> healthPotionButtons;
    [SerializeField] List<GameObject> manaPotionButtons;
     [SerializeField] List<GameObject> SingleHealthPotionButtons;
    List<GameObject> healthPotionButtonsSelect = new List<GameObject>();
    List<GameObject> manaPotionButtonsSelect = new List<GameObject>();
    List<GameObject> SingleHealthPotionButtonsSelect = new List<GameObject>();

    List<GameObject> player1SkillButtonsSelect = new List<GameObject>();
    List<GameObject> player2SkillButtonsSelect = new List<GameObject>();
    List<GameObject> player3SkillButtonsSelect = new List<GameObject>();
    [SerializeField] private List<Transform> healthBarEnemyPanels; //fixing this
    List<GameObject> backButtonSelect = new List<GameObject>();
    [SerializeField] List<GameObject> enemyArrows = new List<GameObject>();

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
    public int playerTotalCount;
    public int first = 0;
    public bool gamestart = false;
    int currentPlayerForMouse = 0;
    GameObject lastSelected = null;
    int taunt = 0;
    int invis = 0;
    [SerializeField] Text invisText;
    public static BattleSystem Instance;

    int healSwitch = 0; // inefficient, I know...

    int level;

    [SerializeField] Text statusText;

    private List<Item> inventory;
    private InventoryManager inventoryManager2D;
    public Text displayText;
    private static List<Item> inventoryList;
    CameraShake cameraShake;

    void Awake()
    {
        Debug.Log("[BattleSystem] Subscribing to event");
        GameManager2D.OnBattleStateChanged += OnBattleStateChanged; 
        Instance = this;
    }

    private void OnDestroy()
    {
        GameManager2D.OnBattleStateChanged -= OnBattleStateChanged;
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
            inventoryManager2D = InventoryManager.Instance;
            break;
        
        case BattleState.PREPARE:
            Debug.Log("[BattleSystem] Preparing battle system!");
            //playerTurnSetup();
            //playerSelectSetup();
            AudioSystem2D.instance.stopBattleMusic();
            return;
        
        case BattleState.START:
            AudioSystem2D.instance.playBattleMusic();
            level = GameManager2D.instance.levels;

            // reset if batle ended earlier than when logic could stop invis
            invisText.gameObject.SetActive(false);
            Renderer render = characterList.characters[1].player.GetComponentInChildren<Renderer>();
            render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, 1f);

            gamestart = true;
            if (first == 0)
            {
                player1SkillSetup();
                player2SkillSetup();
                player3SkillSetup();
                // setup back buttons
                for (int i = 0; i < backButtons.Count; i++)
                {   
                    Debug.Log("Setting up back button: " + i);
                    backButtonsText[i].gameObject.SetActive(false);
                    backButtonSelect.Add(Instantiate(backButtons[i], buttonPanel));
                    int index = i;
                    backButtonSelect[i].GetComponent<Button>().onClick.AddListener(() => BackButtonClicked(index));
                    backButtonSelect[i].SetActive(false);
                }

                for (int i = 0; i < itemListText.Count; i++)
                {
                    itemListText[i].gameObject.SetActive(false);
                    healthPotionButtonsSelect.Add(Instantiate(healthPotionButtons[i], buttonPanel));
                    healthPotionButtonsSelect[i].SetActive(false);
                    itemManaListText[i].gameObject.SetActive(false);
                    manaPotionButtonsSelect.Add(Instantiate(manaPotionButtons[i], buttonPanel));
                    manaPotionButtonsSelect[i].SetActive(false);
                    itemSHealthListText[i].gameObject.SetActive(false);
                    SingleHealthPotionButtonsSelect.Add(Instantiate(SingleHealthPotionButtons[i], buttonPanel));
                    SingleHealthPotionButtonsSelect[i].SetActive(false);
                    int index = i;
                    healthPotionButtonsSelect[i].GetComponent<Button>().onClick.AddListener(() => commenceHealthPotion(index));
                    manaPotionButtonsSelect[i].GetComponent<Button>().onClick.AddListener(() => commenceManaPotion(index));
                    SingleHealthPotionButtonsSelect[i].GetComponent<Button>().onClick.AddListener(() => commenceSingleHealthPotion(index));
                }
            }
            first = 1;
            currentEnemyCount = GameManager2D.enemyCount;
            totalEnemyCount = currentEnemyCount;
            currentPlayerCount = GameManager2D.characterCount;
            playerTotalCount = currentPlayerCount;
            PlayerCountTurn = 0;
            invis = 0;
            int baseWeight = characterList.characters[1].playerUnit.getWeight() - characterList.characters[1].weight;
            characterList.characters[1].playerUnit.addWeight(-1 * baseWeight);
            statusText.text = "Start of Battle";
            Debug.Log("[BattleSystem] Setting up battle system!");
            for (int i = 0; i < currentPlayerCount; i++) {
                //characterList.characters[i].playerUnit.revive();
                characterList.characters[i].playerHealth.GetComponent<Slider>().value = characterList.characters[i].playerUnit.getCurrentHP();
                characterList.characters[i].playerMana.GetComponent<Slider>().value = characterList.characters[i].playerUnit.getEnergy();
                characterList.characters[i].playerUnit.setPassiveHeal(0);
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
                enemyList[i].enemyStat.gameObject.SetActive(false);
                enemyList[i].enemyHealth.GetComponent<Slider>().value = enemyList[i].enemyUnit.getCurrentHP();
                enemyList[i].enemyUnit.gameObject.SetActive(true);
                enemyList[i].enemyHealth.gameObject.SetActive(true);
                enemyList[i].healthPanel.gameObject.SetActive(true);
                enemyList[i].enemyHud.gameObject.SetActive(true);
                enemyList[i].enemyStun.gameObject.SetActive(false);
                enemyList[i].enemyUnit.isStunned();
            }
            for (int i = 0; i < ManaCostText.Count; i++) {
                ManaCostText[i].gameObject.SetActive(false);
                ManaCostText[i].text = characterList.characters[i].playerUnit.getEnergy().ToString() + " / " + characterList.characters[i].playerUnit.getMaxEnergy().ToString();
            }
            GameManager2D.instance.UpdateBattleState(BattleState.PLAYERTURN);
            break;
        
        case BattleState.PLAYERTURN:
            for (int i = 0; i < ManaCostText.Count; i++) {
                ManaCostText[i].gameObject.SetActive(true);
            }
            displayText.gameObject.SetActive(false);
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
                    while (characterList.characters[PlayerCountTurn] == null || characterList.characters[PlayerCountTurn].playerUnit.getDead()) {
                        PlayerCountTurn++;
                        if (PlayerCountTurn >= currentPlayerCount)
                            GameManager2D.instance.UpdateBattleState(BattleState.ENEMYTURN);
                    }
                    OnButtonClicked(PlayerCountTurn);
                }
                break;
        
        case BattleState.ENEMYTURN:
                statusText.text = "Enemy's Turn";
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
                if (currentEnemyCount <= 0) {
                    Debug.Log("Enemy State WON 1");
                    GameManager2D.instance.UpdateBattleState(BattleState.WON);
                }
                
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
        if (enemyList[i].enemyUnit.isBurnt())
        {
            enemyList[i].enemyUnit.healthChange(-1 * enemyList[i].enemyUnit.burnDamage());
            enemyList[i].enemyHealth.GetComponent<Slider>().value = enemyList[i].enemyUnit.getCurrentHP();
            if (enemyList[i].enemyUnit.getCurrentHP() <= 0)
            {
                enemyList[i].enemyUnit.gameObject.SetActive(false);
                enemyList[i].enemyHealth.gameObject.SetActive(false);
                enemyList[i].healthPanel.gameObject.SetActive(false);
                enemyList[i].enemyHud.gameObject.SetActive(false);
                enemyList[i].enemyStat.gameObject.SetActive(false);
                enemyList[i].enemyStun.gameObject.SetActive(false);
                enemyList[i].enemyUnit.isStunned();
                currentEnemyCount--;
                if (currentEnemyCount <= 0) {
                    Debug.Log("Enemy State WON 2");
                    GameManager2D.instance.UpdateBattleState(BattleState.WON);
                    yield break;
                }
            }
            if (!enemyList[i].enemyUnit.getBurn())
                enemyList[i].enemyStat.gameObject.SetActive(false);
        }
        else {
            enemyList[i].enemyStat.gameObject.SetActive(false);
        }

        if (enemyList[i].enemyUnit.getDead() || enemyList[i].enemyUnit.isStunned()) {
            enemyList[i].enemyStun.gameObject.SetActive(false);
            continue;
        }
        bool heal = false;
        if (i == 2) { // healer logic
            for (int j = 1 ; j >=0 ; j--) {
                if (enemyList[j].enemyUnit.getDead() || enemyList[j].enemyUnit.getCurrentHP() > (int)(enemyList[j].enemyUnit.getMaxHP() * 0.5) )
                    continue;
                enemyList[j].enemyUnit.healthChange(enemyList[i].enemyUnit.unitAttack());
                enemyList[j].enemyHealth.GetComponent<Slider>().value = enemyList[j].enemyUnit.getCurrentHP();
                displayText.text = enemyList[i].enemyUnit.getName() + " healed " + enemyList[j].enemyUnit.getName() + " for " + enemyList[i].enemyUnit.unitAttack() + " HP!";
                heal = true;
                yield return new WaitForSeconds(1.5f);
                break;
            }
        }

        if (heal)
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
        displayText.gameObject.SetActive(true);
        displayText.text = enemyList[i].enemyUnit.getName() + " attacked " + characterList.characters[randomint].playerUnit.getName() + " for " + enemyList[i].enemyUnit.getDamagewithDefense(enemyList[i].enemyUnit.unitAttack()) + " damage!";
        characterList.characters[randomint].playerUnit.healthChange(-1 * enemyList[i].enemyUnit.getDamagewithDefense(enemyList[i].enemyUnit.unitAttack()));
        CameraShake.instance.TriggerShake(0.2f, 0.2f);
        characterList.characters[randomint].playerHealth.GetComponent<Slider>().value = characterList.characters[randomint].playerUnit.getCurrentHP();

        // Check if player died
        if (characterList.characters[randomint].playerUnit.getCurrentHP() <= 0)
        {
            characterList.characters[randomint].playerUnit.gameObject.SetActive(false);
            characterList.characters[randomint].playerHealth.gameObject.SetActive(false);
            characterList.characters[randomint].healthBarPanel.gameObject.SetActive(false);
            characterList.characters[randomint].playerHud.gameObject.SetActive(false);
            playerTotalCount--;
        }

        if (playerTotalCount <= 0)
        {
            GameManager2D.instance.UpdateBattleState(BattleState.LOST);
            yield break;
        }

        yield return new WaitForSeconds(1.5f); // Delay between attacks
    }

    // Check if players are still alive, then switch turn
    if (playerTotalCount > 0)
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

        if (invis > 0) {
            if (characterList.characters[1].playerUnit.isInvis() == 0) {
                Debug.Log("Invis Wore Off");
                invisText.gameObject.SetActive(false);
                invis = 0;
                int baseWeight = characterList.characters[1].playerUnit.getWeight() - characterList.characters[1].weight;
                characterList.characters[1].playerUnit.addWeight(-1 * baseWeight);
                Renderer render = characterList.characters[1].player.GetComponentInChildren<Renderer>();
                render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, 1f);
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
            GameObject enemySelect = Instantiate(enemySelectPrefab, healthBarEnemyPanels[i]);
            // DontDestroyOnLoad(enemySelect);
            enemySelect.GetComponent<Button>().onClick.AddListener(() => ApplyDamage(index));
            enemySelectButtons.Add(enemySelect);
            enemySelectButtons[i].GetComponent<ButtonEnemy>().hoverButton = enemyArrows[i];
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

            buttonGroup.buttonsForPlayer.Add(Instantiate(buttonPrefabsItem[i], buttonPanel));
            buttonGroup.buttonsForPlayer[2].GetComponent<Button>().onClick.AddListener(() => ItemButtonClicked(index));

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
        player1SkillButtonsSelect[0].GetComponent<buttonText>().display = displayText;
        player1SkillButtonsSelect[0].GetComponent<buttonText>().skillIndex = 0;
        player1SkillButtonsSelect[0].GetComponent<buttonText>().characterIndex = 0;
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
        player1SkillButtonsSelect[1].GetComponent<buttonText>().display = displayText;
        player1SkillButtonsSelect[1].GetComponent<buttonText>().skillIndex = 1;
        player1SkillButtonsSelect[1].GetComponent<buttonText>().characterIndex = 0;
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
        player1SkillButtonsSelect[2].GetComponent<buttonText>().display = displayText;
        player1SkillButtonsSelect[2].GetComponent<buttonText>().skillIndex = 2;
        player1SkillButtonsSelect[2].GetComponent<buttonText>().characterIndex = 0;
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
        player1SkillButtonsSelect[3].GetComponent<buttonText>().display = displayText;
        player1SkillButtonsSelect[3].GetComponent<buttonText>().skillIndex = 3;
        player1SkillButtonsSelect[3].GetComponent<buttonText>().characterIndex = 0;
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
        player2SkillButtonsSelect[0].GetComponent<buttonText>().display = displayText;
        player2SkillButtonsSelect[0].GetComponent<buttonText>().skillIndex = 0;
        player2SkillButtonsSelect[0].GetComponent<buttonText>().characterIndex = 1;
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
        player2SkillButtonsSelect[1].GetComponent<buttonText>().display = displayText;
        player2SkillButtonsSelect[1].GetComponent<buttonText>().skillIndex = 1;
        player2SkillButtonsSelect[1].GetComponent<buttonText>().characterIndex = 1;
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
        player2SkillButtonsSelect[2].GetComponent<buttonText>().display = displayText;
        player2SkillButtonsSelect[2].GetComponent<buttonText>().skillIndex = 2;
        player2SkillButtonsSelect[2].GetComponent<buttonText>().characterIndex = 1;
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
        player2SkillButtonsSelect[3].GetComponent<buttonText>().display = displayText;
        player2SkillButtonsSelect[3].GetComponent<buttonText>().skillIndex = 3;
        player2SkillButtonsSelect[3].GetComponent<buttonText>().characterIndex = 1;
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
        player3SkillButtonsSelect[0].GetComponent<buttonText>().display = displayText;
        player3SkillButtonsSelect[0].GetComponent<buttonText>().skillIndex = 0;
        player3SkillButtonsSelect[0].GetComponent<buttonText>().characterIndex = 2;
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
        player3SkillButtonsSelect[1].GetComponent<buttonText>().display = displayText;
        player3SkillButtonsSelect[1].GetComponent<buttonText>().skillIndex = 1;
        player3SkillButtonsSelect[1].GetComponent<buttonText>().characterIndex = 2; 
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
        player3SkillButtonsSelect[2].GetComponent<buttonText>().display = displayText;
        player3SkillButtonsSelect[2].GetComponent<buttonText>().skillIndex = 2;
        player3SkillButtonsSelect[2].GetComponent<buttonText>().characterIndex = 2;
        player3SkillButtonsSelect[2].SetActive(false);

        GameObject skillObject4 = new GameObject("ragnaROCKSkill");
        ragnaROCKSkill newSkill4 = skillObject4.AddComponent<ragnaROCKSkill>();
        Skill fourthSkill = GameManager2D.instance.skillListPlayer3.P3Skills[3];
        newSkill4.Setskill(fourthSkill.name, fourthSkill.description, fourthSkill.attack, fourthSkill.cost, fourthSkill.type, fourthSkill.healAmt);
        newSkill4.minigamebackground = GameObject.Find("ragnaROCKBackground");
        newSkill4.fist = GameObject.Find("RagnaFist");
        newSkill4.hitTarget = GameObject.Find("HitTarget");
        // newSkill4.missTarget = GameObject.Find("MissTarget");
        newSkill4.text = GameObject.Find("DirectionsRagna");
        newSkill4.setup();
        playerThreeSkills.Add(newSkill4);

        //set up the button and text
        player3SkillOptions[3].text = fourthSkill.name;
        player3SkillOptions[3].gameObject.SetActive(false);
        player3SkillButtonsSelect.Add(Instantiate(player3SkillButtons[3], buttonPanel));
        player3SkillButtonsSelect[3].GetComponent<Button>().onClick.AddListener(() => ragnaROCKButtonClicked(2));
        player3SkillButtonsSelect[3].GetComponent<buttonText>().display = displayText;
        player3SkillButtonsSelect[3].GetComponent<buttonText>().skillIndex = 3;
        player3SkillButtonsSelect[3].GetComponent<buttonText>().characterIndex = 2;
        player3SkillButtonsSelect[3].SetActive(false);
    }

    void OnButtonClicked(int index)
    {
        displayText.text = "What will " + characterList.characters[PlayerCountTurn].playerUnit.getName() + " do?";
        displayText.gameObject.SetActive(true);
        Debug.Log("Button: " + index);
        ToggleTextFirst(index);
        ToggleTextSecond(index);
        if (characterList.characters[index].playerUnit.isPassiveHeal() != 0) {
            characterList.characters[index].playerUnit.healthChange((int)Math.Ceiling(characterList.characters[index].playerUnit.getMaxHP() * 0.10f));
            characterList.characters[index].playerHealth.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getCurrentHP();
        }
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
        characterList.characters[index].playerHudItem.gameObject.SetActive(true);
        

        currentPlayerForMouse = index;
        lastSelected = buttonsForPlayer[index].buttonsForPlayer[0];
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttonsForPlayer[index].buttonsForPlayer[0]);
    }

    void AttackButtonClicked(int index)
    {   
        characterList.characters[index].playerHudAttack.gameObject.SetActive(false);
        characterList.characters[index].playerHudSkill.gameObject.SetActive(false);
        characterList.characters[index].playerHudItem.gameObject.SetActive(false);
        int buttonToStart = 0;
        Debug.Log("Attack button clicked for player: " + index);
        currentPlayerSelected = index;
        buttonsForPlayer[currentPlayerSelected].buttonsForPlayer[0].SetActive(false);
        buttonsForPlayer[currentPlayerSelected].buttonsForPlayer[1].SetActive(false);
        buttonsForPlayer[currentPlayerSelected].buttonsForPlayer[2].SetActive(false);
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
            if (level >= 10) {
                player1SkillOptions[0].gameObject.SetActive(true);
                player1SkillOptions[1].gameObject.SetActive(true);
                player1SkillOptions[2].gameObject.SetActive(true);
                player1SkillOptions[3].gameObject.SetActive(true);
            }
            else if (level >= 5) {
                player1SkillOptions[0].gameObject.SetActive(true);
                player1SkillOptions[1].gameObject.SetActive(true);
                player1SkillOptions[2].gameObject.SetActive(true);
            }
            else if (level >= 2) {
                player1SkillOptions[0].gameObject.SetActive(true);
                player1SkillOptions[1].gameObject.SetActive(true);
            }
            else {
                player1SkillOptions[0].gameObject.SetActive(true);
            }
            backButtonsText[0].gameObject.SetActive(true);
        }
        else if (index == 1) {
            if (level >= 10) {
                player2SkillOptions[0].gameObject.SetActive(true);
                player2SkillOptions[1].gameObject.SetActive(true);
                player2SkillOptions[2].gameObject.SetActive(true);
                player2SkillOptions[3].gameObject.SetActive(true);
            }
            else if (level >= 6) {
                player2SkillOptions[0].gameObject.SetActive(true);
                player2SkillOptions[1].gameObject.SetActive(true);
                player2SkillOptions[2].gameObject.SetActive(true);
            }
            else if (level >= 3) {
                player2SkillOptions[0].gameObject.SetActive(true);
                player2SkillOptions[1].gameObject.SetActive(true);
            }
            else {
                player2SkillOptions[0].gameObject.SetActive(true);
            }
            backButtonsText[1].gameObject.SetActive(true);
        }
        else {
            if (level >= 12) {
                player3SkillOptions[0].gameObject.SetActive(true);
                player3SkillOptions[1].gameObject.SetActive(true);
                player3SkillOptions[2].gameObject.SetActive(true);
                player3SkillOptions[3].gameObject.SetActive(true);
            }
            else if (level >= 10) {
                player3SkillOptions[0].gameObject.SetActive(true);
                player3SkillOptions[1].gameObject.SetActive(true);
                player3SkillOptions[2].gameObject.SetActive(true);
            }
            else if (level >= 5) {
                player3SkillOptions[0].gameObject.SetActive(true);
                player3SkillOptions[1].gameObject.SetActive(true);
            }
            else {
                player3SkillOptions[0].gameObject.SetActive(true);
            }
            backButtonsText[2].gameObject.SetActive(true);
        }
        characterList.characters[index].playerHudAttack.gameObject.SetActive(false);
        characterList.characters[index].playerHudSkill.gameObject.SetActive(false);
        characterList.characters[index].playerHudItem.gameObject.SetActive(false);
        for (int i = 0; i < 2; i++)
        {
            buttonsForPlayer[index].buttonsForPlayer[i].SetActive(false);
        }
        if (index == 0) {
            if (level >= 10) {
                player1SkillButtonsSelect[0].SetActive(true);
                player1SkillButtonsSelect[1].SetActive(true);
                player1SkillButtonsSelect[2].SetActive(true);
                player1SkillButtonsSelect[3].SetActive(true);
            }
            else if (level >= 5) {
                player1SkillButtonsSelect[0].SetActive(true);
                player1SkillButtonsSelect[1].SetActive(true);
                player1SkillButtonsSelect[2].SetActive(true);
            }
            else if (level >= 2) {
                player1SkillButtonsSelect[0].SetActive(true);
                player1SkillButtonsSelect[1].SetActive(true);
            }
            else {
                player1SkillButtonsSelect[0].SetActive(true);
            }
            backButtonSelect[0].gameObject.SetActive(true);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(player1SkillButtonsSelect[0]);
            lastSelected = player1SkillButtonsSelect[0];
        }
        else if (index == 1) {
            if (level >= 10) {
                player2SkillButtonsSelect[0].SetActive(true);
                player2SkillButtonsSelect[1].SetActive(true);
                player2SkillButtonsSelect[2].SetActive(true);
                player2SkillButtonsSelect[3].SetActive(true);
            }
            else if (level >= 6) {
                player2SkillButtonsSelect[0].SetActive(true);
                player2SkillButtonsSelect[1].SetActive(true);
                player2SkillButtonsSelect[2].SetActive(true);
            }
            else if (level >= 3) {
                player2SkillButtonsSelect[0].SetActive(true);
                player2SkillButtonsSelect[1].SetActive(true);
            }
            else {
                player2SkillButtonsSelect[0].SetActive(true);
            }
            backButtonSelect[1].gameObject.SetActive(true);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(player2SkillButtonsSelect[0]);
            lastSelected = player2SkillButtonsSelect[0];
        }
        else {
            if (level >= 12) {
                player3SkillButtonsSelect[0].SetActive(true);
                player3SkillButtonsSelect[1].SetActive(true);
                player3SkillButtonsSelect[2].SetActive(true);
                player3SkillButtonsSelect[3].SetActive(true);
            }
            else if (level >= 10) {
                player3SkillButtonsSelect[0].SetActive(true);
                player3SkillButtonsSelect[1].SetActive(true);
                player3SkillButtonsSelect[2].SetActive(true);
            }
            else if (level >= 5) {
                player3SkillButtonsSelect[0].SetActive(true);
                player3SkillButtonsSelect[1].SetActive(true);
            }
            else {
                player3SkillButtonsSelect[0].SetActive(true);
            }
            backButtonSelect[2].gameObject.SetActive(true);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(player3SkillButtonsSelect[0]);
            lastSelected = player3SkillButtonsSelect[0];
        }
    }

    public void ItemButtonClicked(int index) {
        Debug.Log("Item button clicked for player: " + index);
        // characterList.characters[index].playerHud.gameObject.SetActive(false);
        // characterList.characters[index].playerHudAttack.gameObject.SetActive(false);
        // characterList.characters[index].playerHudSkill.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++) {
            buttonsForPlayer[index].buttonsForPlayer[i].SetActive(false);
        }

        int healthPotions = inventoryManager2D.GetItemQuantity(0);
        int manaPotions = inventoryManager2D.GetItemQuantity(2);
        int singleHealthPotion = inventoryManager2D.GetItemQuantity(3);

        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
        characterList.characters[index].playerHudAttack.gameObject.SetActive(false);
        characterList.characters[index].playerHudSkill.gameObject.SetActive(false);
        characterList.characters[index].playerHudItem.gameObject.SetActive(false);

        // Debug.Log("Health Potions: " + healthPotions);
        itemListText[index].text = "Party HP: " + healthPotions.ToString();
        itemListText[index].gameObject.SetActive(true);
        healthPotionButtonsSelect[index].SetActive(true);

        itemManaListText[index].text = "Single MP: " + manaPotions.ToString();
        itemManaListText[index].gameObject.SetActive(true);
        manaPotionButtonsSelect[index].SetActive(true);

        itemSHealthListText[index].text = "Single HP: " + singleHealthPotion.ToString();
        itemSHealthListText[index].gameObject.SetActive(true);
        SingleHealthPotionButtonsSelect[index].SetActive(true);

        backButtonsText[index].gameObject.SetActive(true);
        backButtonSelect[index].gameObject.SetActive(true);


        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(healthPotionButtonsSelect[index]);
        lastSelected = healthPotionButtonsSelect[index];
    }

    public void commenceHealthPotion(int index) {
        healthPotionButtonsSelect[index].SetActive(false);
        itemListText[index].gameObject.SetActive(false);
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
        itemSHealthListText[index].gameObject.SetActive(false);
        SingleHealthPotionButtonsSelect[index].SetActive(false);
        manaPotionButtonsSelect[index].SetActive(false);
        itemManaListText[index].gameObject.SetActive(false);
        healthPotionButtonsSelect[index].SetActive(false);
        itemListText[index].gameObject.SetActive(false);
        Debug.Log("Health Potion used");
        if (inventoryManager2D.GetItemQuantity(0) > 0) {
            inventoryManager2D.UseItem(0);
            for (int i  = 0; i < characterList.characters.Count; i++) {
                characterList.characters[i].playerHealth.GetComponent<Slider>().value = characterList.characters[i].playerUnit.getCurrentHP();
            }
            characterList.characters[index].playerHud.gameObject.SetActive(true);
            checkplayerTurn();
        }
        else {
            Debug.Log("No health potions left!");
            characterList.characters[index].playerHud.gameObject.SetActive(true);
            BackButtonClicked(index);
            return;
        }
    }

    public void commenceManaPotion(int index) {
        manaPotionButtonsSelect[index].SetActive(false);
        itemManaListText[index].gameObject.SetActive(false);
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
        itemSHealthListText[index].gameObject.SetActive(false);
        manaPotionButtonsSelect[index].SetActive(false);
        itemManaListText[index].gameObject.SetActive(false);
        healthPotionButtonsSelect[index].SetActive(false);
        itemListText[index].gameObject.SetActive(false);
        itemSHealthListText[index].gameObject.SetActive(false);
        SingleHealthPotionButtonsSelect[index].SetActive(false);
        Debug.Log("Mana Potion used");
        if (inventoryManager2D.GetItemQuantity(2) > 0) {
            inventoryManager2D.SetIndex(index);
            inventoryManager2D.UseItem(2);
            characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
            characterList.characters[index].playerHud.gameObject.SetActive(true);
            checkplayerTurn();
        }
        else {
            Debug.Log("No mana potions left!");
            characterList.characters[index].playerHud.gameObject.SetActive(true);
            BackButtonClicked(index);
            return;
        }
    }

    public void commenceSingleHealthPotion(int index) {
        healthPotionButtonsSelect[index].SetActive(false);
        itemListText[index].gameObject.SetActive(false);
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
        manaPotionButtonsSelect[index].SetActive(false);
        itemManaListText[index].gameObject.SetActive(false);
        healthPotionButtonsSelect[index].SetActive(false);
        itemListText[index].gameObject.SetActive(false);
        itemSHealthListText[index].gameObject.SetActive(false);
        SingleHealthPotionButtonsSelect[index].SetActive(false);
        Debug.Log("Single Health Potion used");
        if (inventoryManager2D.GetItemQuantity(3) > 0) {
            inventoryManager2D.SetIndex(index);
            inventoryManager2D.UseItem(3);
            characterList.characters[index].playerHealth.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getCurrentHP();
            checkplayerTurn();
        }
        else {
            Debug.Log("No health potions left!");
            BackButtonClicked(index);
            return;
        }
    }

    void BackButtonClicked(int goBack) {
        Debug.Log("Back button clicked for player: " + goBack);
        // characterList.characters[index].playerHud.gameObject.SetActive(true);
        // characterList.characters[index].playerHudAttack.gameObject.SetActive(true);
        // characterList.characters[index].playerHudSkill.gameObject.SetActive(true);
        healthPotionButtonsSelect[goBack].SetActive(false);
        itemListText[goBack].gameObject.SetActive(false);
        healthPotionButtonsSelect[goBack].SetActive(false);
        itemManaListText[goBack].gameObject.SetActive(false);
        manaPotionButtonsSelect[goBack].SetActive(false);
        itemSHealthListText[goBack].gameObject.SetActive(false);
        SingleHealthPotionButtonsSelect[goBack].SetActive(false);
        for (int i = 0; i < 4; i++) {
            player1SkillButtonsSelect[i].SetActive(false);
            player2SkillButtonsSelect[i].SetActive(false);
            player3SkillButtonsSelect[i].SetActive(false);
            player1SkillOptions[i].gameObject.SetActive(false);
            player2SkillOptions[i].gameObject.SetActive(false);
            player3SkillOptions[i].gameObject.SetActive(false);

        }
        backButtonsText[goBack].gameObject.SetActive(false);
        backButtonSelect[goBack].gameObject.SetActive(false);
        OnButtonClicked(goBack);
    }

    void SlashButtonClicked(int index) {
        if (characterList.characters[index].playerUnit.getEnergy() < playerOneSkills[0].getSkillCost()) {
            Debug.Log("Not enough energy to use skill!");
            BackButtonClicked(index);
            return;
        }
        // characterList.characters[index].playerUnit.useEnergy(playerOneSkills[0].getSkillCost());
        // characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
        Debug.Log("Slash button clicked");
        for (int i = 0; i < player1SkillButtons.Count; i++) {
            player1SkillOptions[i].gameObject.SetActive(false);
            player1SkillButtonsSelect[i].SetActive(false);
        }
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
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
        if (characterList.characters[index].playerUnit.getEnergy() < playerOneSkills[1].getSkillCost()) {
            Debug.Log("Not enough energy to use skill!");
            BackButtonClicked(index);
            return;
        }
        // characterList.characters[index].playerUnit.useEnergy(playerOneSkills[1].getSkillCost());
        // characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
        Debug.Log("Fireball button clicked");
        for (int i = 0; i < player1SkillButtons.Count; i++) {
            player1SkillOptions[i].gameObject.SetActive(false);
            player1SkillButtonsSelect[i].SetActive(false);
        }
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
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
        if (characterList.characters[index].playerUnit.getEnergy() < playerOneSkills[2].getSkillCost()) {
            Debug.Log("Not enough energy to use skill!");
            BackButtonClicked(index);
            return;
        }
        // characterList.characters[index].playerUnit.useEnergy(playerOneSkills[2].getSkillCost());
        // characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
        Debug.Log("Pierce button clicked");

        for (int i = 0; i < player1SkillButtons.Count; i++) {
            player1SkillOptions[i].gameObject.SetActive(false);
            player1SkillButtonsSelect[i].SetActive(false);
        }
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
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
        AudioSystem2D.instance.playSwordSound();
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
                AudioSystem2D.instance.PlaySuccess();
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                enemyList[enemyIndex].enemyUnit.healthChange(-1 * playerOneSkills[0].skillInflict());
                enemyList[enemyIndex].enemyHealth.GetComponent<Slider>().value = enemyList[enemyIndex].enemyUnit.getCurrentHP();
                characterList.characters[index].playerUnit.useEnergy(playerOneSkills[0].getSkillCost());
                characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
                ManaCostText[index].text = characterList.characters[index].playerUnit.getEnergy().ToString() + " / " + characterList.characters[index].playerUnit.getMaxEnergy().ToString();
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
                AudioSystem2D.instance.PlayFailure();
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
        AudioSystem2D.instance.playSwordSound2();
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
                // enemyList[enemyIndex].enemyUnit.setBurnt(3);
                enemyList[enemyIndex].enemyStat.gameObject.SetActive(true);
                Debug.Log("Player succeeded in minigame!");
                AudioSystem2D.instance.PlaySuccess();
                enemyList[enemyIndex].enemyUnit.setBurnt(3);
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                enemyList[enemyIndex].enemyUnit.healthChange(-1 * playerOneSkills[2].skillInflict());
                enemyList[enemyIndex].enemyHealth.GetComponent<Slider>().value = enemyList[enemyIndex].enemyUnit.getCurrentHP();
                characterList.characters[index].playerUnit.useEnergy(playerOneSkills[2].getSkillCost());
                characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
                ManaCostText[index].text = characterList.characters[index].playerUnit.getEnergy().ToString() + " / " + characterList.characters[index].playerUnit.getMaxEnergy().ToString();
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
                AudioSystem2D.instance.PlayFailure();
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
                AudioSystem2D.instance.PlaySuccess();
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                enemyList[enemyIndex].enemyUnit.healthChange(-1 * playerOneSkills[1].skillInflict());
                enemyList[enemyIndex].enemyHealth.GetComponent<Slider>().value = enemyList[enemyIndex].enemyUnit.getCurrentHP();
                characterList.characters[index].playerUnit.useEnergy(playerOneSkills[1].getSkillCost());
                characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
                ManaCostText[index].text = characterList.characters[index].playerUnit.getEnergy().ToString() + " / " + characterList.characters[index].playerUnit.getMaxEnergy().ToString();
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
                AudioSystem2D.instance.PlayFailure();
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
        if (characterList.characters[index].playerUnit.getEnergy() < playerTwoSkills[0].getSkillCost()) {
            Debug.Log("Not enough energy to use skill!");
            BackButtonClicked(index);
            return;
        }
        characterList.characters[index].playerUnit.useEnergy(playerTwoSkills[0].getSkillCost());
        // Debug.Log("HEAL: " + playerOneSkills[0].getSkillCost());
        characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
        ManaCostText[index].text = characterList.characters[index].playerUnit.getEnergy().ToString() + " / " + characterList.characters[index].playerUnit.getMaxEnergy().ToString();
        healSwitch = 0;
        Debug.Log("Heal button clicked");
        for (int i = 0; i < player2SkillOptions.Count; i++) {
            player2SkillOptions[i].gameObject.SetActive(false);
            player2SkillButtonsSelect[i].SetActive(false);
        }
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
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
        if (characterList.characters[index].playerUnit.getEnergy() < playerTwoSkills[1].getSkillCost()) {
            Debug.Log("Not enough energy to use skill!");
            BackButtonClicked(index);
            return;
        }
        // characterList.characters[index].playerUnit.useEnergy(playerOneSkills[1].getSkillCost());
        // characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
        Debug.Log("Light Arrow Button Clicked!");

        for (int i = 0; i < player2SkillButtons.Count; i++) {
            player2SkillOptions[i].gameObject.SetActive(false);
            player2SkillButtonsSelect[i].SetActive(false);
        }
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
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
                AudioSystem2D.instance.PlaySuccess();
                characterList.characters[index].playerUnit.useEnergy(playerTwoSkills[1].getSkillCost());
                characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
                ManaCostText[index].text = characterList.characters[index].playerUnit.getEnergy().ToString() + " / " + characterList.characters[index].playerUnit.getMaxEnergy().ToString();
                if (invis == 0) {
                    Debug.Log("Player is invisible!");
                    characterList.characters[index].playerUnit.setInvis(3);
                    characterList.characters[2].playerUnit.addWeight(-20);
                    invis = 1;
                    invisText.gameObject.SetActive(true);
                    Renderer render = characterList.characters[index].player.GetComponentInChildren<Renderer>();
                    render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, 0.5f);
                }
                else {
                    characterList.characters[index].playerUnit.setInvis(3);
                }
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
                AudioSystem2D.instance.PlayFailure();
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
        if (characterList.characters[index].playerUnit.getEnergy() < playerTwoSkills[2].getSkillCost()) {
            Debug.Log("Not enough energy to use skill!");
            BackButtonClicked(index);
            return;
        }
        characterList.characters[index].playerUnit.useEnergy(playerTwoSkills[2].getSkillCost());
        characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
        ManaCostText[index].text = characterList.characters[index].playerUnit.getEnergy().ToString() + " / " + characterList.characters[index].playerUnit.getMaxEnergy().ToString();
        healSwitch = 1;
        Debug.Log("Heal button clicked");
        for (int i = 0; i < player2SkillOptions.Count; i++) {
            player2SkillOptions[i].gameObject.SetActive(false);
            player2SkillButtonsSelect[i].SetActive(false);
        }
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
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
        if (characterList.characters[index].playerUnit.getEnergy() < playerTwoSkills[3].getSkillCost()) {
            Debug.Log("Not enough energy to use skill!");
            BackButtonClicked(index);
            return;
        }
        characterList.characters[index].playerUnit.useEnergy(playerTwoSkills[3].getSkillCost());
        characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
        ManaCostText[index].text = characterList.characters[index].playerUnit.getEnergy().ToString() + " / " + characterList.characters[index].playerUnit.getMaxEnergy().ToString();
        Debug.Log("Heavens Delight button clicked");
        AudioSystem2D.instance.playHealingSound3();
        for (int i = 0; i < player2SkillOptions.Count; i++) {
            player2SkillOptions[i].gameObject.SetActive(false);
            player2SkillButtonsSelect[i].SetActive(false);
        }
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
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
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
        commenceRagnaROCKBUTTON(index, 0); // Do all enemies
    }

    void commenceRagnaROCKBUTTON(int index, int enemyIndex) {
        if (characterList.characters[index].playerUnit.getEnergy() < playerThreeSkills[3].getSkillCost()) {
            Debug.Log("Not enough energy to use skill!");
            BackButtonClicked(index);
            return;
        }
        playerThreeSkills[3].PlayMinigame((result) => {
            if (result == 1)    {
                Debug.Log("Player succeeded in minigame!");
                CameraShake.instance.TriggerShake(1.0f, 0.3f);
                AudioSystem2D.instance.playRaganROCKSound();
                AudioSystem2D.instance.PlaySuccess();
                characterList.characters[index].playerUnit.useEnergy(playerThreeSkills[3].getSkillCost());
                characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
                ManaCostText[index].text = characterList.characters[index].playerUnit.getEnergy().ToString() + " / " + characterList.characters[index].playerUnit.getMaxEnergy().ToString();
                // attack all enemies
                for (int i = 0; i < totalEnemyCount; i++) {
                    if (!enemyList[i].enemyUnit.getDead()) {
                        enemyList[i].enemyUnit.healthChange(-1 * playerThreeSkills[3].skillInflict());
                        enemyList[i].enemyHealth.GetComponent<Slider>().value = enemyList[i].enemyUnit.getCurrentHP();
                        enemyList[i].enemyUnit.setStunned();
                        enemyList[i].enemyStun.gameObject.SetActive(true);
                        if (enemyList[i].enemyUnit.getCurrentHP() <= 0) {
                            removeEnemy(i);
                            enemyList[i].enemyUnit.isStunned();
                            enemyList[i].enemyStun.gameObject.SetActive(false);
                        }
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
                AudioSystem2D.instance.PlayFailure();
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
        if (characterList.characters[index].playerUnit.getEnergy() < playerThreeSkills[1].getSkillCost()) {
            Debug.Log("Not enough energy to use skill!");
            BackButtonClicked(index);
            return;
        }
        characterList.characters[index].playerUnit.useEnergy(playerThreeSkills[1].getSkillCost());
        characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
        ManaCostText[index].text = characterList.characters[index].playerUnit.getEnergy().ToString() + " / " + characterList.characters[index].playerUnit.getMaxEnergy().ToString();
        Debug.Log("Rocky Wall button clicked");
        AudioSystem2D.instance.playRockWallSound();
        for (int i = 0; i < player3SkillButtons.Count; i++) {
            player3SkillOptions[i].gameObject.SetActive(false);
            player3SkillButtonsSelect[i].SetActive(false);
        }
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
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
        if (characterList.characters[index].playerUnit.getEnergy() < playerThreeSkills[2].getSkillCost()) {
            Debug.Log("Not enough energy to use skill!");
            BackButtonClicked(index);
            return;
        }
        // characterList.characters[index].playerUnit.useEnergy(playerOneSkills[2].getSkillCost());
        // characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
        Debug.Log("Rocky Taunt button clicked");
        for (int i = 0; i < player3SkillButtons.Count; i++) {
            player3SkillOptions[i].gameObject.SetActive(false);
            player3SkillButtonsSelect[i].SetActive(false);
        }
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
        // get the enemy index to attack
        for (int i = 0; i < enemySelectButtons.Count; i++) {
            int enemyindex = i;
            if (!enemyList[i].enemyUnit.getDead()) {
                enemySelectButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                enemySelectButtons[i].GetComponent<Button>().onClick.AddListener(() => commenceRumbleAndTumbleButton(index, enemyindex));
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

    void commenceRumbleAndTumbleButton(int index, int enemyIndex) {
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
        playerThreeSkills[1].PlayMinigame((result) => {
            if (result == 1)    {
                Debug.Log("Player succeeded in minigame!");
                characterList.characters[index].playerUnit.useEnergy(playerThreeSkills[2].getSkillCost());
                characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
                ManaCostText[index].text = characterList.characters[index].playerUnit.getEnergy().ToString() + " / " + characterList.characters[index].playerUnit.getMaxEnergy().ToString();
                AudioSystem2D.instance.playRockBreakSound();
                AudioSystem2D.instance.PlaySuccess();
                enemyList[enemyIndex].enemyUnit.setStunned();
                enemyList[enemyIndex].enemyStun.gameObject.SetActive(true);
                characterList.characters[index].playerHud.gameObject.SetActive(true);
                characterList.characters[index].playerHealth.SetActive(true);
                characterList.characters[index].healthBarPanel.gameObject.SetActive(true);
                enemyList[enemyIndex].enemyUnit.healthChange(-1 * playerThreeSkills[1].skillInflict());
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
                AudioSystem2D.instance.PlayFailure();
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
        if (characterList.characters[index].playerUnit.getEnergy() < playerThreeSkills[0].getSkillCost()) {
            Debug.Log("Not enough energy to use skill!");
            BackButtonClicked(index);
            return;
        }
        // characterList.characters[index].playerUnit.useEnergy(playerOneSkills[0].getSkillCost());
        // characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
        Debug.Log("Rocky Taunt button clicked");
        for (int i = 0; i < player3SkillButtons.Count; i++) {
            player3SkillOptions[i].gameObject.SetActive(false);
            player3SkillButtonsSelect[i].SetActive(false);
        }
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
        commenceRockyTauntButton(index, 0); // Do all enemies
    }

    void flameShowerButtonClicked(int index) {
        if (characterList.characters[index].playerUnit.getEnergy() < playerOneSkills[3].getSkillCost()) {
            Debug.Log("Not enough energy to use skill!");
            BackButtonClicked(index);
            return;
        }
        // characterList.characters[index].playerUnit.useEnergy(playerOneSkills[3].getSkillCost());
        // characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
        Debug.Log("Flame Shower Button Clicked");
        for (int i = 0; i < player1SkillButtons.Count; i++) {
            player1SkillOptions[i].gameObject.SetActive(false);
            player1SkillButtonsSelect[i].SetActive(false);
        }
        backButtonsText[index].gameObject.SetActive(false);
        backButtonSelect[index].gameObject.SetActive(false);
        commenceFlameShowerButtonClicked(index, 0); // Do all enemies
    }

    void commenceFlameShowerButtonClicked(int index, int enemyIndex) {
        AudioSystem2D.instance.playFlameSwordSound();
        // EVENT SYS CALL FOR FLAME SHOWER 
        playerOneSkills[3].PlayMinigame((result) => {
            if (result == 1)    {
                // attack all enemies
                characterList.characters[index].playerUnit.useEnergy(playerOneSkills[3].getSkillCost());
                characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
                ManaCostText[index].text = characterList.characters[index].playerUnit.getEnergy().ToString() + " / " + characterList.characters[index].playerUnit.getMaxEnergy().ToString();
                AudioSystem2D.instance.PlaySuccess();
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
                AudioSystem2D.instance.PlayFailure();
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
                characterList.characters[index].playerUnit.useEnergy(playerThreeSkills[0].getSkillCost());
                characterList.characters[index].playerMana.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getEnergy();
                ManaCostText[index].text = characterList.characters[index].playerUnit.getEnergy().ToString() + " / " + characterList.characters[index].playerUnit.getMaxEnergy().ToString();
                AudioSystem2D.instance.playSmallRumbleSound();
                CameraShake.instance.TriggerShake(1.0f, 0.5f);
                Debug.Log("Player succeeded in minigame!");
                AudioSystem2D.instance.PlaySuccess();
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
                AudioSystem2D.instance.PlayFailure();
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
            AudioSystem2D.instance.playHealingSound();
            characterList.characters[index].playerUnit.healthChange(playerTwoSkills[0].skillHeal());
            characterList.characters[index].playerHealth.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getCurrentHP();
        }
        else {
            AudioSystem2D.instance.playHealingSound2();
            characterList.characters[index].playerUnit.healthChange(playerTwoSkills[2].skillHeal());
            characterList.characters[index].playerHealth.GetComponent<Slider>().value = characterList.characters[index].playerUnit.getCurrentHP();
            characterList.characters[index].playerUnit.setPassiveHeal(3);
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
        CameraShake.instance.TriggerShake(0.2f, 0.2f);
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
        enemyArrows[index].gameObject.SetActive(false);
        enemyList[index].enemy.SetActive(false);
        enemySelectButtons[index].SetActive(false);
        enemyList[index].enemy.gameObject.SetActive(false);
        enemyList[index].enemyUnit.gameObject.SetActive(false);
        enemyList[index].enemyHealth.gameObject.SetActive(false);
        enemyList[index].healthPanel.gameObject.SetActive(false);
        enemyList[index].enemyHud.gameObject.SetActive(false);
        enemyList[index].enemyStun.gameObject.SetActive(false);
        enemyList[index].enemyStat.gameObject.SetActive(false);
        currentEnemyCount--;

        // check if all enemies are gone, if so, battle won
        if (currentEnemyCount <= 0)
            GameManager2D.instance.UpdateBattleState(BattleState.WON);
    }

    void checkplayerTurn() {
        for (int i = 0; i < enemyArrows.Count; i++) {
            enemyArrows[i].gameObject.SetActive(false);
        }
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
// ;-;