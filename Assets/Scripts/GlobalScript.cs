using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private int totalCharc = 0;
    private int totalTyre = 0;
    void Awake()
    {
        BackgroundAudio.Play();
        totalCharc = CharacterGenerator.GetComponent<CharacterGenerator>().GetCol() * CharacterGenerator.GetComponent<CharacterGenerator>().GetRow();
        totalTyre = TyreGenerator.GetComponent<TyreGenerator>().GetCol() * TyreGenerator.GetComponent<TyreGenerator>().GetRow();
    }
    public Color[] GenCharacterColor()
    {
        tempColor = color.ToList();

        for (int i = 0; i < totalCharc; i++)
        {
            int rn = random.Next(0, tempColor.Count);
            charcColorArr.Add(tempColor[rn]);
            // tempColor.RemoveAt(rn);
        }
        return charcColorArr.ToArray();
    }

    public Color[] GenTyreColor()
    {
        for (int i = 0; i < (totalTyre - totalCharc); i++)
        {
            tyreColorArr.Add(color[random.Next(0, 4)]);
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
        Vector3 initialPosition = tyre.GetComponent<TyreController>().GetInitialPosition();
        Vector3 prevPosition;
        int count = 1;
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
                    tyre.transform.SetAsFirstSibling();
                    tyre.GetComponent<TyreController>().ToggleOnTyreMovable2();
                }
                count++;
            }

            else break;
        }

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
    }




}
