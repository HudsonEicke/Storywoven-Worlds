using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager3D : MonoBehaviour
{
    static GameManager3D _instance;
    private static List<Item> inventory = new List<Item>();
    public static GameManager3D Instance { get { return _instance; } }
    public GameObject itemStorage;

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
