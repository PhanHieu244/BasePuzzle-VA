using System;
using System.Collections.Generic;
using UnityEngine;

public class MaxSdkCallbacks : MonoBehaviour
{
	public static MaxSdkCallbacks Instance { get; private set; }

	public static event Action<MaxSdkBase.SdkConfiguration> OnSdkInitializedEvent;

	public static event Action<string> OnBannerAdLoadedEvent;

	public static event Action<string, int> OnBannerAdLoadFailedEvent;

	public static event Action<string> OnBannerAdClickedEvent;

	public static event Action<string> OnBannerAdExpandedEvent;

	public static event Action<string> OnBannerAdCollapsedEvent;

	public static event Action<string> OnInterstitialLoadedEvent;

	public static event Action<string, int> OnInterstitialLoadFailedEvent;

	public static event Action<string> OnInterstitialHiddenEvent;

	public static event Action<string> OnInterstitialDisplayedEvent;

	public static event Action<string, int> OnInterstitialAdFailedToDisplayEvent;

	public static event Action<string> OnInterstitialClickedEvent;

	public static event Action<string> OnRewardedAdLoadedEvent;

	public static event Action<string, int> OnRewardedAdLoadFailedEvent;

	public static event Action<string> OnRewardedAdDisplayedEvent;

	public static event Action<string> OnRewardedAdHiddenEvent;

	public static event Action<string> OnRewardedAdClickedEvent;

	public static event Action<string, int> OnRewardedAdFailedToDisplayEvent;

