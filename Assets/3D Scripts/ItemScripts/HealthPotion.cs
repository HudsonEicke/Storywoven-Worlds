using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    [SerializeField]
    private int healthToAdd;

    public override void Use()
    {
        //ADD CODE HERE TO ADD HEALTH

        this.quantity -= 1;

        Debug.Log("USED");
    }
}
