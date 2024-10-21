using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataHandler
{
    public string directoryPath;
    public string fileName;

    public DataHandler(string directoryPath, string fileName)
    {
        this.directoryPath = directoryPath;
        this.fileName = fileName;
    }

    public void SaveData(GameData gameData)
    {
        string fullPath = Path.Combine(Application.persistentDataPath, directoryPath, fileName);
        Debug.Log($"Saving data to {fullPath}");
        try
        {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, directoryPath)))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, directoryPath));
            }

            string data = JsonUtility.ToJson(gameData);
            File.WriteAllText(fullPath, data);
            Debug.Log("Data saved successfully");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save data to {fullPath}. Exception: {e}");
        }
    }

    public GameData LoadData()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, Path.Combine(directoryPath, fileName));
        if (File.Exists(fullPath))
        {
            try
            {
                string data = File.ReadAllText(fullPath);
                GameData gameData = JsonUtility.FromJson<GameData>(data);
                Debug.Log($"Data Loaded successfully from {fullPath}");

                return gameData;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return null;
    }
    public void NewData()
    {
        if (File.Exists(Path.Combine(directoryPath, fileName)))
        {
            File.Delete(Path.Combine(directoryPath, fileName));
        }
    }
}
