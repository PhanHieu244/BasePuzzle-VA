using UnityEngine;

public class FirstSceneController : MonoBehaviour
{
	public static FirstSceneController instance;

	private void Awake()
	{
		instance = this;
		Application.targetFrameRate = 60;
		CPlayerPrefs.useRijndael(true);
		if (!CUtils.IsGameInitialzied())
		{
			CUtils.SetInitGame();
		}
		CUtils.SetAndroidVersion("1.0.0");
		CUtils.SetIOSVersion("1.0");
		CUtils.SetWindowsPhoneVersion("1.0.0.0");
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !DialogController.instance.IsDialogShowing())
		{
			QuitGame.instance.ShowConfirmDialog();
		}
	}
}
