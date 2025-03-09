using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    public ThirdPersonMovement player;
    public DeathChecker checker;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            checker.PlayerFell();
            player.moveBack = true;
        }
    }
}
