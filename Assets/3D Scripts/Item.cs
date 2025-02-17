using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Sprite itemIcon;
    public string itemName;
    public int quantity;

    public abstract void Use();
}
