using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldHealthPotion : Item
{
    [SerializeField]
    private int healthToAdd;

    public override void Use()
    {
        if (ImportantComponentsManager.Instance.thirdPersonMovement.playerHealthController.currentHealth == ImportantComponentsManager.Instance.thirdPersonMovement.playerHealthController.maxHealth)
        {
            quantity += 1;
        }
        else
        {
            ImportantComponentsManager.Instance.thirdPersonMovement.playerHealthController.Heal(healthToAdd);
        }
    }
}
