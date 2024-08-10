using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class SaveLoadManager : MonoBehaviour
{
    private string filePath;
    public GameObject Global;
    void Awake()
    {
        filePath = Application.persistentDataPath + "/data.dat";
        LoadPlayerData();
    }


    public void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            Global.GetComponent<Settings>().SetLevel(data.level);
            Global.GetComponent<Settings>().SetCoin(data.coins);
        }
        else
        {
            Debug.LogWarning("Save file not found in " + filePath);
        }
    }
}
