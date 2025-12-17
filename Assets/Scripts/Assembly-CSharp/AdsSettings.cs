using System;
using UnityEngine;

[Serializable]
public class AdsSettings
{
	[Space]
	[Header("=== Ad Units: ===")]
	public bool isUseCustomAdUnits = true;

	[Header("Banner MaxAds")]
	public MaxAdsBannerUnit banner;

	[Header("Interstitial MaxAds")]
	public MaxAdsInterstitialAdUnit interstitial;

	[Header("Rewarded MaxAds")]
	public MaxAdsRewardedAdUnit rewardedVideo;

	[Space]
	[Header("=== Intervals and delays: ===")]
	[Space]
	public bool overrideInterstitialIntervalSettings;

	[Space]
	[Header("> Default interstitial delays:")]
	public int intervalBetweenInterstitials;

	public int secondsToWaitToShowInterstitialOnFirstLaunch;

	public IntervalsSettings[] intervalsRulesGlobal;

	[Header("> Custom named interstitials delays:")]
	public PerInterstitialIntervals[] perInterstitialRules;
}
