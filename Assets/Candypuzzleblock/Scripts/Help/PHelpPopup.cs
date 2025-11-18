using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Help popup.
/// </summary>
public class PHelpPopup : MonoBehaviour 
{
	/// <summary>
	/// Raises the close button pressed event.
	/// </summary>
	public void OnCloseButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PStackManager.Instance.OnCloseButtonPressed ();
		}
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		if (PGamePlay.Instance != null) {
			PGamePlay.Instance.OnHelpPopupClosed ();
		}
	}
}
