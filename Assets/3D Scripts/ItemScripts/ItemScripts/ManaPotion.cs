using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : Item
{
    // public int playerIndex;

    public override void Use(int index)
    {

        if (!GameManager3D.Instance.inCombat)
        {
            quantity += 1;
            ImportantComponentsManager.Instance.dialogueBox.DisplayText("Only usable in combat", 2.5f);
            return;
        }

        Debug.Log("Using Mana Potion");
        // GameManager3D.Instance.characterList3D.characters[i].playerUnit.healthChange(healthToAdd);
        GameManager2D.instance.characterList.characters[index].playerUnit.restoreEnergy();
    }
}
