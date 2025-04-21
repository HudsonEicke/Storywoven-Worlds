using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    [SerializeField]
    private int healthToAdd;

    public override void Use(int index)
    {
        if (!GameManager3D.Instance.inCombat)
        {
            ImportantComponentsManager.Instance.dialogueBox.DisplayText("Party health restored", 2.5f);
        }

        for (int i = 0; i < GameManager3D.Instance.characterList3D.characters.Count; i++)
        {
            // GameManager3D.Instance.characterList3D.characters[i].playerUnit.healthChange(healthToAdd);
            GameManager2D.instance.characterList.characters[i].playerUnit.healthChange(healthToAdd);
        }
    }
}
