using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSdkUnityEditor : MaxSdkBase
{
	private static bool _isInitialized;

	private static bool _hasSdkKey;

	private static bool _hasUserConsent;

	private static bool _isAgeRestrictedUser;

	private static readonly HashSet<string> _requestedAdUnits;

	static MaxSdkUnityEditor()
	{
		_hasUserConsent = true;
		_isAgeRestrictedUser = false;
		_requestedAdUnits = new HashSet<string>();
		MaxSdkBase.InitCallbacks();
	}

	public static void SetSdkKey(string sdkKey)
	{
		_hasSdkKey = true;
	}

	public static void InitializeSdk()
	{
		_ensureHaveSdkKey();
		_isInitialized = true;
		_hasSdkKey = true;
		ExecuteWithDelay(delegate
		{
			_isInitialized = true;
		});
	}

	public static bool IsInitialized()
	{
		return _isInitialized;
	}

	public static ConsentDialogState GetConsentDialogState()
	{
		return ConsentDialogState.Unknown;
	}

	public static void SetHasUserConsent(bool hasUserConsent)
	{
		_hasUserConsent = hasUserConsent;
	}

	public static bool HasUserConsent()
	{
		return _hasUserConsent;
	}

	public static void SetIsAgeRestrictedUser(bool isAgeRestrictedUser)
	{
		_isAgeRestrictedUser = isAgeRestrictedUser;
	}

	public static bool IsAgeRestrictedUser()
	{
		return _isAgeRestrictedUser;
	}

	public static void CreateBanner(string adUnitIdentifier, BannerPosition bannerPosition)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "create banner");
		RequestAdUnit(adUnitIdentifier);
	}

	public static void ShowBanner(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "show banner");
		if (!IsAdUnitRequested(adUnitIdentifier))
		{
			Debug.LogWarning("Banner '" + adUnitIdentifier + "' was not created, can not show it");
		}
	}

	public static void DestroyBanner(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "destroy banner");
	}

	public static void HideBanner(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "hide banner");
	}

	public static void LoadInterstitial(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "load interstitial");
	}

	public static bool IsInterstitialReady(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "check interstitial loaded");
		if (!IsAdUnitRequested(adUnitIdentifier))
		{
			Debug.LogWarning("Interstitial '" + adUnitIdentifier + "' was not requested, can not check if it is loaded");
			return false;
		}
		return true;
	}

	public static void ShowInterstitial(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "show interstitial");
		if (!IsAdUnitRequested(adUnitIdentifier))
		{
			Debug.LogWarning("Interstitial '" + adUnitIdentifier + "' was not requested, can not show it");
		}
	}

	public static void LoadRewardedAd(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "load rewarded ad");
		RequestAdUnit(adUnitIdentifier);
	}

	public static bool IsRewardedAdReady(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "check rewarded ad loaded");
		if (!IsAdUnitRequested(adUnitIdentifier))
		{
			Debug.LogWarning("Rewarded ad '" + adUnitIdentifier + "' was not requested, can not check if it is loaded");
			return false;
		}
		return true;
	}

	public static void ShowRewardedAd(string adUnitIdentifier)
	{
		if (!IsAdUnitRequested(adUnitIdentifier))
		{
			Debug.LogWarning("Rewarded ad '" + adUnitIdentifier + "' was not requested, can not show it");
		}
		else
		{
			MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "show rewarded ad");
		}
	}

	public static void TrackEvent(string name)
	{
	}

	private static void RequestAdUnit(string adUnitId)
	{
		_ensureInitialized();
		_requestedAdUnits.Add(adUnitId);
	}

	private static bool IsAdUnitRequested(string adUnitId)
	{
		_ensureInitialized();
		return _requestedAdUnits.Contains(adUnitId);
	}

	private static void _ensureHaveSdkKey()
	{
		if (!_hasSdkKey)
		{
			Debug.LogWarning("MAX Ads SDK did not receive SDK key. Please call Max.SetSdkKey() to assign it");
		}
	}

	private static void _ensureInitialized()
	{
		_ensureHaveSdkKey();
		if (!_isInitialized)
		{
			Debug.LogWarning("MAX Ads SDK is not initialized by the time ad is requested. Please call Max.InitializeSdk() in your first scene");
		}
	}

	private static void ExecuteWithDelay(Action action)
	{
		MaxSdkCallbacks.Instance.StartCoroutine(ExecuteAction(action));
	}

	private static IEnumerator ExecuteAction(Action action)
	{
		yield return null;
		action();
	}
}
