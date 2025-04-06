using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    static SaveManager _instance;
    public static SaveManager Instance { get { return _instance; } }

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

    private void Start()
    {
        LoadPlayer();
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(CheckpointManager.Instance, InventoryManager.Instance);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        CheckpointManager.Instance.currentPlayerCheckpoint = data.checkpointID;
        CheckpointManager.Instance.MovePlayerToCheckpoint();

        ItemIdManager.Instance.LoadInventory(data.inventoryIDs, data.itemQuantity);
    }
}
