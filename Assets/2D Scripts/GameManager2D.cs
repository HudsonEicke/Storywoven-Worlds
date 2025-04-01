using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBattle; // We want to manage the state of our game using an ENUM
using System;
using UnityEngine.SceneManagement;
public class GameManager2D : MonoBehaviour
{
    // initializing variables
    public static GameManager2D instance;
    private AudioSystem2D audiosystem2D;
    public BattleState State;
    private CharacterSystem characterSystem;
    private EnemySystem enemySystem;
    private SkillSystemPlayer skillSystemPlayer;
    public static event Action<BattleState> OnBattleStateChanged;
    // private BattleSystem battleSystem;
    public CharacterList characterList;
    public SkillListPlayer1 skillListPlayer1;
    public SkillListPlayer2 skillListPlayer2;
    public List<EnemySystem.EnemyHealthAndInfo> enemyList;

    [SerializeField] public static int enemyCount;
    [SerializeField] public static int characterCount;
    [SerializeField] GameObject backGround;
    [SerializeField] GameObject allyBattleStations;
    [SerializeField] GameObject enemyBattleStations;

    // for 3D manager stuff
    public static event Action<int> OnGameEnd; // Event to notify game result

    public void InitializeGame(int enemies, int characters)
    {
        enemyCount = enemies;
        characterCount = characters;
        UpdateBattleState(BattleState.START);
    }

    public void test() {
        Debug.Log("TEST");
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // destroy if instance already exists
        }
        // audiosystem2D = gameObject.GetComponent<AudioSystem2D>();
        // get characters
        characterSystem = FindObjectOfType<CharacterSystem>();
        skillSystemPlayer = FindObjectOfType<SkillSystemPlayer>();
        // get enemies
        enemySystem = FindObjectOfType<EnemySystem>();
    }

    void Start()
    {
        Debug.Log("STARTINGS STUFF");
        UpdateBattleState(BattleState.PREPARE);
    }

    public void playAudio()
    {
        Debug.Log(audiosystem2D.ReturnAudio());
    }

    public void UpdateBattleState(BattleState newState) 
    {

        Debug.Log($"[GameManager2D] Updating state to: {newState}");
        Debug.Log($"[GameManager2D] Checking event subscriptions... (Subscribers: {OnBattleStateChanged?.GetInvocationList().Length ?? 0})");

        State = newState;

        switch(newState) 
        {
            case BattleState.SETUP:
                Debug.Log("[GameManager2D] Setting up everything");
                characterList = characterSystem.Load(2); // Load data from file
                skillListPlayer1 = skillSystemPlayer.Load();
                skillListPlayer2 = skillSystemPlayer.Load2(); // Load data from file
                enemyList = enemySystem.loadEnemies(3);
                backGround.SetActive(false);
                break;
            case BattleState.PREPARE:
                Debug.Log("[GameManager2D] Preparing Game");
                backGround.SetActive(false);
                break;
            case BattleState.START:
                backGround.SetActive(true);
                Debug.Log("[GameManager2D] Game Started");
                Debug.Log("COUNT: " + characterCount);
                break;
            case BattleState.PLAYERTURN:
                // System.Threading.Thread.Sleep(1000);
                Debug.Log("[GameManager2D] Player's Turn");
                break;
            case BattleState.ENEMYTURN:
                Debug.Log("[GameManager2D] Enemy's Turn");
                break;
            case BattleState.WON:
                Debug.Log("[GameManager2D] You Won!");
                backGround.SetActive(false);
                OnGameEnd?.Invoke(1); // Notify win
                break;
            case BattleState.LOST:
                Debug.Log("[GameManager2D] You Lost!");
                backGround.SetActive(false);
                OnGameEnd?.Invoke(0); // Notify loss
                // StartCoroutine(EndBattle());
                break;
            default:
                Debug.Log("[GameManager2D] Invalid Game State");
                break;
        }
        OnBattleStateChanged?.Invoke(newState);
    }

    private IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(2f); // Small delay before closing
        SceneManager.UnloadSceneAsync("GameManager2D"); // Close battle scene
    }
}