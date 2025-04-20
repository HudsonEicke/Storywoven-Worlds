using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public bool isSaveVersion = true;
    public bool idxBased = true;
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isSaveVersion)
            {
                SaveManager.Instance.SavePlayer(true);

                if (idxBased)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                else
                    SceneManager.LoadScene(sceneName);

            }
            else
            {
                if(sceneName == "Exit")
                    Application.Quit();

                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
