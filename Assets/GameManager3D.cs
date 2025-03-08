using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager3D : MonoBehaviour
{
    [SerializeField] GameObject camera1;
    static GameManager3D _instance;
    public static GameManager3D Instance { get { return _instance; } }
    public bool startBattle = false;
    private GameManager2D gameManager2D;

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
    }

    private void Start()
    {
        gameManager2D = FindObjectOfType<GameManager2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startBattle)
        {
            startBattle = false;
            StartBattle();
        }
    }

    private void OnEnable()
    {

    }

    public void StartBattle()
    {
        Debug.Log("Start Battle");
        camera1.SetActive(false);

        // made the variables static so they can be modified from the 3D scene
        GameManager2D.characterCount = 2; 
        GameManager2D.enemyCount = 1; 

        SceneManager.LoadScene("AngeloScene", LoadSceneMode.Additive);
    }

    private void OnGameEnd(int isEnd) {
        StartCoroutine(handleGameEnd(isEnd));
    }

    private IEnumerator handleGameEnd(int isEnd)
{
    Debug.Log("[GameManager3D] Game Ended");
    if (isEnd == 1) {
        Debug.Log("[GameManager3D] Game WON");
        // do whatever here for game win
    }
    else {
        Debug.Log("[GameManager3D] Game LOST");
        // do whatever here for game lost
    }
    SceneManager.UnloadSceneAsync("AngeloScene");

    yield return new WaitForSeconds(0); // We need a very slight delay or else it will yell at us
    camera1.SetActive(true); 
}
}
