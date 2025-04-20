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
    private int maxEnergy;
    private int defense;
    private bool isDead;
    private int isWeight;
    private int burn;
    private bool stun;
    private int invis;
    private int passiveHeal;

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
        if (stun == true) {
            stun = false;
            return true;
        }
        return false;
    }

    public int isInvis() {
        if (invis > 0) {
            invis--;
            return invis;
        }
        return 0;
    }    

    public bool getBurn(){
        return burn > 0;
    }
    
    public bool getStun(){
        return stun;
    }

    public int burnDamage() {
        int burnDamage = 0;
        burnDamage = (int)(maxHP * 0.1f);
        return burnDamage;
    }

    public void setBurnt(int burn) {
        this.burn = burn;
    }

    public void setStunned() {
        stun = true;
    }

    public void setInvis(int invis) {
        this.invis = invis;
    }

    public void setPassiveHeal(int heal) {
        passiveHeal = heal;
    }

    public int isPassiveHeal() {
        if (passiveHeal > 0) {
            passiveHeal--;
            return 1;
        }
        return 0;
    }

    public void SetStats(int health, int dmg, int def, int current, string name, int lvl, int energy, int weight) {
        unitName = name;
        unitLevel = lvl;
        damage = dmg;
        maxHP = health;
        currentHP = current;
        currentEnergy = energy;
        maxEnergy = energy;
        defense = def;
        isDead = false;
        isWeight = weight;
        burn = 0;
        stun = false;
        invis = 0;
        passiveHeal = 0;
    }

    public int getDamagewithDefense(int damage) {
        return damage - (int)(damage * (defense / 100.0f)); // reduce damage by defense percentage
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
        stun = false;
        currentHP = maxHP;
        currentEnergy = maxEnergy;
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

    public void useEnergy(int energy) {
        currentEnergy -= energy;
    }

    public void restoreEnergy() {
        currentEnergy = maxEnergy;
    }
}
