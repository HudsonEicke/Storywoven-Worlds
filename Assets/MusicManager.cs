using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicPlayer;
    public float timeToDecrease = 1f;
    [Space]
    [Header("Default Music")]
    public AudioClip defaultMusicClip;
    public float defaultMusicVolume;
    [Space]
    [Header("Castle Music")]
    public AudioClip castleMusicClip;
    public float castleMusicVolume;
    [Space]
    [Header("Boss Music")]
    public AudioClip bossMusicClip;
    public float bossMusicVolume;

    public bool swapMusic;

    private bool inSmoothTransition = false;
    private bool decreasing = false;
    private float defaultVolume;
    private float currentAudioVolume;
    private float amountToDecreasePerInverval;

    private bool increasing = false;
    private AudioClip newAudioClip;
    private float newAudioVolume;
    private float amountToIncreasePerInverval;

    private float timeRemaining = 0f;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer.clip = defaultMusicClip;
        musicPlayer.volume = defaultMusicVolume;
        musicPlayer.Play();
        currentAudioVolume = defaultMusicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (swapMusic)
        {
            swapMusic = false;
            SwapMusicSmooth(2);
        }

        if(inSmoothTransition)
        {
            if (decreasing)
            {
                timeRemaining -= Time.deltaTime;

                if (timeRemaining <= 0f)
                {
                    timeRemaining = timeToDecrease;
                    musicPlayer.clip = newAudioClip;
                    musicPlayer.Play();
                    decreasing = false;
                    increasing = true;
                }
                else
                {
                    musicPlayer.volume -= amountToDecreasePerInverval * Time.deltaTime;
                }
            }
            else if(increasing)
            {
                Debug.Log("INCREASING");
                timeRemaining -= Time.deltaTime;

                if (timeRemaining <= 0f)
                {
                    inSmoothTransition = false;
                    increasing = false;
                    musicPlayer.volume = newAudioVolume;
                    currentAudioVolume = newAudioVolume;
                }
                else
                {
                    musicPlayer.volume += amountToIncreasePerInverval * Time.deltaTime;
                }
            }
        }
    }

    void SwapMusicSmooth(int musicID)
    {
        amountToDecreasePerInverval = currentAudioVolume / timeToDecrease;
        decreasing = true;
        inSmoothTransition = true;
        timeRemaining = timeToDecrease;

        switch (musicID)
        {
            case 0:
                amountToIncreasePerInverval = defaultVolume / timeToDecrease;
                newAudioClip = defaultMusicClip;
                newAudioVolume = defaultMusicVolume;
                break;
            case 1:
                amountToIncreasePerInverval = castleMusicVolume / timeToDecrease;
                newAudioClip = castleMusicClip;
                newAudioVolume = castleMusicVolume;
                break;
            case 2:
                amountToIncreasePerInverval = bossMusicVolume / timeToDecrease;
                newAudioClip = bossMusicClip;
                newAudioVolume = bossMusicVolume;
                break;
        }
    }
}
