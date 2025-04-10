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
    private int burn;
    private int stun;

    public string getName() {
        return unitName;
    }

    public int getMaxHP() {
        return maxHP;
    }

    public int getCurrentHP() {
        return currentHP;
    }

    public bool isBurnt() {
        if (burn > 0) {
            burn--;
            return true;
        }
        return false;
    }

    public bool isStunned() {
        if (stun > 0) {
            stun--;
            return true;
        }
        return false;
    }

    public bool getBurn(){
        return burn > 0;
    }
    
    public bool getStun(){
        return stun > 0;
    }

    public int burnDamage() {
        int burnDamage = 0;
        burnDamage = (int)(maxHP * 0.1f);
        return burnDamage;
    }

    public void setBurnt(int burn) {
        this.burn = burn;
    }

    public void setStunned(int stun) {
        this.stun = stun;
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
        burn = 0;
        stun = 0;
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
        burn = 0;
        currentHP = maxHP;
    }

    public int getWeight() {
        return isWeight;
    }
    public void addWeight(int weight) {
        isWeight += weight;
    }

    public int getEnergy() {
        return currentEnergy;
    }
}
