using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSoundPlayer : MonoBehaviour
{
    public AudioSource fireSource;

    public void PlayFireSoundEffect()
    {
        fireSource.pitch = Random.Range(0.9f, 1.3f);
        fireSource.Play();
    }
}
