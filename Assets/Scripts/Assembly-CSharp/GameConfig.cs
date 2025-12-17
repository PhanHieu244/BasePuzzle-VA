using System;
using UnityEngine;

[Serializable]
public class GameConfig
{
	[Header("")]
	public int interstitialAdPeriod;

	public int rewardedVideoPeriod;

	public int rewardedVideoAmount;

	public string androidPackageID;

	public string iosAppID;

	public string facebookPageID;
}
