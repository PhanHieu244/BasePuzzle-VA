using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DxCoder;

public class Splash : MonoBehaviour
{
    [SerializeField]
    int WaitSec;
    //[SerializeField]



    // Start is called before the first frame update
    void Start()
    {

        

        StartCoroutine(LoadGame());
    }
    IEnumerator LoadGame() {
        yield return new WaitForSeconds(WaitSec);
        SceneManager.LoadScene("Menu");
    }
}
