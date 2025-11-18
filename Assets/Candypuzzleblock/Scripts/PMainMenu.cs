using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PMainMenu : MonoBehaviour
{
    // Use this for initialization
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Rate()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.alhamad.legopuzzleblock");
    }

    public void More()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=ALHAMAD");
    }


    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ExitThisGame()
    {
        Application.Quit();
    }
}