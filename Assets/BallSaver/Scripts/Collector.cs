using DxCoder;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
    public string ballTag ;
    public int needToCollect;
    [HideInInspector]public  int collectCount;
    public GameObject Blast;
    public TextMeshPro count;
    bool isGameEnd;
    public UnityAction OnBallsCollected; // Event triggered when balls are collected in the cup
    // Start is called before the first frame update
    void Start()
    {
        collectCount = 0;
        count.text = collectCount.ToString();
        isGameEnd = false;
    }

    // Update is called once per frame
    void Update()
    {

        //GameObject[] balls = GameObject.FindGameObjectsWithTag(ballTag);

        //if (balls.Length == 0)
        //{

        //   // GameWin();
        //    // Add your game over logic here
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (other.gameObject.GetComponent<BallInfo>().GetTag().Equals(ballTag))
            {
                // GameManager.instance.CollectBall(other.gameObject);
                collectCount++;
                SoundManager.Instance.PlaySound(SoundManager.Instance.Collect);
                count.text = collectCount.ToString();
                if (!isGameEnd)
                {
                    if (collectCount >= needToCollect)
                    {
                        Debug.Log(collectCount + " Collected \\\\" + needToCollect);
                        OnBallsCollected?.Invoke();
                        // GameManager.instance.GameWin();

                        isGameEnd = true;
                        Blast.SetActive(true);
                    }
                }
                Destroy(other.gameObject);
            }
                
        }
    }

}
