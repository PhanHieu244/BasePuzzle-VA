public class ButtonSoundToggle : TButton
{
	protected override void Start()
	{
		base.Start();
		base.IsOn = CSound.instance.IsEnabled();
	}

	public override void OnButtonClick()
	{
		base.OnButtonClick();
		CSound.instance.SetEnabled(base.IsOn);
	}
}
