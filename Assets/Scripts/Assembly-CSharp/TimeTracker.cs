using System;
using System.Collections.Generic;
//using Facebook.Unity;
using UnityEngine;

public class TimeTracker : SingletonMono<TimeTracker>
{
	private DateTime _timeStart;

	private bool isCounting;

	public void StartTimer()
	{
		_timeStart = DateTime.UtcNow;
		isCounting = true;
	}

	public TimeSpan StopTimer()
	{
		if (!isCounting)
		{
			return new TimeSpan(0L);
		}
		TimeSpan result = DateTime.UtcNow - _timeStart;
		isCounting = false;
		return result;
	}

	public double StopTimer_Seconds()
	{
		return StopTimer().TotalSeconds;
	}

	public float StopTimer_SecondsFloat()
	{
		float num = 0f;
		double num2 = StopTimer_Seconds();
		if (num2 <= 3.4028234663852886E+38)
		{
			return (float)num2;
		}
		return float.MaxValue;
	}

	public void StopTimerAndLogLevel(int world, int level)
	{
		float time = StopTimer_SecondsFloat();
		LogEvent(time, world, level);
	}

	public void LogEvent(float time, int world, int level)
	{
		if (time < 1E-05f)
		{
			Debug.Log("Nothing to log!");
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("world", world);
		dictionary.Add("level", level);
		Debug.Log("### Log: AchievedLevel   time:" + time.ToString() + "  level: " + level);
		///FB.LogAppEvent("AchievedLevel", time, dictionary);
	}
}
