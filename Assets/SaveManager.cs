using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    static SaveManager _instance;
    public static SaveManager Instance { get { return _instance; } }
    public bool devTest = false;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        if (devTest)
        {
            SaveSystem.DevSave();
        }

        LoadPlayer();
    }

    public void SavePlayer()
    {
        if(CheckpointManager.Instance.sceneID == 1)
            SaveSystem.SavePlayer(CheckpointManager.Instance, InventoryManager.Instance, GameManager3D.Instance);
        else
            SaveSystem.SavePlayer(CheckpointManager.Instance, InventoryManager.Instance, ImportantComponentsManager.Instance.powerupManager, GameManager3D.Instance, ProgressManager.Instance);
    }

    public void SavePlayer(bool nextLevel)
    {
        SaveSystem.SavePlayer(true, CheckpointManager.Instance, InventoryManager.Instance, GameManager3D.Instance);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        CheckpointManager.Instance.currentPlayerCheckpoint = data.checkpointID;
        CheckpointManager.Instance.MovePlayerToCheckpoint();

        ItemIdManager.Instance.LoadInventory(data.inventoryIDs, data.itemQuantity);

        GameManager3D.Instance.playerMoney = data.playerMoney;

        GameManager3D.Instance.level = data.playerLevel;

        if (data.sceneID == 2)
        {
            ImportantComponentsManager.Instance.powerupManager.LoadPowerups(data.hasDoubleJump, data.hasSprint, data.hasBoostedHealth);
            ProgressManager.Instance.SetStage(data.currentStage, true);
        }
    }
}
