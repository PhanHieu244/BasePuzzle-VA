using System;

public class OkDialog : Dialog
{
	public Action onOkClick;

	public virtual void OnOkClick()
	{
		CSound.instance.PlayButton();
		if (onOkClick != null)
		{
			onOkClick();
		}
		CSound.instance.PlayButton();
		Close();
	}
}
