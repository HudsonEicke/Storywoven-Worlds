using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager3D : MonoBehaviour
{
    static GameManager3D _instance;
    public static GameManager3D Instance { get { return _instance; } }
    public bool startBattle = false;

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
        SceneManager.LoadScene("AngeloScene", LoadSceneMode.Additive);
        Time.timeScale = 0;
    }
}
