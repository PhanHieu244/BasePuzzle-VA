using UnityEngine;
using UnityEngine.UI;

public class ButtonWorld : MonoBehaviour
{
	public Text progressText;

	private int world;

	private void Start()
	{
		world = base.transform.GetSiblingIndex() + 1;
		int num = LevelController.GetUnlockLevel(world) - 1;
		progressText.text = num + "/60";
	}

	public void OnClick()
	{
		GameState.chosenWorld = world;
		CUtils.LoadScene(2, true);
		CSound.instance.PlayButton();
	}
}
