using UnityEngine;
using System.Collections;

public class PQuitconfirmGame : MonoBehaviour
{
	/// <summary>
	/// Raises the close button pressed event.
	/// </summary>
	public void OnStayButtonPressed ()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PStackManager.Instance.OnCloseButtonPressed ();
		}
	}

	/// <summary>
	/// Raises the ok button pressed event.
	/// </summary>
	public void OnQuitButtonPressed ()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PGameController.Instance.QuitGame ();
			PStackManager.Instance.OnCloseButtonPressed ();
		}
	}
}
