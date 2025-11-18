using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSelectMode : MonoBehaviour 
{
	[SerializeField]
	private GameObject mainContent;

	void Awake()
	{
		SetUpModeSelection ();
	}

	void SetUpModeSelection()
	{
		Transform modeButton = null;
		foreach(GameModesInfo modeInfo in PGameInfo.Instance.gameModesInfo)
		{
			switch (modeInfo.gameMode) 
			{
			case GameMode.CLASSIC:
				modeButton = mainContent.transform.Find ("btn-classic");
				if (modeButton != null) {
					modeButton.gameObject.SetActive (modeInfo.isActive);
					modeButton = null;
				}
				break;

			case GameMode.TIMED:
				modeButton = mainContent.transform.Find ("btn-timed");
				if (modeButton != null) {
					modeButton.gameObject.SetActive (modeInfo.isActive);
					modeButton = null;
				}
					break;
			case GameMode.BLAST:
				modeButton = mainContent.transform.Find ("btn-blast");
				if (modeButton != null) {
					modeButton.gameObject.SetActive (modeInfo.isActive);
					modeButton = null;
				}
					break;
			case GameMode.ADVANCE:
				modeButton = mainContent.transform.Find ("btn-advance");
				if (modeButton != null) {
					modeButton.gameObject.SetActive (modeInfo.isActive);
					modeButton = null;
				}
				break;
			case GameMode.CHALLENGE:
				modeButton = mainContent.transform.Find ("btn-challenge");
				if (modeButton != null) {
					modeButton.gameObject.SetActive (modeInfo.isActive);
					modeButton = null;
				}
				break;
			}
		}
	}

	public void OnClassicButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PGameController.gameMode = GameMode.CLASSIC;
			PStackManager.Instance.LoadGamePlayFromModeSelection ();
		}
	}

	public void OnTimedButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PGameController.gameMode = GameMode.TIMED;
			PStackManager.Instance.LoadGamePlayFromModeSelection ();
		}
	}

	public void OnBlastButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PGameController.gameMode = GameMode.BLAST;
			PStackManager.Instance.LoadGamePlayFromModeSelection ();
		}
	}

	public void OnAdvanceButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PGameController.gameMode = GameMode.ADVANCE;
			PStackManager.Instance.LoadGamePlayFromModeSelection ();
		}
	}

	public void OnHexaButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PGameController.gameMode = GameMode.CHALLENGE;
			PStackManager.Instance.LoadGamePlayFromModeSelection ();
		}
	}

	public void OnCloseButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PStackManager.Instance.OnCloseButtonPressed ();
		}
	}
}
