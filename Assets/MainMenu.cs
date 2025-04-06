using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;

    private void Start()
    {
        if(!File.Exists(SaveSystem.path))
        {
            continueButton.SetActive(false);
        }
    }

    public void Continue()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + data.sceneID);
    }

    public void NewGame()
    {
        SaveSystem.NewSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
