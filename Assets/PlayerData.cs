using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int sceneID;
    public int checkpointID;
    public int[] inventoryIDs;
    public int[] itemQuantity;

    //Normal save file generator
    public PlayerData(CheckpointManager checkpointManager, InventoryManager inventoryManager)
    {
        sceneID = checkpointManager.sceneID;
        checkpointID = checkpointManager.currentPlayerCheckpoint;
        (int[] id, int[] itemQuantity) tempInventoryData = inventoryManager.GenerateInventorySaveFile();
        inventoryIDs = tempInventoryData.id;
        itemQuantity = tempInventoryData.itemQuantity;
    }

    //New save case
    public PlayerData()
    {
        sceneID = 1;
        checkpointID = 0;
        inventoryIDs = new int[0];
        itemQuantity = new int[0];
    }

    //Next level save file generator
    public PlayerData(int sceneID, int checkpointID, InventoryManager inventoryManager)
    {
        this.sceneID = sceneID;
        this.checkpointID = checkpointID;
        (int[] id, int[] itemQuantity) tempInventoryData = inventoryManager.GenerateInventorySaveFile();
        inventoryIDs = tempInventoryData.id;
        itemQuantity = tempInventoryData.itemQuantity;
    }
}
