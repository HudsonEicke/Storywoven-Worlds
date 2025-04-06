using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    static string path = Application.persistentDataPath + "/player.save";

    public static void SavePlayer(CheckpointManager checkpointManager, InventoryManager inventoryManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(checkpointManager, inventoryManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SavePlayer(bool NextLevel, CheckpointManager checkpointManager, InventoryManager inventoryManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(checkpointManager.sceneID, 0, inventoryManager);
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
