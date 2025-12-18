using System;
using UnityEngine;

[Serializable]
public class AdsSettings
{
	[Space]
	[Header("=== Ad Units: ===")]
	public bool isUseCustomAdUnits = true;

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
