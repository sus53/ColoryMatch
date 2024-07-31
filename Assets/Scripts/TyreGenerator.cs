using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyreGenerator : MonoBehaviour
{
    public GameObject Tyre;
    public GameObject TyreGround;
    [SerializeField] int row;
    [SerializeField] int col;
    [SerializeField] float genSpeed;
    public float initial_x = -0f;
    public float initial_y = 0.135f;
    public float initial_z = 0.3f;
    public float diff_x = 0.2f;
    public float diff_z = -0.3f;
    public String AssetName;
    public Transform Tyres;
    public GameObject Global;
    public Transform CharacterTyreGround;
    private Color[] color;
    void Start()
    {
        color = Global.GetComponent<GlobalScript>().GenTyreColor();
        StartCoroutine(StartSpawning());
    }

    public int GetRow()
    {
        return row;
    }

    public int GetCol()
    {
        return col;
    }

    IEnumerator StartSpawning()
    {
        float x = initial_x;
        float z = initial_z;
        int colorIndex = 0;
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                yield return new WaitForSeconds(genSpeed);
                GameObject TyreClone = Instantiate(Tyre, Tyres);
                GameObject TyreGroundClone = Instantiate(TyreGround, CharacterTyreGround);
                TyreClone.name = AssetName + (i + 1) + (j + 1);
                TyreGroundClone.name = AssetName + (i + 1) + (j + 1);
                StartCoroutine(ChangeScale(TyreClone, new Vector3(12f, 15f, 12f), 0.5f));
                if (i == 0)
                {
                    TyreClone.GetComponent<TyreController>().ToggleOnTyreMovable2();
                }
                TyreClone.GetComponent<Renderer>().materials[0].SetFloat("_DissolveStrength", 0);
                TyreClone.GetComponent<Renderer>().materials[1].SetFloat("_DissolveStrength", 0);
                TyreClone.GetComponent<Renderer>().materials[0].SetColor("_Color", color[colorIndex]);
                TyreClone.GetComponent<Renderer>().materials[1].SetColor("_Color", color[colorIndex]);
                TyreClone.transform.position = new Vector3(x, initial_y, z);
                TyreGroundClone.transform.position = new Vector3(x, 0.01f, z - 0.02f);
                TyreGroundClone.transform.localScale = new Vector3(0.17f, 0.13f, 0.001f);
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
}
