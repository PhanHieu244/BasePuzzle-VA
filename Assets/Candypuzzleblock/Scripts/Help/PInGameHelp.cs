using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PInGameHelp : MonoBehaviour 
{
	public GameObject helpObject = null;

	/// <summary>
	/// Starts the help.
	/// </summary>
	public void StartHelp()
	{
		switch (PGameController.gameMode) {
		case GameMode.CLASSIC:
		case GameMode.CHALLENGE:
			ShowBasicHelp ();
			break;
		case GameMode.BLAST:
			PStackManager.Instance.SpawnUIScreen ("Help-Bomb-Mode");
			PlayerPrefs.SetInt ("isHelpShown_" + PGameController.gameMode.ToString (), 1);
			break;
		case GameMode.TIMED:
			PGamePlay.Instance.timeSlider.PauseTimer ();
			PStackManager.Instance.SpawnUIScreen ("Help-TimeMode");
			PlayerPrefs.SetInt ("isHelpShown_" + PGameController.gameMode.ToString (), 1);
			break;
		case GameMode.ADVANCE:
			PStackManager.Instance.SpawnUIScreen ("Help-Advance-Mode");
			PlayerPrefs.SetInt ("isHelpShown_" + PGameController.gameMode.ToString (), 1);
			break;
		}

	}

	/// <summary>
	/// Shows the basic help.
	/// </summary>
	public void ShowBasicHelp()
	{
		
	}

	/// <summary>
	/// Stops the help.
	/// </summary>
	public void StopHelp()
	{
		helpObject = PStackManager.Instance.PeekWindow ();
		PStackManager.Instance.PopWindow ();

		if (helpObject != null && helpObject.name.Contains("Help-Classic")) {
			Destroy (helpObject);
			PlayerPrefs.SetInt ("isBasicHelpShown", 1);
		} 
		PGamePlay.Instance.isHelpOnScreen = false;
		Destroy (this);
	}
}
