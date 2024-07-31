using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{


    public void RestartLevel()
    {

        String levName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(levName);
    }

    public void NextLevel()
    {
        String levName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Level" + (int.Parse(levName.Substring(5, 1)) + 1).ToString());
    }
}
