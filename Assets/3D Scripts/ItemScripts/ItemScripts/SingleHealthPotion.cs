using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHealthPotion : Item
{
    // public int playerIndex;
    [SerializeField]
    private int healthToAdd;

    public override void Use(int index)
    {
        if(!GameManager3D.Instance.inCombat)
        {
            quantity += 1;
            ImportantComponentsManager.Instance.dialogueBox.DisplayText("Only usable in combat", 2.5f);
            return;
        }

        Debug.Log("Using Single Health Potion");
        GameManager2D.instance.characterList.characters[index].playerUnit.healthChange(healthToAdd);
    }
}
