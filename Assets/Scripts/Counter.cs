using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public GameObject Settings;
    int level;
    private string filePath;
    int coin;
    void Start()
    {
        filePath = Application.persistentDataPath + "/data.dat";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            if (CompareTag("Level"))
            {
                GetComponent<TMP_Text>().text = data.level.ToString();
                level = data.level;
            }
            if (CompareTag("Coin"))
            {
                GetComponent<TMP_Text>().text = data.coins.ToString();
                coin = data.coins;
            }
        }
        else
        {
            if (CompareTag("Level"))
            {
                GetComponent<TMP_Text>().text = 1.ToString();
            }
            Debug.LogWarning("Save file not found in " + filePath);
        }
    }

    public void RefreshCounter()
    {
        if (CompareTag("Level"))
        {
            level = Settings.GetComponent<Settings>().GetLevel();
            GetComponent<TMP_Text>().text = level.ToString();
        }
        if (CompareTag("Coin"))
        {
            GetComponent<TMP_Text>().text = Settings.GetComponent<Settings>().GetCoin().ToString();
            Settings.GetComponent<GlobalScript>().SavePlayerData(0);
        }
    }



}
