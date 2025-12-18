using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdNetworksManager : MonoBehaviour
{
	public enum BannerAdNetworks
	{
		none = 0,
		admob = 1,
		maxads = 2
	}

	public enum InterstitialAdNetworks
	{
		none = 0,
		admob = 1,
		maxads = 2
	}

	public enum RewardedVideoAdNetworks
	{
		none = 0,
		admob = 1,
		maxads = 2
	}

	public static AdNetworksManager instance;

	public BannerAdNetworks bannerAdNetworkToUse;

	public InterstitialAdNetworks interstitialAdNetworkToUse;

	public RewardedVideoAdNetworks rewardedVideoAdNetworkToUse;

	private int _intervalBetweenInterstitialsCurrentDefault;

	private int _secondsToWaitToShowInterstitialOnFirstLaunchCurrentDefault;

	public string advertisingId;

	public string maxSdkKey = "_M9Fn_01QeA1rhbGelKPFNHTaQQE-lDz5NDtNEuot3qhlQzOAHoPf4zEVNST7gRC2f3PuyCm_0SlRP7bgv0K-z";

	[SerializeField]
	private AdsSettings settingsDefault;

	[Space]
	public bool autoInitialize = true;

	public UnityEvent onBeforeInitialization;

	public UnityEvent onInitializationCompleted;

	private bool initialized;

	private string adsEnabledPlayerPrefString = "ALLINONE_ADS_ENABLED";

	private AllInOneMainThreadDispatcher _mainThreadDispatcher;

	private IntervalsSettings[] _intervalsRulesGlobal;

	private Dictionary<string, PerInterstitialIntervals> _perInterstitialRules;

	public const bool IS_LOG_SHOW = true;

	public int IntervalBetweenInterstitials
	{
		get
		{
			return _intervalBetweenInterstitialsCurrentDefault;
		}
	}

	public bool AdsEnabled
	{
		get
		{
			int @int = PlayerPrefs.GetInt(adsEnabledPlayerPrefString, 1);
			if (@int == 1)
			{
				return true;
			}
			return false;
		}
		set
		{
			if (value)
			{
				PlayerPrefs.SetInt(adsEnabledPlayerPrefString, 1);
			}
			else
			{
				PlayerPrefs.SetInt(adsEnabledPlayerPrefString, 0);
			}
		}
	}

	public int FirstLaunchDate
	{
		get
		{
			return PlayerPrefs.GetInt("FIRST_LAUNCH_DATE", 0);
		}
		set
		{
			PlayerPrefs.SetInt("FIRST_LAUNCH_DATE", value);
		}
	}

	public int LastInterstitialShowTime
	{
		get
		{
			return PlayerPrefs.GetInt("LAST_INTERSTITIAL_SHOW_TIME", 0);
		}
		set
		{
			PlayerPrefs.SetInt("LAST_INTERSTITIAL_SHOW_TIME", value);
		}
	}

	public int AdsClickCount
	{
		get
		{
			return BannerClickCount + InterstitialClickCount + RewardedVideoClickCount;
		}
	}

	public int BannerClickCount
	{
		get
		{
			return PlayerPrefs.GetInt("ALLINONE_BANNER_CLICK_COUNT", 0);
		}
		set
		{
			PlayerPrefs.SetInt("ALLINONE_BANNER_CLICK_COUNT", value);
		}
	}

	public int InterstitialClickCount
	{
		get
		{
			return PlayerPrefs.GetInt("ALLINONE_INTERSTITIAL_CLICK_COUNT", 0);
		}
		set
		{
			PlayerPrefs.SetInt("ALLINONE_INTERSTITIAL_CLICK_COUNT", value);
		}
	}

	public int RewardedVideoClickCount
	{
		get
		{
			return PlayerPrefs.GetInt("ALLINONE_REWARDED_CLICK_COUNT", 0);
		}
		set
		{
			PlayerPrefs.SetInt("ALLINONE_REWARDED_CLICK_COUNT", value);
		}
	}

	public AllInOneMainThreadDispatcher mainThreadDispatcher
	{
		get
		{
			if (_mainThreadDispatcher == null)
			{
				_mainThreadDispatcher = GetComponent<AllInOneMainThreadDispatcher>();
			}
			return _mainThreadDispatcher;
		}
	}

	private PerInterstitialIntervals GetIntervalSettings(string name)
	{
		if (_perInterstitialRules.ContainsKey(name))
		{
			if (_perInterstitialRules[name].settings.Length == 0)
			{
				_perInterstitialRules[name].settings = _intervalsRulesGlobal;
			}
			return _perInterstitialRules[name];
		}
		return _perInterstitialRules[string.Empty];
	}

	private IntervalsSettings GetIntervalSettingsForTIme(IntervalsSettings[] settings, int time)
	{
		IntervalsSettings intervalsSettings = new IntervalsSettings();
		intervalsSettings.afterMinutes = 0;
		intervalsSettings.intervalBetweenInterstitials = _intervalBetweenInterstitialsCurrentDefault;
		for (int i = 0; i < settings.Length; i++)
		{
			if (settings[i].afterMinutes * 60 > time)
			{
				return intervalsSettings;
			}
			intervalsSettings = settings[i];
		}
		return intervalsSettings;
	}

	private void Awake()
	{
		if (instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		instance = this;
		if (autoInitialize)
		{
			Initialize();
		}
	}

	public void Initialize(bool GDPRConsent = true)
	{
		Debug.Log("#ADS: Initialize DEFAULT");
		settingsDefault.overrideInterstitialIntervalSettings = true;
		Initialize(settingsDefault);
	}

	public void Initialize(AdsSettings settings, bool GDPRConsent = true)
	{
		if (initialized)
		{
			Reset();
		}
		if (_intervalsRulesGlobal == null || _perInterstitialRules == null)
		{
			Debug.Log("#ADS: Read default settings first");
			settingsDefault.overrideInterstitialIntervalSettings = true;
			ReadSettings(settingsDefault);
		}
		Debug.Log("#ADS: AllInOne - Initialization started with GDPRConsent = " + GDPRConsent);
		if (settings.isUseCustomAdUnits)
		{
			
		}
		else
		{
			
		}
		ReadSettings(settings);
		if (FirstLaunchDate == 0)
		{
			FirstLaunchDate = CurrentTime();
		}
		onBeforeInitialization.Invoke();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!initialized)
		{
			
		}
		
		onInitializationCompleted.Invoke();
		RequestAndSetAdvertisingID();
		GameObject gameObject = GameObject.Find("MaxSdkCallbacks");
		if ((bool)gameObject)
		{
			gameObject.transform.SetParent(base.transform);
		}
		initialized = true;
	}

	public void ShowBanner()
	{
		Debug.Log("#ADS: Show Banner");
		if (!initialized)
		{
			StartCoroutine(WaitForInit(ShowBanner));
		}
		else if (AdsEnabled)
		{
		
		}
		else
		{
			Debug.LogWarning("#ADS: Calling banner but ads are not enabled");
		}
	}

	private IEnumerator WaitForInit(Action action)
	{
		Debug.Log("#ADS: Waiting for init");
		while (!initialized)
		{
			yield return null;
		}
		action();
	}

	public void HideBanner()
	{
		Debug.Log("#ADS: Hide Banner ? " + initialized);
		if (initialized)
		{
			Debug.Log("#ADS: Hide Banner");
			
		}
	}

	private void ReadSettings(AdsSettings settings)
	{
		if (_perInterstitialRules == null)
		{
			_perInterstitialRules = new Dictionary<string, PerInterstitialIntervals>();
		}
		else
		{
			_perInterstitialRules.Clear();
		}
		if (settings.overrideInterstitialIntervalSettings)
		{
			_intervalBetweenInterstitialsCurrentDefault = settings.intervalBetweenInterstitials;
			_secondsToWaitToShowInterstitialOnFirstLaunchCurrentDefault = settings.secondsToWaitToShowInterstitialOnFirstLaunch;
			_intervalsRulesGlobal = settings.intervalsRulesGlobal;
			for (int i = 0; i < settings.perInterstitialRules.Length; i++)
			{
				_perInterstitialRules.Add(settings.perInterstitialRules[i].name, settings.perInterstitialRules[i]);
			}
		}
		PerInterstitialIntervals perInterstitialIntervals = new PerInterstitialIntervals();
		perInterstitialIntervals.name = string.Empty;
		perInterstitialIntervals.secondsToWaitToShowInterstitialOnFirstLaunch = _secondsToWaitToShowInterstitialOnFirstLaunchCurrentDefault;
		perInterstitialIntervals.settings = _intervalsRulesGlobal;
		_perInterstitialRules.Add(string.Empty, perInterstitialIntervals);
	}

	public void ShowInterstitial()
	{
		ShowInterstitial(delegate
		{
			Debug.Log("#ADS: Interstitial Shown");
		}, string.Empty);
	}

	public void ShowInterstitial(string identifier = "")
	{
		ShowInterstitial(delegate
		{
			Debug.Log("#ADS: Interstitial Shown");
		}, identifier);
	}

	public void ShowInterstitial(Action onAdClosed, string identifier = "")
	{
		PerInterstitialIntervals intervalSettings = GetIntervalSettings(identifier);
		IntervalsSettings intervalSettingsForTIme = GetIntervalSettingsForTIme(intervalSettings.settings, CurrentTime() - FirstLaunchDate);
		Debug.Log("#ADS: Show interstitial " + identifier + " on time l1:" + (CurrentTime() - FirstLaunchDate) + " Lin:" + (CurrentTime() - LastInterstitialShowTime) + " >> aft:" + intervalSettingsForTIme.afterMinutes + " intrv:" + intervalSettingsForTIme.intervalBetweenInterstitials + " fl:" + intervalSettings.secondsToWaitToShowInterstitialOnFirstLaunch + " ALLOW:" + (intervalSettings.secondsToWaitToShowInterstitialOnFirstLaunch >= 0 && intervalSettingsForTIme.intervalBetweenInterstitials >= 0).ToString());
		bool flag = AdsEnabled && CurrentTime() - LastInterstitialShowTime > intervalSettingsForTIme.intervalBetweenInterstitials && CurrentTime() - FirstLaunchDate > intervalSettings.secondsToWaitToShowInterstitialOnFirstLaunch && intervalSettings.secondsToWaitToShowInterstitialOnFirstLaunch >= 0 && intervalSettingsForTIme.intervalBetweenInterstitials >= 0;
		if (!initialized)
		{
			return;
		}
		if (flag)
		{
			if (LoadingScreenPopup.instance != null)
			{
				LoadingScreenPopup.instance.Show();
			}
			
		}
		else
		{
			Debug.LogWarning("#ADS: Calling interstitial but ads are not enabled");
			onAdClosed();
		}
	}

	private IEnumerator WaitInterstitialAndShow(Action onAdClosed, string identifier = "")
	{
		float timer = 0f;
		Debug.Log("#ADS: WAIT INTERSTITIAL...");
		return null;
	}

	public bool IsShowingInterstitial()
	{
		
		return false;
	}

	public void ShowRewardedVideo(Action<bool> onRewardVideoWatched)
	{
		if (initialized)
		{
			
		}
	}

	public void RequestAndSetAdvertisingID()
	{
		Application.RequestAdvertisingIdentifierAsync(delegate(string advertisingId, bool trackingEnabled, string error)
		{
			this.advertisingId = advertisingId;
			Debug.Log("#ADS: advertisingId " + advertisingId + " " + trackingEnabled + " " + error);
		});
	}

	public bool IsShowingRewardedVideo()
	{
		
		return false;
	}

	public bool RewardedVideoLoaded()
	{
		
		return true;
	}

	public int CurrentTime()
	{
		DateTime dateTime = new DateTime(2017, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return (int)(DateTime.UtcNow - dateTime).TotalSeconds;
	}

	public void DisableAds()
	{
		AdsEnabled = false;
		HideBanner();
	}

	public void Reset()
	{
		
	}

	public void ActionDelayed(Action a, float delay)
	{
		StartCoroutine(ActionDelayedCo(a, delay));
	}

	private IEnumerator ActionDelayedCo(Action a, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (a != null)
		{
			a();
		}
	}
}
