using System;

[Serializable]
public class PerInterstitialIntervals
{
	public string name;

	public int secondsToWaitToShowInterstitialOnFirstLaunch;

	public IntervalsSettings[] settings;
}
