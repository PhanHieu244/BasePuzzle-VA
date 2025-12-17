using UnityEngine;

public class QuitGame : MonoBehaviour
{
	public static QuitGame instance;

	private void Awake()
	{
		instance = this;
	}

	public void ShowConfirmDialog()
	{
		DialogController.instance.ShowDialog(DialogType.QuitGame);
	}
}
