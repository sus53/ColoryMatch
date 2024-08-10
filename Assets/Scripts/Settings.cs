using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private bool isMusic = true;
    private bool isSound = true;
    private int level = 1;
    public GameObject Audios;
    private int coin = 100;
    string filePath;
    public GameObject noSound;
    public GameObject settingUI;
    private bool setting = false;
    void Awake()
    {
        filePath = Application.persistentDataPath + "/data.dat";
    }
    void Start()
    {
        LoadPlayerData();
    }
    public void ToggleSetting()
    {
        setting = !setting;
        if (setting)
        {
            settingUI.SetActive(true);
        }
        else
        {
            settingUI.SetActive(false);
        }
    }
    public void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            level = data.level;
            coin = data.coins;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + filePath);
        }
    }
    public int GetCoin()
    {
        return coin;
    }
    public void AddCoin(int coin)
    {
        this.coin += coin;
    }
    public void SetCoin(int coin)
    {
        this.coin = coin;
    }

    public void DeductCoin()
    {
        coin -= 100;
    }
    public int GetLevel()
    {
        return level;
    }
    public void SetLevel(int level)
    {
        this.level = level;
    }
    public void ChangeMusic()
    {
        isMusic = !isMusic;
        if (isMusic)
        {
            Audios.transform.GetChild(0).GetComponent<AudioSource>().Play();
            noSound.SetActive(false);
        }
        else
        {
            Audios.transform.GetChild(0).GetComponent<AudioSource>().Pause();
            noSound.SetActive(true);
        }
    }

    // public void ChangeSound()
    // {
    //     isSound = !isSound;
    //     if (isSound)
    //     {
    //         Audios.transform.GetChild(0).GetComponent<AudioSource>().Play();
    //     }
    //     else
    //     {
    //         Audios.transform.GetChild(0).GetComponent<AudioSource>().Pause();
    //     }
    // }

    public void ChangeLevel(int lev)
    {
        level = lev;
    }



}
