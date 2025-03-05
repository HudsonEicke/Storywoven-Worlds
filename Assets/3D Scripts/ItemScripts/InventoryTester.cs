using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public int ID;
    public int quantity;
    public bool addItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
