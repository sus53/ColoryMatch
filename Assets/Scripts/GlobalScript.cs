using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class GlobalScript : MonoBehaviour
{
    private Color[] color = { new(1f, 0f, 0f), new(0f, 1f, 0f), new(0f, 0.67f, 0.91f), new(1f, 0.5f, 0f) };
    private List<Color> tempColor;
    private List<Color> charcColorArr = new();
    private List<Color> tyreColorArr = new();
    private readonly System.Random random = new();
    public GameObject CharacterGenerator;
    public GameObject TyreGenerator;
    private bool isTyreMovable = true;
    public GameObject levCompleteUI;
    public AudioSource levelCompleteAudio;
    public AudioSource BackgroundAudio;
    private List<Color> randColor = new();
    private int totalCharc = 0;
    private int totalTyre = 0;
    private string filePath;

    void Awake()
    {
        filePath = Application.persistentDataPath + "/data.dat";
        BackgroundAudio.Play();
        totalCharc = CharacterGenerator.GetComponent<CharacterGenerator>().GetCol() * CharacterGenerator.GetComponent<CharacterGenerator>().GetRow();
        totalTyre = TyreGenerator.GetComponent<TyreGenerator>().GetCol() * TyreGenerator.GetComponent<TyreGenerator>().GetRow();
    }
    public List<Color> GetRndColor()
    {
        return randColor;
    }
    public Color[] GenCharacterColor()
    {
        tempColor = color.ToList();

        for (int i = 0; i < totalCharc; i++)
        {
            int rn = random.Next(0, tempColor.Count);
            charcColorArr.Add(tempColor[rn]);
        }
        return charcColorArr.ToArray();
    }

    public Color[] GenTyreColor()
    {
        for (int i = 0; i < (totalTyre - totalCharc); i++)
        {
            tyreColorArr.Add(color[random.Next(0, 4)]);
            randColor.Add(color[random.Next(0, 4)]);
        }
        for (int i = 0; i < totalCharc; i++)
        {
            int rn = random.Next(0, charcColorArr.Count);
            tyreColorArr.Add(charcColorArr[rn]);
            charcColorArr.RemoveAt(rn);
        }
        return tyreColorArr.ToArray();
    }
    public IEnumerator MoveTyreToFront(GameObject tyre)
    {
        GameObject prevGO = tyre;
        Vector3 initialPosition = tyre.GetComponent<TyreController>().GetInitialPosition();
        Vector3 prevPosition;
        int count = 1;
        try
        {
            GameObject a = tyre.transform.parent.transform.Find("TyreClone" + (int.Parse(tyre.gameObject.name.Substring(9, 2)) + 10)).gameObject;

        }
        catch (System.Exception)
        {
            yield break;
        }
        GameObject nextTyre = tyre.transform.parent.transform.Find("TyreClone" + (int.Parse(tyre.gameObject.name.Substring(9, 2)) + 10)).gameObject;
        if (nextTyre.transform.childCount == 10)
        {
            float elapsedTime = 0f;
            float duration = 1f;
            float dissolveStrength;
            while (elapsedTime < duration)
            {
                dissolveStrength = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                Material mat = nextTyre.transform.GetChild(9).GetComponent<Renderer>().material;
                mat.SetFloat("_DissolveStrength", dissolveStrength);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Destroy(nextTyre.transform.GetChild(9).gameObject);
        }
        while (true)
        {
            string moveTurnName = "TyreClone" + (int.Parse(tyre.gameObject.name.Substring(9, 2)) + 10);

            if (tyre.transform.parent.transform.Find(moveTurnName) != null)
            {
                tyre = tyre.transform.parent.transform.Find(moveTurnName).gameObject;
                prevPosition = tyre.transform.position;
                yield return new WaitForSeconds(0.01f);
                float elapsedTime = 0f;
                float speed = 0.1f;
                while (elapsedTime < speed)
                {
                    tyre.transform.position = Vector3.Lerp(tyre.transform.position, initialPosition, elapsedTime / speed);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                tyre.transform.position = initialPosition;
                initialPosition = prevPosition;
                if (count == 1)
                {
                    tyre.GetComponent<TyreController>().ToggleOnTyreMovable2();
                }
                count++;
            }

            else break;
        }
        TyreGenerator.GetComponent<TyreGenerator>().SpawnTyre(tyre);
    }


    public bool GetTyreMovable()
    {
        return isTyreMovable;
    }

    public void SetTyreMovable(bool isTyreMovable)
    {
        this.isTyreMovable = isTyreMovable;
    }

    public void NextLevel()
    {
        BackgroundAudio.Stop();
        levelCompleteAudio.Play();
        levCompleteUI.SetActive(true);
        SavePlayerData(1);
    }

    public void SavePlayerData(int i)
    {
        string levName = SceneManager.GetActiveScene().name;
        GetComponent<Settings>().AddCoin(10);
        int coins = GetComponent<Settings>().GetCoin();
        int level = int.Parse(levName.Substring(5, 1)) + i;
        GetComponent<Settings>().SetLevel(level);

        SaveData data = new SaveData(level, coins);

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(filePath, json);

    }
    public void PlayBackgroundAudio()
    {

        BackgroundAudio.Play();
    }

}


[System.Serializable]
public class SaveData
{
    public int level;
    public int coins;

    public SaveData(int level, int coins)
    {
        this.level = level;
        this.coins = coins;
    }
}
