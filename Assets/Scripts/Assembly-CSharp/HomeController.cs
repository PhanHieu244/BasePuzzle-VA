public class HomeController : BaseController
{
	private const int FACEBOOK = 0;

	public void OnClick(int index)
	{
		if (index == 0)
		{
			CUtils.LikeFacebookPage(ConfigController.Config.facebookPageID);
		}
		CSound.instance.PlayButton();
	}

	protected override void Start()
	{
		base.Start();
		AdNetworksManager.instance.ShowBanner();
	}
}
