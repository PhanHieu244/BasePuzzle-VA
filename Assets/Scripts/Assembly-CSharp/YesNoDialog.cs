using System;

public class YesNoDialog : Dialog
{
	public Action onYesClick;

	public Action onNoClick;

	public virtual void OnYesClick()
	{
		if (onYesClick != null)
		{
			onYesClick();
		}
		CSound.instance.PlayButton();
		Close();
	}

	public virtual void OnNoClick()
	{
		if (onNoClick != null)
		{
			onNoClick();
		}
		CSound.instance.PlayButton();
		Close();
	}
}
