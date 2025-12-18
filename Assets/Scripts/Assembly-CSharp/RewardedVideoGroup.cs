using System;
using UnityEngine;

public class RewardedVideoGroup : MonoBehaviour
{
	public GameObject buttonGroup;

	public GameObject textGroup;

	public TimerText timerText;

	private const string ACTION_NAME = "rewarded_video";

	private void Start()
	{
		if (timerText != null)
		{
			TimerText obj = timerText;
			obj.onCountDownComplete = (Action)Delegate.Combine(obj.onCountDownComplete, new Action(OnCountDownComplete));
		}
		if (!IsAvailableToShow())
		{
			buttonGroup.SetActive(false);
			if (IsAdAvailable() && !IsActionAvailable())
			{
				int time = (int)((double)ConfigController.Config.rewardedVideoPeriod - CUtils.GetActionDeltaTime("rewarded_video"));
				ShowTimerText(time);
			}
		}
		InvokeRepeating("IUpdate", 1f, 1f);
	}

	private void IUpdate()
	{
		buttonGroup.SetActive(IsAvailableToShow());
	}

	public void OnClick()
	{
		AdNetworksManager.instance.ShowRewardedVideo(delegate(bool result)
		{
			HandleRewardBasedVideoRewarded(result);
		});
		CSound.instance.PlayButton();
	}

	private void ShowTimerText(int time)
	{
		if (textGroup != null)
		{
			textGroup.SetActive(true);
			timerText.SetTime(time);
			timerText.Run();
		}
	}

	public void HandleRewardBasedVideoRewarded(bool success)
	{
		if (success)
		{
			buttonGroup.SetActive(false);
			ShowTimerText(ConfigController.Config.rewardedVideoPeriod);
		}
	}

	private void OnCountDownComplete()
	{
		textGroup.SetActive(false);
		if (IsAdAvailable())
		{
			buttonGroup.SetActive(true);
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
		return false;
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause && textGroup != null && textGroup.activeSelf)
		{
			int time = (int)((double)ConfigController.Config.rewardedVideoPeriod - CUtils.GetActionDeltaTime("rewarded_video"));
			ShowTimerText(time);
		}
	}
}
