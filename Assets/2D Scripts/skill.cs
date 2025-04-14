using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill : MonoBehaviour
{
    private int skillAttack;
    private int skillCost;
    private string skillName;
    private string skillDescription;
    private string skillType;
    private int skillHealAmt;

    public void Setskill(string name, string description, int attack, int cost, string type, int healAmt)
    {
        skillName = name;
        skillDescription = description;
        skillAttack = attack;
        skillCost = cost;
        skillType = type;
        skillHealAmt = healAmt;
    }

    public virtual int skillInflict() {
        return skillAttack;
        // will add more advanced calculations later
    }

    public virtual int skillHeal() {
        return skillHealAmt;
        // will add more advanced calculations later
    }

    public virtual int getSkillCost() {
        return skillCost;
        // will add more advanced calculations later
    }

    public virtual void PlayMinigame(Action<int> onComplete)
    {
        // Default implementation (if any)
        // Call onComplete with the result of the minigame (1 for success, 0 for failure)
        onComplete(1);
    }
}
