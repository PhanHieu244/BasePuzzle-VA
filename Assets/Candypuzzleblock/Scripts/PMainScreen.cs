using UnityEngine;
using System.Collections;

public class PMainScreen : PUIScreen 
{	
	/// <summary>
	/// Init this instance.
	/// </summary>
	public override void init()
	{
		base.init ();
        PGameController.gameMode = GameMode.CLASSIC;
        PStackManager.Instance.LoadGamePlayFromModeSelection();
	}

	/// <summary>
	/// Raises the play button pressed event.
	/// </summary>
	public void OnPlayButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PStackManager.Instance.SpawnUIScreen ("SelectMode");
		}
	}
}
