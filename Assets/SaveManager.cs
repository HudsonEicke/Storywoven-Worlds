using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    static SaveManager _instance;
    public static SaveManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        LoadPlayer();
    }

    public void SavePlayer()
    {
        if(CheckpointManager.Instance.sceneID == 1)
            SaveSystem.SavePlayer(CheckpointManager.Instance, InventoryManager.Instance);
        else
            SaveSystem.SavePlayer(CheckpointManager.Instance, InventoryManager.Instance, ImportantComponentsManager.Instance.powerupManager);
    }

    public void SavePlayer(bool nextLevel)
    {
        SaveSystem.SavePlayer(true, CheckpointManager.Instance, InventoryManager.Instance);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        CheckpointManager.Instance.currentPlayerCheckpoint = data.checkpointID;
        CheckpointManager.Instance.MovePlayerToCheckpoint();

        ItemIdManager.Instance.LoadInventory(data.inventoryIDs, data.itemQuantity);

        if (data.sceneID == 2)
            ImportantComponentsManager.Instance.powerupManager.LoadPowerups(data.hasDoubleJump, data.hasSprint, data.hasBoostedHealth);
    }
}
