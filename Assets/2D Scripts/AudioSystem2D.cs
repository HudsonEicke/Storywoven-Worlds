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
    public AudioClip swordSound;
    public AudioClip swordSound2;
    public AudioClip fireBall;
    public AudioClip flameSword;

    // healing sounds
    public AudioClip healingSound;
    public AudioClip healingSound2;
    public AudioClip healingSound3;

    // rock sounds
    public AudioClip rockBreak;
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

    public void playSwordSound() {
        Debug.Log("Sword Sound Playing");
        buttonSelect.PlayOneShot(swordSound);

    }
    public void playSwordSound2() {
        Debug.Log("Sword Sound 2 Playing");
        buttonSelect.PlayOneShot(swordSound2);
    }

    public void playFireballSound() {
        Debug.Log("Fireball Sound Playing");
        buttonSelect.PlayOneShot(fireBall);
    }

    public void playFlameSwordSound() {
        Debug.Log("Flame Sword Sound Playing");
        buttonSelect.PlayOneShot(flameSword);
    }

    public void playHealingSound() {
        Debug.Log("Healing Sound Playing");
        buttonSelect.PlayOneShot(healingSound);
    }

    public void playHealingSound2() {
        Debug.Log("Healing Sound 2 Playing");
        buttonSelect.PlayOneShot(healingSound2);
    }

    public void playHealingSound3() {
        Debug.Log("Healing Sound 3 Playing");
        buttonSelect.PlayOneShot(healingSound3);
    }

    
}
