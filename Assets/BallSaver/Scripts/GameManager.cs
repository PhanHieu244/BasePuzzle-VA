using UnityEngine;
using UnityEngine.SceneManagement;
using DxCoder;
using System.Collections;
public class BGameManager : MonoBehaviour
{
    public string ballTag = "Ball";
    private int ballsCollected = 0;
    public GameObject GameWinUI;
    bool isGameEnd;

    public static BGameManager instance;

    public void Awake()
    {
        instance = this; 
    }

    public void Start()
    {
        SoundManager.Instance.PlayMusic(SoundManager.Instance.GameMusic);

        GameWinUI.SetActive(false);
        isGameEnd = false;
    }
    public void CollectBall(GameObject ball)
    {
        ball.tag = "Untagged";
        ballsCollected++;
        CheckGameOver();
    }

    private void CheckGameOver()
    {
      
    }


    public void GameWin() {
        if (!isGameEnd)
        {
            isGameEnd=true;
            int level = PlayerPrefs.GetInt("Level", 1);
            PlayerPrefs.SetInt("Level", (level + 1));
            StartCoroutine(GameWinDelay());
        }

    
    }

    IEnumerator GameWinDelay() {
        SoundManager.Instance.StopMusic();

        SoundManager.Instance.PlaySound(SoundManager.Instance.GameWinMusic);

        yield return new WaitForSeconds(1f);
        GameWinUI.SetActive(true);
    }

    public void GotoMenu() {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
        SceneManager.LoadScene("Menu");

    }
    public void NextLevel() {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

        SceneManager.LoadScene("Game");
    }
}
