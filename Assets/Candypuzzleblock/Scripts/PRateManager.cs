using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

 public class PRateManager : PSingleton<PRateManager>
{
    [FormerlySerializedAs("rateBox")] [SerializeField]
    private PRateBox pRateBox;

    [SerializeField]
    private Text playCountText;

    public int countToRate =0;

    [HideInInspector]
    public int playCount;

    [HideInInspector]
    public bool rateOff = false;

    public void ClickPlay()
    {
        playCount++;
        playCountText.text = playCount.ToString();

        if (playCount % countToRate == 0 && !rateOff)

        {
            pRateBox.gameObject.SetActive(true);
        }
    }
}
