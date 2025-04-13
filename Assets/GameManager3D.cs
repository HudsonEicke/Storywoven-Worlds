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
    private GameManager2D gameManager2D;

    public static event Action freezeWorld;
    public static event Action unFreezeWorld;

    // getting character information from character system
    private CharacterSystem3D characterSystem3D;
    public CharacterList3D characterList3D;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        Debug.Log("[GameManager3D] subscribing to OnGameEnd");
        GameManager2D.OnGameEnd += OnGameEnd;
        camera2D.SetActive(false);
        event2D.enabled = false;
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
    }

    private void OnEnable()
    {

    }

    public void StartBattle(int enemyCount)
    {
        Debug.Log("Start Battle");
        camera1.SetActive(false);
        event1.enabled = false;
        playerCamera.SetActive(false);

        // made the variables static so they can be modified from the 3D scene
        //GameManager2D.characterCount = 2; 
        //GameManager2D.enemyCount = enemyCount; 
        int players = 3;
        freezeWorld?.Invoke();
        ImportantComponentsManager.Instance.invetoryUIManager.CloseInventory();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        camera2D.SetActive(true);
        event2D.enabled = true;
        //gameManager2D.UpdateBattleState(BattleState.START);
        GameManager2D.instance.InitializeGame(enemyCount, players);
        // SceneManager.LoadScene("AngeloScene", LoadSceneMode.Additive);
    }

    private void OnGameEnd(int isEnd) 
    {
        StartCoroutine(handleGameEnd(isEnd));
        unFreezeWorld?.Invoke();
    }

    private IEnumerator handleGameEnd(int isEnd)
    {
        Debug.Log("[GameManager3D] Game Ended");
        if (isEnd == 1) 
        {
            Debug.Log("[GameManager3D] Game WON");
            // do whatever here for game win
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
        yield return new WaitForSeconds(0); // We need a very slight delay or else it will yell at us
        // camera1.SetActive(true); 
        event1.enabled = true;
        playerCamera.SetActive(true);
    }

    public void OnDeath()
    {
        SceneManager.LoadScene("DeathScene");
    }
}
