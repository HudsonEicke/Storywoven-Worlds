using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class healSkill : skill
{
    // pretty basic skill, I dont want to make a minigame since I would like the player to get a guaranteed heal
    public override int skillHeal()
    {
        return base.skillHeal(); // dw bout this for now
    }
}
