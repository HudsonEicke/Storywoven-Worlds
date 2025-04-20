using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string path = Application.persistentDataPath + "/player.save";

    public static void SavePlayer(CheckpointManager checkpointManager, InventoryManager inventoryManager, GameManager3D gameManager3D)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(checkpointManager, inventoryManager, gameManager3D.playerMoney, gameManager3D.level);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SavePlayer(CheckpointManager checkpointManager, InventoryManager inventoryManager, PowerupManager powerupManager, GameManager3D gameManager3D)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(checkpointManager, inventoryManager, powerupManager, gameManager3D.playerMoney, gameManager3D.level);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SavePlayer(bool NextLevel, CheckpointManager checkpointManager, InventoryManager inventoryManager, GameManager3D gameManager3D)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(checkpointManager.sceneID + 1, 0, inventoryManager, gameManager3D.playerMoney, gameManager3D.level);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void NewSave()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData();
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        PlayerData data;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
        }
        else
        {
            data = new PlayerData();
        }

        return data;
    }
}
