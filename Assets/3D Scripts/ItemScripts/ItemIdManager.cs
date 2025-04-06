using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIdManager : MonoBehaviour
{
    static ItemIdManager _instance;
    public List<GameObject> idToItem = new List<GameObject>();
    public static ItemIdManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public bool AddItem(int id, int quantity)
    {
        if(quantity <= 0)
        {
            Debug.Log(quantity + " is not a valid quantity for an item");
            return false;
        }

        if(id < 0 ||  id >= idToItem.Count)
        {
            Debug.Log("No item has the ID " + id);
            return false;
        }

        GameObject newItem = Instantiate(idToItem[id], new Vector3(0, 0, 0), Quaternion.identity);
        newItem.transform.parent = InventoryManager.Instance.itemStorage.transform;
        newItem.GetComponent<Item>().quantity = quantity;
        InventoryManager.Instance.AddItem(newItem.GetComponent<Item>());

        return true;
    }

    public void LoadInventory(int[] id, int[] quantity)
    {
        InventoryManager.Instance.EmptyInventory();

        //No items to load case
        if (id == null || quantity == null)
            return;

        for (int i = 0; i < id.Length; i++)
        {
            AddItem(id[i], quantity[i]);
        }
    }
}
