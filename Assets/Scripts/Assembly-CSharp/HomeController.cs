public class HomeController : BaseController
{
	private const int FACEBOOK = 0;

	public void OnClick(int index)
	{
		CSound.instance.PlayButton();
	}
}
