using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHover : MonoBehaviour
{
    public AudioClip hoverAudio;


    public void PlayHoverAudio()
    {
        Debug.Log("OnHover");
        AudioSysMenu.instance.HoverSound(hoverAudio);
    }
}
