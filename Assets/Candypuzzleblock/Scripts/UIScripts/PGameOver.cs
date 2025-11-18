using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PGameOver : MonoBehaviour {

	[SerializeField] Text txtScore;
	[SerializeField] private Text txtBestScore;
	[SerializeField] private Text txtCoinReward;

	public void SetLevelScore(int score, int coinReward)
	{
		int bestScore = PlayerPrefs.GetInt ("BestScore_" + PGameController.gameMode.ToString (), score);

		if (score >= bestScore) {
			PlayerPrefs.SetInt ("BestScore_" + PGameController.gameMode.ToString (), score);
		}

		txtScore.text = string.Format("{0:#,#.}", score.ToString("0"));
        txtBestScore.text = string.Format("{0:#,#.}", (PlayerPrefs.GetInt("BestScore_" +
                                                                          PGameController.gameMode.ToString()).ToString("0")));
		txtCoinReward.text = string.Format("{0:#,#.}", coinReward.ToString("0"));

		PCurrencyManager.Instance.AddCoinBalance (coinReward);
	}



	public void OnHomeButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PStackManager.Instance.OnCloseButtonPressed ();
		}
	}

	public void OnReplayButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PStackManager.Instance.RestartGamePlay ();
		}
	}

    public void OnRewardedVideoButtonPressed()
    {
        if(PInputManager.Instance.CanInput())
        {
            PAudioManager.Instance.PlayButtonClickSound();
            PStackManager.Instance.ContinueGameplay();
            PGamePlay.Instance.InstantRescue();
        }
    }

    public void Rate()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.alhamad.legopuzzleblocklego");
    }
}
