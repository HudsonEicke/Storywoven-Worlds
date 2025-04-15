using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager _instance;
    private static List<Item> inventory = new List<Item>();
    public static InventoryManager Instance { get { return _instance; } }
    public GameObject itemStorage;

    private void Awake()
    {
        _instance = this;
    }

    //adds the item to the inventory
    public void AddItem(Item item)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].itemId == item.itemId)
            {
                Debug.Log("Increasing quantity of " + item.itemName + " from " + inventory[i].quantity + " to " + (inventory[i].quantity + item.quantity));
                inventory[i].quantity += item.quantity;
                Destroy(item.gameObject);
                return;
            }
        }

        Debug.Log("Adding " + item.itemName + " with quantity " + item.quantity);
        inventory.Add(item);
    }

    //takes a starting inclusive starting index and end index and grabs all items in that range
    public List<Item> GetInventoryRange(int start, int end)
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
    public Item GetNextItem(int lastIndex)
    {
        if (lastIndex < inventory.Count - 1)
        {
            return inventory[lastIndex + 1];
        }

        return null;
    }

    //takes the first index and gets the previous item if it exists
    public Item GetPrevious(int firstIndex)
    {
        if (firstIndex > 0 && inventory.Count != 0)
        {
            return inventory[firstIndex - 1];
        }

        return null;
    }

    //takes an index of an item to use and uses it
    public void UseIndex(int index)
    {
        if (index >= 0 && index < inventory.Count)
        {
            inventory[index].Use();
            inventory[index].quantity -= 1;

            //checks if the player is now out of the item
            if (inventory[index].quantity == 0)
            {
                Item removedItem = inventory[index];
                inventory.RemoveAt(index);
                Destroy(removedItem.gameObject);
            }
        }
        else
        {
            if (inventory.Count == 0)
            {
                Debug.Log("The inventory is empty so can't use any items");
            }
            else
            {
                Debug.Log(index + " is not a valid index the inventory only has a range from 0 to " + (inventory.Count - 1) + " currently");
            }
        }
    }

    //gets the quantity of the given id
    public int GetItemQuantity(int id)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemId == id)
                return inventory[i].quantity;
        }

        return 0;
    }

    //returns how many items were removed
    public int RemoveItemAmount(int id, int amountToRemove)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemId == id)
            {
                if(amountToRemove > inventory[i].itemId)
                {
                    int quantity = inventory[i].quantity;
                    inventory.RemoveAt(i);
                    return quantity;
                }
                else
                {
                    inventory[i].quantity -= amountToRemove;

                    if(inventory[i].quantity <= 0)
                    {
                        inventory.RemoveAt(i);
                    }

                    return amountToRemove;
                }
            }
        }

        return 0;
    }

    public void EmptyInventory()
    {
        while (inventory.Count > 0)
        {
            RemoveItemAmount(inventory[0].itemId, inventory[0].quantity);
        }
    }

    public (int[] id, int[] quantity) GenerateInventorySaveFile()
    {
        int[] id = new int[inventory.Count];
        int[] quantity = new int[inventory.Count];

        for(int i = 0; i < inventory.Count; i++)
        {
            id[i] = inventory[i].itemId;
            quantity[i] = inventory[i].quantity;
        }

        return (id, quantity);
    }
}
