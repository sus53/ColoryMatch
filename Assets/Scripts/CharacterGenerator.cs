using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{

    public GameObject Character;
    public GameObject CharacterGround;
    [SerializeField] int row;
    [SerializeField] int col;
    [SerializeField] float genSpeed;
    private Dictionary<int, List<string>> characterData = new Dictionary<int, List<string>>();
    private List<string> characterDataArr = new List<string>();
    public float initial_x;
    public float initial_y;
    public float initial_z;
    public float diff_x;
    public float diff_z;
    public String AssetName;
    public Transform Characters;
    public GameObject Global;
    public Transform CharacterTyreGround;
    private Color[] color;
    void Start()
    {
        StartCoroutine(StartSpawning());
        color = Global.GetComponent<GlobalScript>().GenCharacterColor();
    }

    public int GetRow()
    {
        return row;
    }
    public int GetCol()
    {
        return col;
    }
    public Dictionary<int, List<string>> GetCharacterData()
    {
        return characterData;
    }
    IEnumerator StartSpawning()
    {
        AddChararacterDatas();
        float x = initial_x;
        float z = initial_z;
        int colorIndex = 0;
        for (int i = 0; i < col; i++)
        {

            for (int j = 0; j < row; j++)
            {
                yield return new WaitForSeconds(genSpeed);
                GameObject CharacterClone = Instantiate(Character, Characters);
                CharacterClone.name = AssetName + (i + 1) + (j + 1);
                StartCoroutine(ChangeScale(CharacterClone, new Vector3(0.055f, 0.055f, 0.055f), 0.5f));
                CharacterClone.transform.GetChild(0).GetComponent<Renderer>().material.color = color[colorIndex];
                CharacterClone.transform.GetChild(1).GetComponent<Renderer>().material.color = color[colorIndex];
                CharacterClone.transform.GetChild(2).GetComponent<Renderer>().material.color = color[colorIndex];
                CharacterClone.transform.GetChild(3).GetComponent<Renderer>().material.color = color[colorIndex];
                CharacterClone.transform.GetChild(4).GetComponent<Renderer>().material.color = color[colorIndex];
                CharacterClone.transform.position = new Vector3(x, initial_y, z);
                GameObject CharacterGroundClone = Instantiate(CharacterGround, CharacterTyreGround);
                CharacterGroundClone.name = AssetName + (i + 1) + (j + 1);
                CharacterGroundClone.transform.position = new Vector3(x, 0.01f, z);
                CharacterGroundClone.transform.localScale = new Vector3(0.18f, 0.18f, 0.001f);
                x += diff_x;
                colorIndex++;

            }
            x = initial_x;
            z += diff_z;
        }

    }

    IEnumerator ChangeScale(GameObject tyre, Vector3 targetScale, float duration)
    {
        Vector3 originalScale = tyre.transform.localScale;
        float currentTime = 0f;

        while (currentTime <= duration)
        {
            tyre.transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }

        tyre.transform.localScale = targetScale;
    }

    void AddChararacterDatas()
    {
        for (int i = 1; i <= row; i++)
        {
            for (int j = 1; j <= col; j++)
            {
                characterDataArr.Add(AssetName + (j) + (i));
            }
            characterData.Add(i, new List<string>(characterDataArr));
            characterDataArr.Clear();
        }
    }
}
