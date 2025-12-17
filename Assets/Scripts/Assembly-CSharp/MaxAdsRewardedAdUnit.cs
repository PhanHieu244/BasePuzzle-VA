using System;
using UnityEngine;

[Serializable]
public class MaxAdsRewardedAdUnit : RewardedAdUnit
{
	[SerializeField]
	private string[] androidAdUnitID = new string[1] { "f7882609589450dc" };

	[SerializeField]
	private string[] iosAdUnitID;

	private AdsIdRotator ids;

	public override void Initialize(AdNetworksManager caller, bool GDPRConsent, float delay = 0f)
	{
		Debug.Log("#Ads_rewarded_max: Initialize");
		ids = new AdsIdRotator(androidAdUnitID, iosAdUnitID);
		AddListeners();
		Load();
	}

	private void AddListeners()
	{
		Debug.Log("#Ads_rewarded_max: Subscribe events");
		MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
		MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
		MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
		MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
		MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdHiddenEvent;
		MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
		MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
	}

	private void RemoveListeners()
	{
		Debug.Log("#Ads_rewarded_max: Unsubscribe events");
		MaxSdkCallbacks.OnRewardedAdLoadedEvent -= OnRewardedAdLoadedEvent;
		MaxSdkCallbacks.OnRewardedAdLoadFailedEvent -= OnRewardedAdLoadFailedEvent;
		MaxSdkCallbacks.OnRewardedAdDisplayedEvent -= OnRewardedAdDisplayedEvent;
		MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent -= OnRewardedAdFailedToDisplayEvent;
		MaxSdkCallbacks.OnRewardedAdHiddenEvent -= OnRewardedAdHiddenEvent;
		MaxSdkCallbacks.OnRewardedAdClickedEvent -= OnRewardedAdClickedEvent;
		MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;
	}

	private void Load()
	{
		Debug.Log("#Ads_rewarded_max: LOAD " + ids.currentAdId);
		MaxSdkAndroid.LoadRewardedAd(ids.currentAdId);
		_loadStatus = LoadStatus.loading;
	}

	public override void Show(Action<bool> onRewardVideoWatched)
	{
		Debug.Log("#Ads_rewarded_max: SHOW : status=" + base.loadStatus.ToString() + "  ready=" + MaxSdkAndroid.IsRewardedAdReady(ids.currentAdId));
		if (IsLoaded())
		{
			OnRewardVideoWatched = onRewardVideoWatched;
			Debug.Log("#Ads_rewarded_max: SHOW");
			MaxSdkAndroid.ShowRewardedAd(ids.currentAdId);
		}
		else if (base.loadStatus != 0)
		{
			Debug.Log("#Ads_rewarded_max: Can't show - load...");
			Load();
		}
		else
		{
			Debug.Log("#Ads_rewarded_max: Still loading ...");
		}
	}

	private void OnRewardedAdLoadedEvent(string id)
	{
		Debug.Log("#Ads_rewarded_max: LoadedEvent ");
		_loadStatus = LoadStatus.loaded;
	}

	private void OnRewardedAdLoadFailedEvent(string id, int code)
	{
		Debug.Log("#Ads_rewarded_max: LoadFailedEvent ");
		_loadStatus = LoadStatus.failed;
		ids.NextAd();
		Load();
	}

	private void OnRewardedAdDisplayedEvent(string id)
	{
		Debug.Log("#Ads_rewarded_max: DisplayedEvent ");
		Load();
	}

	private void OnRewardedAdFailedToDisplayEvent(string id, int code)
	{
		Debug.Log("#Ads_rewarded_max: FailedToDisplayEvent ");
		ids.NextAd();
		Load();
	}

	private void OnRewardedAdHiddenEvent(string id)
	{
		Debug.Log("#Ads_rewarded_max: HiddenEvent ");
		if (OnAdClosed != null)
		{
			OnAdClosed();
		}
		Load();
	}

	private void OnRewardedAdClickedEvent(string id)
	{
		Debug.Log("#Ads_rewarded_max: ClickedEvent ");
		if (onAdClick != null)
		{
			onAdClick.Invoke();
		}
		Load();
	}

	private void OnRewardedAdReceivedRewardEvent(string id, MaxSdkBase.Reward reward)
	{
		Debug.Log("#Ads_rewarded_max: ReceivedRewardEvent ");
		if (OnRewardVideoWatched != null)
		{
			OnRewardVideoWatched(true);
		}
	}

	public override void Dispose()
	{
		RemoveListeners();
		ids = null;
	}
}
