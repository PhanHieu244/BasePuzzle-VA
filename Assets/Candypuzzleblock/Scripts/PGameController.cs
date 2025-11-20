using UnityEngine;
using System.Collections;
using PuzzleGames;
using TMPro;

public class PGameController : PSingleton<PGameController> 
{
	public static GameMode gameMode = GameMode.CLASSIC;
	public Canvas UICanvas; 
	
	public TextMeshProUGUI ResetPowerQuantityText;
	public TextMeshProUGUI ClearPowerQuantityText;

	// Checks if interner is available or not.
	public bool isInternetAvailable()
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Quits the game.
	/// </summary>
	public void QuitGame()
	{
		Invoke ("QuitGameWithDelay", 0.5F);
	}

	/// <summary>
	/// Quits the game with delay.
	/// </summary>
	void QuitGameWithDelay ()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit ();
		#endif
	}

	public void TryUseResetPower()
	{
		var resetPowerQuantity = PlayerPrefs.GetInt("ResetPower");
		if (resetPowerQuantity < 1)
		{
			PGamePlayUI.Instance.ShowShop();
		}
		else
		{
			PlayerPrefs.SetInt("ResetPower", resetPowerQuantity - 1);
			PGamePlay.Instance.ResetShape();	
			UpdateResetPowerQuantityText();
		}
	}

	public void TryUseClearPower()
	{
		var clearPowerQuantity = PlayerPrefs.GetInt("ClearPower");
		if (clearPowerQuantity < 1)
		{
			PGamePlayUI.Instance.ShowShop();
		}
		else
		{
			PlayerPrefs.SetInt("ClearPower", clearPowerQuantity - 1);
			PGamePlay.Instance.ClearMap();
			UpdateClearPowerQuantityText();
		}
	}

	public void UpdateResetPowerQuantityText()
	{
		var resetPowerQuantity = PlayerPrefs.GetInt("ResetPower");
		ResetPowerQuantityText.text = resetPowerQuantity.ToString();
	}
	
	public void UpdateClearPowerQuantityText()
	{
		var clearPowerQuantity = PlayerPrefs.GetInt("ClearPower");
		ClearPowerQuantityText.text = clearPowerQuantity.ToString();
	}
	// Use this for initialization
	void Start () 
	{
		
		UpdateResetPowerQuantityText();
		UpdateClearPowerQuantityText();

		StartCoroutine(CoCountdown());
	}
	
	
	public TextMeshProUGUI countdownText;
	public int countdown;
	IEnumerator CoCountdown()
	{
		while (countdown > 0)
		{
			countdownText.text = countdown.ToString();
			yield return new WaitForSeconds(1f);
			countdown--;
		}
            
		StartCoroutine(EndGame());
	}

	IEnumerator  EndGame()
	{
		GameObject gameOverScreen = PStackManager.Instance.SpawnUIScreen ("GameOver");
		
		gameOverScreen.GetComponent<PGameOver> ().SetLevelScore (FindObjectOfType<PScoreManager>().GetScore(), 0);
		var heart = ResourceType.Heart.Manager();
		heart.Subtract(1);
		yield return new WaitForSeconds(2f);
		LoadSceneManager.Instance.LoadScene("Home");
	}

	public IEnumerator WinGame()
	{
		GameObject gameOverScreen = PStackManager.Instance.SpawnUIScreen ("GameWin");
		
		gameOverScreen.GetComponent<PGameOver> ().SetLevelScore (FindObjectOfType<PScoreManager>().GetScore(), 10);
		var gold = ResourceType.Gold.Manager();
		gold.Add(10);
		yield return new WaitForSeconds(2f);
		LevelDataController.instance.CompleteLevel();
		LoadSceneManager.Instance.LoadScene("Home");
	}
}
