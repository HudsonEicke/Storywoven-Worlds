using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem2D : MonoBehaviour
{
    public static AudioSystem2D instance;
    public AudioClip buttonClick;
    public AudioSource buttonSelect;

    // battle music
    public AudioClip battleMusic;

    // battle sounds
    public AudioClip Success;
    public AudioClip Failure;
    private void Awake() {
        instance = this;
    }
    public string ReturnAudio() {
        return "Audio Playing";
    }

    public void playBattleMusic() {
        // play battle music
        buttonSelect.clip = battleMusic;
        buttonSelect.loop = true;
        buttonSelect.Play();
        
    }

    public void stopBattleMusic() {
        // stop battle music
        buttonSelect.Stop();
    }

    public void PlayAudio() {
        Debug.Log("Button Click Playing");
        buttonSelect.PlayOneShot(buttonClick);
    }

    public void PlaySuccess() {
        Debug.Log("Success Sound Playing");
        buttonSelect.PlayOneShot(Success);
    }
    public void PlayFailure() {
        Debug.Log("Failure Sound Playing");
        buttonSelect.PlayOneShot(Failure);
    }
}
