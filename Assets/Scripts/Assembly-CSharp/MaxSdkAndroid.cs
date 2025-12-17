using UnityEngine;

public class MaxSdkAndroid : MaxSdkBase
{
	private static readonly AndroidJavaClass MaxUnityPluginClass;

	static MaxSdkAndroid()
	{
		MaxUnityPluginClass = new AndroidJavaClass("com.applovin.mediation.unity.MaxUnityPlugin");
		MaxSdkBase.InitCallbacks();
	}

	public static void SetSdkKey(string sdkKey)
	{
		MaxUnityPluginClass.CallStatic("setSdkKey", sdkKey);
	}

	public static void InitializeSdk()
	{
		MaxUnityPluginClass.CallStatic("initializeSdk");
	}

	public static bool IsInitialized()
	{
		return MaxUnityPluginClass.CallStatic<bool>("isInitialized", new object[0]);
	}

	public static ConsentDialogState GetConsentDialogState()
	{
		if (!IsInitialized())
		{
			Debug.LogWarning("MAX Ads SDK has not been initialized yet. GetConsentDialogState() may return ConsentDialogState.Unknown");
		}
		return (ConsentDialogState)MaxUnityPluginClass.CallStatic<int>("getConsentDialogState", new object[0]);
	}

	public static void SetHasUserConsent(bool hasUserConsent)
	{
		MaxUnityPluginClass.CallStatic("setHasUserConsent", hasUserConsent);
	}

	public static bool HasUserConsent()
	{
		return MaxUnityPluginClass.CallStatic<bool>("hasUserConsent", new object[0]);
	}

	public static void SetIsAgeRestrictedUser(bool isAgeRestrictedUser)
	{
		MaxUnityPluginClass.CallStatic("setIsAgeRestrictedUser", isAgeRestrictedUser);
	}

	public static bool IsAgeRestrictedUser()
	{
		return MaxUnityPluginClass.CallStatic<bool>("isAgeRestrictedUser", new object[0]);
	}

	public static void CreateBanner(string adUnitIdentifier, BannerPosition bannerPosition)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "create banner");
		MaxUnityPluginClass.CallStatic("createBanner", adUnitIdentifier, bannerPosition.ToString());
	}

	public static void ShowBanner(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "show banner");
		MaxUnityPluginClass.CallStatic("showBanner", adUnitIdentifier);
	}

	public static void DestroyBanner(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "destroy banner");
		MaxUnityPluginClass.CallStatic("destroyBanner", adUnitIdentifier);
	}

	public static void HideBanner(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "hide banner");
		MaxUnityPluginClass.CallStatic("hideBanner", adUnitIdentifier);
	}

	public static void LoadInterstitial(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "load interstitial");
		MaxUnityPluginClass.CallStatic("loadInterstitial", adUnitIdentifier);
	}

	public static bool IsInterstitialReady(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "check interstitial loaded");
		return MaxUnityPluginClass.CallStatic<bool>("isInterstitialReady", new object[1] { adUnitIdentifier });
	}

	public static void ShowInterstitial(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "show interstitial");
		if (IsInterstitialReady(adUnitIdentifier))
		{
			MaxUnityPluginClass.CallStatic("showInterstitial", adUnitIdentifier);
		}
		else
		{
			Debug.LogWarning("Not showing MAX Ads interstitial: ad not ready");
		}
	}

	public static void LoadRewardedAd(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "load rewarded ad");
		MaxUnityPluginClass.CallStatic("loadRewardedAd", adUnitIdentifier);
	}

	public static bool IsRewardedAdReady(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "check rewarded ad loaded");
		return MaxUnityPluginClass.CallStatic<bool>("isRewardedAdReady", new object[1] { adUnitIdentifier });
	}

	public static void ShowRewardedAd(string adUnitIdentifier)
	{
		MaxSdkBase.ValidateAdUnitIdentifier(adUnitIdentifier, "show rewarded ad");
		if (IsRewardedAdReady(adUnitIdentifier))
		{
			MaxUnityPluginClass.CallStatic("showRewardedAd", adUnitIdentifier);
		}
		else
		{
			Debug.LogWarning("Not showing MAX Ads rewarded ad: ad not ready");
		}
	}

	public static void TrackEvent(string name)
	{
		MaxUnityPluginClass.CallStatic("trackEvent", name);
	}
}
