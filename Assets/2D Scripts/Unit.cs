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
    public int defense;

    public void SetStats(int health, int dmg, int def, int current, string name, int lvl, int energy) {
        unitName = name;
        unitLevel = lvl;
        damage = dmg;
        maxHP = health;
        currentHP = current;
        currentEnergy = energy;
        defense = def;
    }

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
