using System;
using UnityEngine;

[Serializable]
public class MaxAdsInterstitialAdUnit : InterstitialAdUnit
{
	[SerializeField]
	private string[] androidAdUnitID = new string[1] { "9d494427b7625719" };

	[SerializeField]
	private string[] iosAdUnitID;

	private AdsIdRotator ids;

	private MaxAdsInterstitialAdUnit interstitial;

	private AllInOneMainThreadDispatcher mainThreadDispatcher
	{
		get
		{
			return caller.mainThreadDispatcher;
		}
	}

	public override void Initialize(AdNetworksManager caller, bool GDPRConsent, float delay = 0f)
	{
		Debug.Log("#Ads_interstitial_max: Initialize interstitials " + androidAdUnitID[0] + " ...");
		ids = new AdsIdRotator(androidAdUnitID, iosAdUnitID);
		MaxSdkCallbacks.OnSdkInitializedEvent += OnMaxSdkInitialized;
		if (MaxSdkAndroid.IsInitialized())
		{
			LoadInterstitial();
		}
		base.caller = caller;
		base.GDPRConsent = GDPRConsent;
		InitializeInterstitialAds();
	}

	private void OnMaxSdkInitialized(MaxSdkBase.SdkConfiguration config)
	{
		Debug.Log("#Ads_interstitial_max: MaxAds Initialized - load interstitial");
		LoadInterstitial();
	}

	public void InitializeInterstitialAds()
	{
		Debug.Log("#Ads_interstitial_max: Subscribe to events");
		MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
		MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
		MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
		MaxSdkCallbacks.OnInterstitialDisplayedEvent += OnInterstitialDisplayed;
		MaxSdkCallbacks.OnInterstitialClickedEvent += OnInterstitialClicked;
		MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialHidden;
		LoadInterstitial();
	}

	public void LoadInterstitial()
	{
		Debug.Log("#Ads_interstitial_max: LOAD");
		_loadStatus = LoadStatus.loading;
		MaxSdkAndroid.LoadInterstitial(ids.currentAdId);
	}

	private void OnInterstitialClicked(string adUnitId)
	{
		Debug.Log("#Ads_interstitial_max: OnInterstitialClicked " + adUnitId);
		onAdClick.Invoke();
		LoadInterstitial();
	}

	private void OnInterstitialDisplayed(string adUnitId)
	{
		Debug.Log("#Ads_interstitial_max: OnInterstitialDisplayed " + adUnitId);
		LoadInterstitial();
	}

	private void OnInterstitialHidden(string adUnitId)
	{
		Debug.Log("#Ads_interstitial_max: OnInterstitialHidden " + adUnitId);
		mainThreadDispatcher.Enqueue(delegate
		{
			if (_onAdClosed != null)
			{
				Debug.Log("#Ads_interstitial_max: onAdClosed");
				_onAdClosed();
			}
			if (onAdClosed != null)
			{
				onAdClosed();
			}
			ids.NextAd();
			LoadInterstitial();
		});
	}

	private void OnInterstitialLoadedEvent(string adUnitId)
	{
		_loadStatus = LoadStatus.loaded;
		Debug.Log("#Ads_interstitial_max: OnInterstitialLoadedEvent " + adUnitId);
	}

	private void OnInterstitialFailedEvent(string adUnitId, int errorCode)
	{
		_loadStatus = LoadStatus.failed;
		Debug.Log("#Ads_interstitial_max: OnInterstitialFailedEvent " + adUnitId);
	}

	private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
	{
		_loadStatus = LoadStatus.failed;
		Debug.Log("#Ads_interstitial_max: InterstitialFailedToDisplayEvent " + adUnitId);
		ids.NextAd();
		AdNetworksManager.instance.ActionDelayed(LoadInterstitial, 2f);
	}

	public override bool IsLoaded()
	{
		return MaxSdkAndroid.IsInterstitialReady(ids.currentAdId);
	}

	public override void Show(Action onAdClosed)
	{
		Debug.Log("#Ads_interstitial_max: Interstitial Shown! " + ids.currentAdId);
		isShowing = true;
		_onAdClosed = onAdClosed;
		_loadStatus = LoadStatus.failed;
		MaxSdkAndroid.ShowInterstitial(ids.currentAdId);
		LoadInterstitial();
	}

	public override void Dispose()
	{
		Debug.Log("#Ads_interstitial_max: Unsubscribe events ");
		MaxSdkCallbacks.OnSdkInitializedEvent -= OnMaxSdkInitialized;
		MaxSdkCallbacks.OnInterstitialLoadedEvent -= OnInterstitialLoadedEvent;
		MaxSdkCallbacks.OnInterstitialLoadFailedEvent -= OnInterstitialFailedEvent;
		MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent -= InterstitialFailedToDisplayEvent;
		MaxSdkCallbacks.OnInterstitialDisplayedEvent -= OnInterstitialDisplayed;
		MaxSdkCallbacks.OnInterstitialClickedEvent -= OnInterstitialClicked;
		MaxSdkCallbacks.OnInterstitialHiddenEvent -= OnInterstitialHidden;
	}
}
