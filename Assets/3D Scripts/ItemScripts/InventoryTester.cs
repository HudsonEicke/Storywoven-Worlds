using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public int ID;
    public int quantity;
    public bool addItem;

    // Update is called once per frame
    void Update()
    {
        if (addItem)
        {
            ItemIdManager.Instance.AddItem(ID, quantity);
            addItem = false;
        }
    }
}
