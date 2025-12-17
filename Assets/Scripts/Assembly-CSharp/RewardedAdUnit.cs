using System;
using UnityEngine;

[Serializable]
public abstract class RewardedAdUnit : AdUnit
{
	[HideInInspector]
	public bool isShowing;

	public Action<bool> OnRewardVideoWatched;

	public Action OnAdClosed;

	public bool videoLoaded
	{
		get
		{
			return _loadStatus == LoadStatus.loaded;
		}
	}

	public AllInOneMainThreadDispatcher mainThreadDispatcher
	{
		get
		{
			return caller.mainThreadDispatcher;
		}
	}

	public abstract void Show(Action<bool> onRewardVideoWatched);

	public abstract void Dispose();
}
