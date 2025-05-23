using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public void Continue()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        SceneManager.LoadScene(data.sceneID);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
