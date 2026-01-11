using System.Collections;
using System.Collections.Generic;
using PuzzleGames;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MGameManager : MonoBehaviour
{
    public GridManager grid;
    [FormerlySerializedAs("moveList")] public MMoveList mMoveList;
    public BonusManager bonusManager;
    
    public ScoreManager scoreManager;
    
    public TextMeshProUGUI timerText;

    public GameObject gameWin;
    public GameObject gameLose;
    
    public Camera moveListCamera;

    private int _timer = 0;
    public void StartNewGame()
    {
        grid.GenerateGrid();
        mMoveList.GenerateMoveList();
        bonusManager.GenerateBonuses();
        int level = LevelDataController.instance.Level;
        scoreManager.SetTargetScore(level);
        StartCoroutine(ResetCam());
    }

    IEnumerator ResetCam()
    {
        moveListCamera.gameObject.SetActive(false);
        yield return null;
        moveListCamera.gameObject.SetActive(true);
    }

    private void Start()
    {
        StartNewGame();
        _timer = 60;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (_timer > 0)
        {
            timerText.text =  _timer.ToString();
            yield return new WaitForSeconds(1); 
            _timer--;
        }

        Lose();
    }

    public void GetMoreTime()
    {
        var gold = ResourceType.Gold.Manager();
        if (gold.GetAmount() > 10 || gold.IsInFreeMode)
        {
            if (!gold.IsInFreeMode)
            {
                gold.Subtract(10);   
            }

            _timer += 5;
        }
        else
        {
            UIToastManager.Instance.Show("You do not have enough gold.");
        }
    }

    public void Win()
    {
        StopAllCoroutines();
        LevelDataController.instance.CompleteLevel();
        gameWin.SetActive(true);
        StartCoroutine(ReturnToHome());
    }

    public void Lose()
    {
        StopAllCoroutines();
        gameLose.SetActive(true);
        StartCoroutine(ReturnToHome());
    }

    IEnumerator ReturnToHome()
    {
        moveListCamera.gameObject.SetActive(false);
        yield return null;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        yield return new WaitForSeconds(2);
        LoadSceneManager.Instance.LoadScene("Home");
    }
}

