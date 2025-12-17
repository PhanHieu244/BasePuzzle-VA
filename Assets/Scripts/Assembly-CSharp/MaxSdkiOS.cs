using System.Runtime.InteropServices;
using UnityEngine;

public class MaxSdkiOS : MaxSdkBase
{
	static MaxSdkiOS()
	{
		MaxSdkBase.InitCallbacks();
	}

	[DllImport("__Internal")]
	private static extern void _MaxSetSdkKey(string sdkKey);

	public static void SetSdkKey(string sdkKey)
	{
		_MaxSetSdkKey(sdkKey);
	}

	[DllImport("__Internal")]
	private static extern void _MaxInitializeSdk();

	public static void InitializeSdk()
	{
		_MaxInitializeSdk();
	}

	[DllImport("__Internal")]
	private static extern bool _MaxIsInitialized();

	public static bool IsInitialized()
	{
		return _MaxIsInitialized();
	}

	[DllImport("__Internal")]
	private static extern int _MaxConsentDialogState();

	public static ConsentDialogState GetConsentDialogState()
	{
		if (!IsInitialized())
		{
			Debug.LogWarning("MAX Ads SDK has not been initialized yet. GetConsentDialogState() may return ConsentDialogState.Unknown");
		}
		return (ConsentDialogState)_MaxConsentDialogState();
	}

	[DllImport("__Internal")]
	private static extern void _MaxSetHasUserConsent(bool hasUserConsent);

	public static void SetHasUserConsent(bool hasUserConsent)
	{
		_MaxSetHasUserConsent(hasUserConsent);
	}

	[DllImport("__Internal")]
	private static extern bool _MaxHasUserConsent();

	public static bool HasUserConsent()
	{
		return _MaxHasUserConsent();
	}

	[DllImport("__Internal")]
	private static extern void _MaxSetIsAgeRestrictedUser(bool isAgeRestrictedUser);

	public static void SetIsAgeRestrictedUser(bool isAgeRestrictedUser)
	{
		_MaxSetIsAgeRestrictedUser(isAgeRestrictedUser);
	}

	[DllImport("__Internal")]
	private static extern bool _MaxIsAgeRestrictedUser();

	public static bool IsAgeRestrictedUser()
	{
		return _MaxIsAgeRestrictedUser();
	}

	[DllImport("__Internal")]
	private static extern void _MaxCreateBanner(string adUnitIdentifier, string bannerPosition);

	public static void CreateBanner(string adUnitIdentifier, BannerPosition bannerPosition)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "create banner");
		_MaxCreateBanner(adUnitIdentifier, bannerPosition.ToString());
	}

	[DllImport("__Internal")]
	private static extern void _MaxShowBanner(string adUnitIdentifier);

	public static void ShowBanner(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "show banner");
		_MaxShowBanner(adUnitIdentifier);
	}

	[DllImport("__Internal")]
	private static extern void _MaxDestroyBanner(string adUnitIdentifier);

	public static void DestroyBanner(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "destroy banner");
		_MaxDestroyBanner(adUnitIdentifier);
	}

	[DllImport("__Internal")]
	private static extern void _MaxHideBanner(string adUnitIdentifier);

	public static void HideBanner(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "hide banner");
		_MaxHideBanner(adUnitIdentifier);
	}

	[DllImport("__Internal")]
	private static extern void _MaxLoadInterstitial(string adUnitIdentifier);

	public static void LoadInterstitial(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "load interstitial");
		_MaxLoadInterstitial(adUnitIdentifier);
	}

	[DllImport("__Internal")]
	private static extern bool _MaxIsInterstitialReady(string adUnitIdentifier);

	public static bool IsInterstitialReady(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "check interstitial loaded");
		return _MaxIsInterstitialReady(adUnitIdentifier);
	}

	[DllImport("__Internal")]
	private static extern void _MaxShowInterstitial(string adUnitIdentifier);

	public static void ShowInterstitial(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "show interstitial");
		if (IsInterstitialReady(adUnitIdentifier))
		{
			_MaxShowInterstitial(adUnitIdentifier);
		}
		else
		{
			Debug.LogWarning("Not showing MAX Ads interstitial: ad not ready");
		}
	}

	[DllImport("__Internal")]
	private static extern void _MaxLoadRewardedAd(string adUnitIdentifier);

	public static void LoadRewardedAd(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "load rewarded ad");
		_MaxLoadRewardedAd(adUnitIdentifier);
	}

	[DllImport("__Internal")]
	private static extern bool _MaxIsRewardedAdReady(string adUnitIdentifier);

	public static bool IsRewardedAdReady(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "check rewarded ad loaded");
		return _MaxIsRewardedAdReady(adUnitIdentifier);
	}

	[DllImport("__Internal")]
	private static extern void _MaxShowRewardedAd(string adUnitIdentifier);

	public static void ShowRewardedAd(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "show rewarded ad");
		if (IsRewardedAdReady(adUnitIdentifier))
		{
			_MaxShowRewardedAd(adUnitIdentifier);
		}
		else
		{
			Debug.LogWarning("Not showing MAX Ads rewarded ad: ad not ready");
		}
	}

	[DllImport("__Internal")]
	private static extern void _MaxTrackEvent(string name);

	public static void TrackEvent(string name)
	{
		_MaxTrackEvent(name);
	}
}
