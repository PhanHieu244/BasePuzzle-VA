public class PauseDialog : Dialog
{
	protected override void Start()
	{
		base.Start();
	}

	public void OnReplayClick()
	{
		Close();
		CSound.instance.PlayButton();
		MainController.instance.Replay();
	}
}
