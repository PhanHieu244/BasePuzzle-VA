using System;
using System.Collections;
using UnityEngine;

public static class ServerTime
{
	private const string TIME_SERVICE_URI = "https://script.google.com/macros/s/AKfycbzgpLhDNA122Id6AHOXbQ8M3leKVUhegA161MFLuTuGMPQq6LAM/exec";

	private static float _delayAllowedSeconds = 30f;

	private static bool _isTimeSynced;

	private static TimeSpan _timeDifference;

	private static bool _isTimeSyncing;

	public static bool isTimeLegal
	{
		get
		{
			return Mathf.Abs((float)_timeDifference.TotalMilliseconds / 1000f) < _delayAllowedSeconds;
		}
	}

	public static event Action<bool> onServerDateUpdated;

	public static void UpdateServerTime()
	{
		if (!_isTimeSynced && !_isTimeSyncing)
		{
			AdNetworksManager.instance.StartCoroutine(GetServerTime());
		}
		else if (ServerTime.onServerDateUpdated != null)
		{
			ServerTime.onServerDateUpdated(isTimeLegal);
		}
	}

	private static IEnumerator GetServerTime()
	{
		_isTimeSyncing = true;
		WWW www = new WWW("https://script.google.com/macros/s/AKfycbzgpLhDNA122Id6AHOXbQ8M3leKVUhegA161MFLuTuGMPQq6LAM/exec");
		float startTime = Time.unscaledTime;
		yield return www;
		float delay = Time.unscaledTime - startTime;
		DateTime result;
		if (www.error != null)
		{
			Debug.Log("Error: " + www.error);
			if (ServerTime.onServerDateUpdated != null)
			{
				ServerTime.onServerDateUpdated(false);
			}
		}
		else if (DateTime.TryParse(www.text, out result))
		{
			Debug.Log("UTC Time is " + result.ToString());
			_timeDifference = DateTime.UtcNow - result - new TimeSpan(0, 0, 0, 0, Mathf.RoundToInt(delay * 1000f));
			Debug.Log("Difference: " + _timeDifference.TotalSeconds + " seconds");
			if (ServerTime.onServerDateUpdated != null)
			{
				ServerTime.onServerDateUpdated(isTimeLegal);
			}
		}
		else
		{
			Debug.Log("Error: Can't parse time (" + www.text + ")");
			if (ServerTime.onServerDateUpdated != null)
			{
				ServerTime.onServerDateUpdated(false);
			}
		}
		_isTimeSyncing = false;
	}
}