	public static event Action<string, MaxSdkBase.Reward> OnRewardedAdReceivedRewardEvent;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	public void ForwardEvent(string eventPropsStr)
	{
		Dictionary<string, string> dictionary = PropsStringToDict(eventPropsStr);
		string text = dictionary["name"];
		if (text == "OnSdkInitializedEvent")
		{
			string value = dictionary["consentDialogState"];
			MaxSdkBase.SdkConfiguration sdkConfiguration = new MaxSdkBase.SdkConfiguration();
			if ("1".Equals(value))
			{
				sdkConfiguration.ConsentDialogState = MaxSdkBase.ConsentDialogState.Applies;
			}
			else if ("2".Equals(value))
			{
				sdkConfiguration.ConsentDialogState = MaxSdkBase.ConsentDialogState.DoesNotApply;
			}
			else
			{
				sdkConfiguration.ConsentDialogState = MaxSdkBase.ConsentDialogState.Unknown;
			}
			InvokeEvent(MaxSdkCallbacks.OnSdkInitializedEvent, sdkConfiguration);
			return;
		}
		string text2 = dictionary["adUnitId"];
		switch (text)
		{
		case "OnBannerAdLoadedEvent":
			InvokeEvent(MaxSdkCallbacks.OnBannerAdLoadedEvent, text2);
			break;
		case "OnBannerAdLoadFailedEvent":
		{
			int result5 = 0;
			int.TryParse(dictionary["errorCode"], out result5);
			InvokeEvent(MaxSdkCallbacks.OnBannerAdLoadFailedEvent, text2, result5);
			break;
		}
		case "OnBannerAdClickedEvent":
			InvokeEvent(MaxSdkCallbacks.OnBannerAdClickedEvent, text2);
			break;
		case "OnBannerAdExpandedEvent":
			InvokeEvent(MaxSdkCallbacks.OnBannerAdExpandedEvent, text2);
			break;
		case "OnBannerAdCollapsedEvent":
			InvokeEvent(MaxSdkCallbacks.OnBannerAdCollapsedEvent, text2);
			break;
		case "OnInterstitialLoadedEvent":
			InvokeEvent(MaxSdkCallbacks.OnInterstitialLoadedEvent, text2);
			break;
		case "OnInterstitialLoadFailedEvent":
		{
			int result4 = 0;
			int.TryParse(dictionary["errorCode"], out result4);
			InvokeEvent(MaxSdkCallbacks.OnInterstitialLoadFailedEvent, text2, result4);
			break;
		}
		case "OnInterstitialHiddenEvent":
			InvokeEvent(MaxSdkCallbacks.OnInterstitialHiddenEvent, text2);
			break;
		case "OnInterstitialDisplayedEvent":
			InvokeEvent(MaxSdkCallbacks.OnInterstitialDisplayedEvent, text2);
			break;
		case "OnInterstitialAdFailedToDisplayEvent":
		{
			int result3 = 0;
			int.TryParse(dictionary["errorCode"], out result3);
			InvokeEvent(MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent, text2, result3);
			break;
		}
		case "OnInterstitialClickedEvent":
			InvokeEvent(MaxSdkCallbacks.OnInterstitialClickedEvent, text2);
			break;
		case "OnRewardedAdLoadedEvent":
			InvokeEvent(MaxSdkCallbacks.OnRewardedAdLoadedEvent, text2);
			break;
		case "OnRewardedAdLoadFailedEvent":
		{
			int result2 = 0;
			int.TryParse(dictionary["errorCode"], out result2);
			InvokeEvent(MaxSdkCallbacks.OnRewardedAdLoadFailedEvent, text2, result2);
			break;
		}
		case "OnRewardedAdDisplayedEvent":
			InvokeEvent(MaxSdkCallbacks.OnRewardedAdDisplayedEvent, text2);
			break;
		case "OnRewardedAdHiddenEvent":
			InvokeEvent(MaxSdkCallbacks.OnRewardedAdHiddenEvent, text2);
			break;
		case "OnRewardedAdClickedEvent":
			InvokeEvent(MaxSdkCallbacks.OnRewardedAdClickedEvent, text2);
			break;
		case "OnRewardedAdFailedToDisplayEvent":
		{
			int result = 0;
			int.TryParse(dictionary["errorCode"], out result);
			InvokeEvent(MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent, text2, result);
			break;
		}
		case "OnRewardedAdReceivedRewardEvent":
		{
			MaxSdkBase.Reward reward = default(MaxSdkBase.Reward);
			reward.Label = dictionary["rewardLabel"];
			MaxSdkBase.Reward param = reward;
			int.TryParse(dictionary["rewardAmount"], out param.Amount);
			InvokeEvent(MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent, text2, param);
			break;
		}
		default:
			Debug.LogWarning("Unknown MAX Ads event fired: " + text);
			break;
		}
	}

	private static void InvokeEvent<T>(Action<T> evt, T param)
	{
		if (evt.GetInvocationList().Length > 5)
		{
			Debug.LogWarning(string.Concat("MAX Ads Event (", evt, ") has over 5 subscribers. Please make sure you are properly un-subscribing to actions!!!"));
		}
		Debug.Log("Invoking event: " + evt);
		evt(param);
	}

	private static void InvokeEvent<T1, T2>(Action<T1, T2> evt, T1 param1, T2 param2)
	{
		if (evt != null)
		{
			if (evt.GetInvocationList().Length > 5)
			{
				Debug.LogWarning(string.Concat("MAX Ads Event (", evt, ") has over 5 subscribers. Please make sure you are properly un-subscribing to actions!!!"));
			}
			Debug.Log("Invoking event: " + evt);
			evt(param1, param2);
		}
	}

	private static Dictionary<string, string> PropsStringToDict(string str)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (string.IsNullOrEmpty(str))
		{
			return dictionary;
		}
		string[] array = str.Split('\n');
		string[] array2 = array;
		foreach (string text in array2)
		{
			int num = text.IndexOf('=');
			if (num > 0 && num < text.Length)
			{
				string key = text.Substring(0, num);
				string value = text.Substring(num + 1, text.Length - num - 1);
				if (!dictionary.ContainsKey(key))
				{
					dictionary[key] = value;
				}
			}
		}
		return dictionary;
	}
}
