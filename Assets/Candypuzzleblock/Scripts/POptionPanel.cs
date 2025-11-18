using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class POptionPanel : MonoBehaviour 
{
	/// <summary>
	/// Raises the settings button pressed event.
	/// </summary>
	public void OnSettingsButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PStackManager.Instance.SpawnUIScreen ("Settings", true);
		}
	}

	/// <summary>
	/// Raises the messages button pressed event.
	/// </summary>
	public void OnMessagesButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
		}
	}

	/// <summary>
	/// Raises the rate button pressed event.
	/// </summary>
	public void OnRateButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();

			if (PGameController.Instance.isInternetAvailable ()) {
				Application.OpenURL (PGameInfo.ReviewURL);			
			}
		}	
	}

	/// <summary>
	/// Raises the shop button pressed event.
	/// </summary>
	public void OnShopButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PStackManager.Instance.SpawnUIScreen ("Shop", true);	
		}
	}

	/// <summary>
	/// Raises the select language button pressed event.
	/// </summary>
	public void OnSelectLanguageButtonPressed()
	{
		PAudioManager.Instance.PlayButtonClickSound ();
		PStackManager.Instance.SpawnUIScreen ("SelectLanguage", true);	
	}

	/// <summary>
	/// Raises the share button pressed event.
	/// </summary>
	public void OnShareButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
		}
	}

}
