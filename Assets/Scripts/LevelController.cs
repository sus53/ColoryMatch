using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject settings;
    string levName;
    public GameObject GameoverCanvas;
    public GameObject SecondChanceCanvas;
    public GameObject Tyreholder;

    public void RestartLevel()
    {

        levName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(levName);

        Time.timeScale = 1f;
    }

    public void NextLevel()
    {
        int lev = settings.GetComponent<Settings>().GetLevel();
        SceneManager.LoadScene("Level" + (lev).ToString());
    }

    public void MainMenu()
    {
        RestartLevel();
        SceneManager.LoadScene("Level0");
    }
    public void ShowGameoverCanvas()
    {
        SecondChanceCanvas.SetActive(false);
        GameoverCanvas.SetActive(true);
    }
    public void DeductCoin()
    {
        GameoverCanvas.SetActive(false);
        settings.GetComponent<Settings>().DeductCoin();
        SecondChanceCanvas.SetActive(false);
        Tyreholder.SetActive(true);
        Tyreholder.transform.GetChild(0).gameObject.SetActive(true);
        Time.timeScale = 1f;
        settings.GetComponent<GlobalScript>().SetTyreMovable(true);
        settings.GetComponent<GlobalScript>().PlayBackgroundAudio();
    }

}
