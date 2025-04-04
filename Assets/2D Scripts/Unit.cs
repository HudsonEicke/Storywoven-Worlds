using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // basic variables for a unit
    private string unitName;
    private int unitLevel;
    private int damage;
    private int maxHP;
    private int currentHP;
    private int currentEnergy;
    private int defense;
    private bool isDead;
    private int isWeight;

    public string getName() {
        return unitName;
    }

    public int getMaxHP() {
        return maxHP;
    }

    public int getCurrentHP() {
        return currentHP;
    }

    public void SetStats(int health, int dmg, int def, int current, string name, int lvl, int energy, int weight) {
        unitName = name;
        unitLevel = lvl;
        damage = dmg;
        maxHP = health;
        currentHP = current;
        currentEnergy = energy;
        defense = def;
        isDead = false;
        isWeight = weight;
    }

    public bool healthChange(int change)
    {
        Debug.Log("HealthChange");
        currentHP += change;

        if (currentHP <= 0)
        {   
            currentHP = 0;
            Debug.Log("[Unit] Unit had died");
            isDead = true;
            return true;
        }
        else
        {
            if (currentHP >= maxHP)
            {
                currentHP = maxHP;
            }
            return false;
        }
    }

    public int unitAttack() {
        return damage;
        // will add more advanced calculations later
    }

    public bool getDead() {
        return isDead;
    }

    public void revive() {
        isDead = false;
        currentHP = maxHP;
    }

    public int getWeight() {
        return isWeight;
    }
    public void addWeight(int weight) {
        isWeight += weight;
    }
}
