using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject container;
    static PauseMenu _instance;
    public static PauseMenu Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else if(!GameManager3D.Instance.isWorldFrozen)
            {
                Pause();
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        GameManager3D.Instance.UnFreezeWorld();
        isPaused = false;
        container.SetActive(false);
    }

    public void Pause()
    {
        GameManager3D.Instance.FreezeWorld();
        isPaused = true;
        container.SetActive(true);
    }
}
