using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // basic variables for a unit
    public string unitName;
    public int unitLevel;
    public int damage;
    public int maxHP;
    public int currentHP;
    public int currentEnergy;

    public bool healthChange(int change)
    {
        currentHP += change;

        if (currentHP <= 0)
        {   
            Debug.Log("[Unit] Unit had died");
            return true;
        }
        else
        {
            return false;
        }
    }

    public int unitAttack() {
        return damage;
        // will add more advanced calculations later
    }
}
