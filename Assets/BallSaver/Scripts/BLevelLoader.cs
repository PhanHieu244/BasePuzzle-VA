using System.Collections;
using System.Collections.Generic;
using PuzzleGames;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BLevelLoader : MonoBehaviour
{

    public GameObject[] Levels;
    int CurrentLevel;

    [SerializeField]
    public TextMeshProUGUI LevelText;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        CurrentLevel = LevelDataController.instance.Level;
        LevelText.text = "Level " + CurrentLevel;
      //  NextLevel.text = "" + (CurrentLevel + 1);
        Instantiate(Levels[CurrentLevel]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
