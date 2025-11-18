using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSettings : MonoBehaviour 
{
	public void OnRateButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
            #if UNITY_EDITOR
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.ponygames.MagicBlockPuzzle");
#elif UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ponygames.MagicBlockPuzzle");
#elif UNITY_IPHONE
        Application.OpenURL("https://itunes.apple.com/us/app/magic-block-puzzle-breaker/id1402365261?ls=1&mt=8");
#else
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ponygames.MagicBlockPuzzle");
#endif
		}
	}

	public void OnCloseButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PStackManager.Instance.OnCloseButtonPressed ();
		}
	}

	public void OnSelectLanguageButtonPressed(){
		if (PInputManager.Instance.CanInput ()) {

			GameObject currentWindow = PStackManager.Instance.PeekWindow ();
			PStackManager.Instance.PopWindow ();

			PStackManager.Instance.SpawnUIScreen ("SelectLanguage");
			Destroy (currentWindow);

		}
	}
}
