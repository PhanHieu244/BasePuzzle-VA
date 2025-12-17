using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class MaxAdsBannerUnit : BannerAdUnit
{
	private bool _isBannerShow;

	[SerializeField]
	private string[] androidAdUnitID = new string[1] { "51a4ebe43b71f073" };

	[SerializeField]
	private string[] iosAdUnitID;

	private AdsIdRotator ids;

	[SerializeField]
	private bool isShowOnInit;

	private bool isInitialized;

	private const float _TIME_TO_WAIT = 20f;

	private MaxSdkBase.BannerPosition position
	{
		get
		{
			switch (adPosition)
			{
			case Position.Top:
				return MaxSdkBase.BannerPosition.TopCenter;
			case Position.Bottom:
				return MaxSdkBase.BannerPosition.BottomCenter;
			default:
				return MaxSdkBase.BannerPosition.BottomCenter;
			}
		}
	}

	public override void Hide()
	{
		for (int i = 0; i < ids.totalAdIds; i++)
		{
			MaxSdkAndroid.HideBanner(ids.GetUnitId(i));
		}
		_isBannerShow = false;
	}

	public override void Initialize(AdNetworksManager caller, bool GDPRConsent, float delay = 0f)
	{
		Debug.Log("#Ads_banner_max: Init");
		ids = new AdsIdRotator(androidAdUnitID, iosAdUnitID);
		for (int i = 0; i < ids.currentUnitIds.Length; i++)
		{
			MaxSdkAndroid.CreateBanner(ids.currentUnitIds[i], position);
		}
		MaxSdkCallbacks.OnBannerAdLoadedEvent += OnBannerAdLoadedEvent;
		MaxSdkCallbacks.OnBannerAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
		MaxSdkCallbacks.OnBannerAdClickedEvent += OnBannerAdClickedEvent;
		MaxSdkCallbacks.OnBannerAdExpandedEvent += OnBannerAdExpandedEvent;
		MaxSdkCallbacks.OnBannerAdCollapsedEvent += OnBannerAdCollapsedEvent;
		isInitialized = true;
		if (isShowOnInit)
		{
			Show();
		}
	}

	public override void Show()
	{
		Debug.Log("#Ads_banner_max: Show...");
		_isBannerShow = true;
		if (isInitialized && base.loadStatus == LoadStatus.loaded)
		{
			Debug.Log("#Ads_banner_max: Showed!");
			MaxSdkAndroid.ShowBanner(ids.currentAdId);
		}
		else
		{
			Debug.Log("#Ads_banner_max: Wait...");
			AdNetworksManager.instance.StartCoroutine(ShowAfterInit());
		}
	}

	private IEnumerator ShowAfterInit()
	{
		float timer = 0f;
		while (!isInitialized && 20f > timer)
		{
			yield return null;
			timer += Time.deltaTime;
		}
		while (base.loadStatus != LoadStatus.loaded && 20f > timer)
		{
			yield return null;
			timer += Time.deltaTime;
		}
		if (timer <= 20f)
		{
			Debug.Log("#Ads_banner_max: Wait OK!");
			if (!_isBannerShow)
			{
				Debug.Log("#Ads_banner_max: Show dismissed!");
				yield break;
			}
			Debug.Log("#Ads_banner_max: SHOWING after waiting");
			MaxSdkAndroid.ShowBanner(ids.currentAdId);
		}
		else
		{
			Debug.Log("#Ads_banner_max: Wait FAIL (" + timer + ")");
		}
	}

	private void OnBannerAdLoadedEvent(string id)
	{
		Debug.Log("#Ads_banner_max: LOADED " + id);
		_loadStatus = LoadStatus.loaded;
	}

	private void OnBannerAdLoadFailedEvent(string id, int fail)
	{
		Debug.Log("#Ads_banner_max: LOAD FAILED " + id);
		if (string.Compare(id, ids.currentAdId) == 0)
		{
			ids.NextAd();
			Show();
		}
	}

	private void OnBannerAdClickedEvent(string id)
	{
		Debug.Log("#Ads_banner_max: CLICKED " + id);
		onAdClick.Invoke();
	}

	private void OnBannerAdExpandedEvent(string id)
	{
		Debug.Log("#Ads_banner_max: AD_EXPANDED " + id);
	}

	private void OnBannerAdCollapsedEvent(string id)
	{
		Debug.Log("#Ads_banner_max: AD_COLLAPSED " + id);
	}

	public override void Dispose()
	{
		Debug.Log("#Ads_banner_max: Dispose!");
		MaxSdkCallbacks.OnBannerAdLoadedEvent -= OnBannerAdLoadedEvent;
		MaxSdkCallbacks.OnBannerAdLoadFailedEvent -= OnBannerAdLoadFailedEvent;
		MaxSdkCallbacks.OnBannerAdClickedEvent -= OnBannerAdClickedEvent;
		MaxSdkCallbacks.OnBannerAdExpandedEvent -= OnBannerAdExpandedEvent;
		MaxSdkCallbacks.OnBannerAdCollapsedEvent -= OnBannerAdCollapsedEvent;
		for (int i = 0; i < ids.totalAdIds; i++)
		{
			MaxSdkAndroid.DestroyBanner(ids.GetUnitId(i));
		}
	}
}
