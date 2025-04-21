using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject container;
    static PauseMenu _instance;
    public static PauseMenu Instance { get { return _instance; } }
    public Slider slider;
    public AudioMixer audioMixer;
    private float value;


    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        audioMixer.GetFloat("Volume", out value);
        slider.value = value;
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

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
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
