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
    public bool hasDoubleJump;
    public bool hasSprint;
    public bool hasBoostedHealth;
    public int playerMoney;

    //Normal save file generator
    public PlayerData(CheckpointManager checkpointManager, InventoryManager inventoryManager, int money)
    {
        sceneID = checkpointManager.sceneID;
        checkpointID = checkpointManager.currentPlayerCheckpoint;
        (int[] id, int[] itemQuantity) tempInventoryData = inventoryManager.GenerateInventorySaveFile();
        inventoryIDs = tempInventoryData.id;
        itemQuantity = tempInventoryData.itemQuantity;
        hasDoubleJump = false;
        hasSprint = false;
        hasBoostedHealth = false;
        playerMoney = money;
    }

    //Fantasy save file generator
    public PlayerData(CheckpointManager checkpointManager, InventoryManager inventoryManager, PowerupManager powerupManager, int money)
    {
        sceneID = checkpointManager.sceneID;
        checkpointID = checkpointManager.currentPlayerCheckpoint;
        (int[] id, int[] itemQuantity) tempInventoryData = inventoryManager.GenerateInventorySaveFile();
        inventoryIDs = tempInventoryData.id;
        itemQuantity = tempInventoryData.itemQuantity;
        hasDoubleJump = powerupManager.hasDoubleJump;
        hasSprint = powerupManager.hasSprint;
        hasBoostedHealth = powerupManager.hasBoostedHealth;
        playerMoney = money;
    }

    //New save case
    public PlayerData()
    {
        sceneID = 1;
        checkpointID = 0;
        inventoryIDs = new int[0];
        itemQuantity = new int[0];
        hasDoubleJump = false;
        hasSprint = false;
        hasBoostedHealth = false;
        playerMoney = 0;
    }

    //Next level save file generator
    public PlayerData(int sceneID, int checkpointID, InventoryManager inventoryManager, int money)
    {
        this.sceneID = sceneID;
        this.checkpointID = checkpointID;
        (int[] id, int[] itemQuantity) tempInventoryData = inventoryManager.GenerateInventorySaveFile();
        inventoryIDs = tempInventoryData.id;
        itemQuantity = tempInventoryData.itemQuantity;
        hasDoubleJump = false;
        hasSprint = false;
        hasBoostedHealth = false;
        playerMoney = money;
    }
}
