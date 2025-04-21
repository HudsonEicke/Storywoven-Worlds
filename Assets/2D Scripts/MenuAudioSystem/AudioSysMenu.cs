using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSysMenu : MonoBehaviour
{
    static AudioSysMenu _audioInstance;
    public static AudioSysMenu instance { get { return _audioInstance; } }
    public AudioSource audioSource;
    public AudioClip clip;
    public float loopStartTime = 5f;

    private bool hasLoopStarted = false;

    void Awake()
    {
        _audioInstance = this;
    }

    void Start()
    {
        audioSource.clip = clip;
        audioSource.loop = false; // Set to false to control looping manually
        audioSource.Play(); // Play from the beginning
    }

    void Update()
    {
        if (!hasLoopStarted && audioSource.time >= loopStartTime)
        {
            hasLoopStarted = true;
        }

        // This is better: check if audio.time has reached the clip length
        if (hasLoopStarted && audioSource.time >= audioSource.clip.length)
        {
            audioSource.Stop(); // Stop the audio
            Debug.Log("Looping audio from " + loopStartTime + " seconds.");
            audioSource.time = loopStartTime;
            audioSource.Play();
        }
    }

    public void HoverSound(AudioClip hoverAudio)
    {
        Debug.Log("AudioSysMenuHit");
        audioSource.PlayOneShot(hoverAudio);
    }


}
