using UnityEngine;

public class ButtonMoreGames : MyButton
{
	public override void OnButtonClick()
	{
		base.OnButtonClick();
		Application.OpenURL("https://play.google.com/store/apps/developer?id=Superpow");
	}
}
