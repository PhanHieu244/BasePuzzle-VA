using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using static UnityEngine.InputSystem.InputAction;

public class MainMenuBehaviour : MonoBehaviour
{
#if UNITY_STANDALONE_WIN
    public SceneAsset firstScene;
    public SceneAsset mainMenuScene;
#endif
    public GameObject mainMenu;
    public GameObject creditsMenu;
    public PlayerInput playerInput;
    [FormerlySerializedAs("gameManager")] public MGameManager mGameManager;

    public void StartNewGame()
    {

        HideMenu();
        mGameManager.StartNewGame();
    }
    public void BackToMainMenu()
    {
#if UNITY_STANDALONE_WIN
        SceneManager.LoadScene(mainMenuScene.name);
#endif
#if UNITY_WEBGL
#endif
        HideMenu();
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void HideMenu()
    {
       

    }

    public void ShowMenu(GameObject menuToShow)
    {
       
    }

    internal void ShowGameOverMenu()
    {
        mGameManager.Lose();
    }
}
