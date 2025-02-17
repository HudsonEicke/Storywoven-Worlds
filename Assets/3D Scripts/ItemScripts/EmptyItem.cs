using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyItem : Item
{
    //Empty class to represent empty slots in the inventory
    public override void Use()
    {
        return;
    }
}
