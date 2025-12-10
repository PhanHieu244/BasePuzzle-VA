using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public  float Speed= 1f;
    public TextMeshProUGUI loading;
    public Slider slider;
    int startTime = 0;
    int newTime = 100;
    public Text CurrentLevelText;
    int CurrentLevel;


    // Start is called before the first frame update
    void Start()
    {
      
    }

    private void Awake()
    {
        CurrentLevel = PlayerPrefs.GetInt("Level", 1);
        CurrentLevelText.text = "LEVEL " + CurrentLevel;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        // Animation for increasing and decreasing of coins amount
         float seconds = Speed;
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            loading.text = "Loading.... " + Mathf.Floor(Mathf.Lerp(startTime, newTime, (elapsedTime / seconds))) +"%";
            slider.value = Mathf.Floor(Mathf.Lerp(startTime, newTime, (elapsedTime / seconds)));
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        startTime = newTime;
        loading.text = "Loading.... " + newTime + "%";
        SceneManager.LoadScene("Game");
    }
}
