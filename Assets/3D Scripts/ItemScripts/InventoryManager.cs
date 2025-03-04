using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager _instance;
    private static List<Item> inventory = new List<Item>();
    public static InventoryManager Instance { get { return _instance; } }

    public Item itemToAdd = null;

    void Update()
    {
        if (itemToAdd != null)
        {
            AddItem(itemToAdd);
            itemToAdd = null;
        }
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    //adds the item to the inventory
    public static void AddItem(Item item)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].itemId == item.itemId)
            {
                inventory[i].quantity += 1;
                return;
            }
        }

        inventory.Add(item);
    }

    //takes a starting inclusive starting index and end index and grabs all items in that range
    public static List<Item> GetInventoryRange(int start, int end)
    {
        List<Item> itemList = new List<Item>();
        int i;

        for (i = start; i <= end && i < inventory.Count; i++)
        {
            itemList.Add(inventory[i]);
        }

        //if there is not enough items to fill the range
        if (i != end + 1)
        {
            while(i <= end)
            {
                itemList.Add(null);
                i++;
            }
        }

        return itemList;
    }

    //takes a last index and gets the next item after that
    public static Item GetNextItem(int lastIndex)
    {
        if (lastIndex < inventory.Count - 1)
        {
            return inventory[lastIndex + 1];
        }

        return null;
    }

    //takes the first index and gets the previous item if it exists
    public static Item GetPrevious(int firstIndex)
    {
        if (firstIndex > 0 && inventory.Count != 0)
        {
            return inventory[firstIndex - 1];
        }

        return null;
    }

    //takes an index of an item to use and uses it
    public static void UseIndex(int index)
    {
        if (index >= 0 && index < inventory.Count)
        {
            inventory[index].Use();
            inventory[index].quantity -= 1;

            //checks if the player is now out of the item
            if (inventory[index].quantity == 0)
            {
                inventory.RemoveAt(index);
            }
        }
        else
        {
            Debug.Log("!!!INVALID INDEX SELECTED IN THE INVENTORY!!!");
        }
    }
}
