using UnityEngine;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour
{
	private int page;

	private int level;

	public Text levelText;

	public Sprite pass;

	public Sprite current;

	public Sprite locked;

	private void Start()
	{
		int siblingIndex = base.transform.GetSiblingIndex();
		int siblingIndex2 = base.transform.parent.GetSiblingIndex();
		level = siblingIndex2 * 20 + siblingIndex + 1;
		levelText.text = level.ToString();
		int unlockLevel = LevelController.GetUnlockLevel(GameState.chosenWorld);
		GetComponent<Image>().sprite = ((level < unlockLevel) ? pass : ((level != unlockLevel) ? locked : current));
		if (level > unlockLevel)
		{
			levelText.gameObject.SetActive(false);
			GetComponent<Button>().interactable = false;
		}
	}

	public void OnClick()
	{
		GameState.chosenLevel = level;
		CUtils.LoadScene(3, true);
		CSound.instance.PlayButton();
		Debug.Log("### Start Level!");
		SingletonMono<TimeTracker>.instance.StartTimer();
	}
}
