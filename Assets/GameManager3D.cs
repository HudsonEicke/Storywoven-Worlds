using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;
using GameBattle;

public class GameManager3D : MonoBehaviour
{
    [SerializeField] GameObject camera1;
    [SerializeField] EventSystem event1;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject camera2D;
    [SerializeField] EventSystem event2D;
    static GameManager3D _instance;
    public static GameManager3D Instance { get { return _instance; } }
    public bool startBattle = false;
    public GameManager2D gameManager2D;

    public static event Action freezeWorld;
    public static event Action unFreezeWorld;

    // getting character information from character system
    private CharacterSystem3D characterSystem3D;
    public CharacterList3D characterList3D;
    public int playerMoney = 0;
    public int level = 2;
    public bool inCombat = false;
    int prevMoney = 0;
    public bool isWorldFrozen = false;

    public float transitionTime = 2f;
    private float countdownTime = 2f;
    private bool combatPrepare = false;
    private int nextFightEnemyCount;
    public Animator combatStartVisual;
    private GameObject objToDestory;
    public AudioSource swordSound;

    private void Awake()
    {
        _instance = this;
        Debug.Log("[GameManager3D] subscribing to OnGameEnd");
        GameManager2D.OnGameEnd += OnGameEnd;
        camera2D.SetActive(false);
        event2D.enabled = false;
        event2D.gameObject.SetActive(false);
        // for character system
        characterSystem3D = FindObjectOfType<CharacterSystem3D>();
    }

    private void Start()
    {
        gameManager2D = FindObjectOfType<GameManager2D>();
        gameManager2D.UpdateBattleState(BattleState.SETUP);
        characterList3D = characterSystem3D.Load(2);
        Debug.Log("HEALTH: " + characterList3D.characters[0].health);
        Debug.Log("HEALTH: " + characterList3D.characters[1].health);
    }

    // Update is called once per frame
    void Update()
    {
        if (startBattle)
        {
            startBattle = false;
            StartBattle(2);
        }

        if(combatPrepare)
        {
            countdownTime -= Time.deltaTime;

            if (countdownTime <= 0)
            {
                combatPrepare = false;
                StartBattle(nextFightEnemyCount);
            }
        }
    }

    private void OnDestroy()
    {
        GameManager2D.OnGameEnd -= OnGameEnd;
    }

    private void OnEnable()
    {

    }

    public void FreezeWorld()
    {
        isWorldFrozen = true;
        freezeWorld?.Invoke();
    }

    public void UnFreezeWorld()
    {
        isWorldFrozen = false;
        unFreezeWorld?.Invoke();
    }

    public void PrepareBattle(int enemyCount, GameObject objToDestory)
    {
        FreezeWorld();
        freezeWorld?.Invoke();
        ImportantComponentsManager.Instance.invetoryUIManager.CloseInventory();
        ImportantComponentsManager.Instance.thirdPersonMovement.playerHealthController.healthUI.UpdateHealth(0);
        combatPrepare = true;
        nextFightEnemyCount = enemyCount;
        countdownTime = transitionTime;
        combatStartVisual.Play("CombatStartAnimation");
        this.objToDestory = objToDestory;
        swordSound.Play();
    }

    public void StartBattle(int enemyCount)
    {
        Destroy(objToDestory);
        combatStartVisual.Play("IdleCombatStart");
        Debug.Log("Start Battle");
        camera1.SetActive(false);
        event1.enabled = false;
        event1.gameObject.SetActive(false);
        playerCamera.SetActive(false);
        inCombat = true;

        // made the variables static so they can be modified from the 3D scene
        //GameManager2D.characterCount = 2; 
        //GameManager2D.enemyCount = enemyCount; 
        int players = 3;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        camera2D.SetActive(true);
        event2D.enabled = true;
        event2D.gameObject.SetActive(true);
        //gameManager2D.UpdateBattleState(BattleState.START);
        if (level >= 10)
            enemyCount = 3;
        else if (level >= 5)
            enemyCount = 2;
        else
            enemyCount = 1;
        GameManager2D.instance.InitializeGame(enemyCount, players, level / 2);
        // SceneManager.LoadScene("AngeloScene", LoadSceneMode.Additive);
    }

    private void OnGameEnd(int isEnd) 
    {
        StartCoroutine(handleGameEnd(isEnd));
        UnFreezeWorld();
    }

    private IEnumerator handleGameEnd(int isEnd)
    {
        inCombat = false;
        Debug.Log("[GameManager3D] Game Ended");
        if (isEnd == 1) 
        {
            Debug.Log("[GameManager3D] Game WON");
            ImportantComponentsManager.Instance.thirdPersonMovement.playerHealthController.UpdateUI();
            // do whatever here for game win
            level++;

            ItemIdManager.Instance.AddItem(1, 1);
            AddMoney(15);
            if (level % 2 == 0)
                ImportantComponentsManager.Instance.dialogueBox.DisplayText("You have leveled up! You have obtained 2 Overworld Health Potions and " + 30 + " coins!", 5f);
            else
                ImportantComponentsManager.Instance.dialogueBox.DisplayText("You have obtained 2 Overworld Health Potions and " + 30 + " coins!", 5f);
        }
        else 
        {
            Debug.Log("[GameManager3D] Game LOST");
            OnDeath();
            // do whatever here for game lost
        }
        // SceneManager.UnloadSceneAsync("AngeloScene");
        camera2D.SetActive(false);
        event2D.enabled = false;
        event2D.gameObject.SetActive(false);
        yield return new WaitForSeconds(0); // We need a very slight delay or else it will yell at us
        // camera1.SetActive(true); 
        event1.enabled = true;
        event1.gameObject.SetActive(true);
        playerCamera.SetActive(true);
    }

    public void OnDeath()
    {
        SceneManager.LoadScene("DeathScene");
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        Debug.Log($"[GameManager3D] Money Added: {amount} | Total: {playerMoney}");
    }

    public bool SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            Debug.Log($"[GameManager3D] Money Spent: {amount} | Remaining: {playerMoney}");
            return true;
        }

        Debug.LogWarning("[GameManager3D] Not enough money!");
        return false;
    }

    public int GetMoney()
    {
        return playerMoney;
    }
}
