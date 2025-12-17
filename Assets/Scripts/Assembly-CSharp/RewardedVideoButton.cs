using UnityEngine;

public class RewardedVideoButton : MonoBehaviour
{
	private const string ACTION_NAME = "rewarded_video";

	private void Start()
	{
	}

	public void OnClick()
	{
		if (IsAvailableToShow())
		{
			AdNetworksManager.instance.ShowRewardedVideo(delegate(bool result)
			{
				HandleRewardBasedVideoRewarded(result);
			});
		}
		else if (!IsActionAvailable())
		{
			int num = (int)((double)ConfigController.Config.rewardedVideoPeriod - CUtils.GetActionDeltaTime("rewarded_video"));
			Toast.instance.ShowMessage("Please wait " + num + " seconds for the next ad");
		}
		else
		{
			Toast.instance.ShowMessage("Ad is not available now, please wait..");
		}
		CSound.instance.PlayButton();
	}

	public void HandleRewardBasedVideoRewarded(bool success)
	{
		if (success)
		{
			int rewardedVideoAmount = ConfigController.Config.rewardedVideoAmount;
			GameState.hint.ChangeValue(rewardedVideoAmount);
			string text = ((rewardedVideoAmount != 1) ? " hints" : " hint");
			Toast.instance.ShowMessage("You've received " + rewardedVideoAmount + text, 3f);
			CUtils.SetActionTime("rewarded_video");
		}
	}

	public bool IsAvailableToShow()
	{
		return IsActionAvailable() && IsAdAvailable();
	}

	private bool IsActionAvailable()
	{
		return CUtils.IsActionAvailable("rewarded_video", ConfigController.Config.rewardedVideoPeriod);
	}

	private bool IsAdAvailable()
	{
		return AdNetworksManager.instance.RewardedVideoLoaded();
	}
}
