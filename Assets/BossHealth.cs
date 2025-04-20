using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int health = 3;
    public Animator animator;
    public LockInPlace lockInPlace;
    public AudioSource hurtAudio;

    public void Hit()
    {
        health--;
        hurtAudio.Play();
        if (health <= 0)
        {
            animator.StopPlayback();
            lockInPlace.locked = false;
            animator.Play("Fly Death 1", 0, 0);
        }
        else
        {
            animator.StopPlayback();
            animator.Play("Fly Got Hit", 0, 0);
        }
    }

    private void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Destroy(gameObject);
        }    
    }
}
