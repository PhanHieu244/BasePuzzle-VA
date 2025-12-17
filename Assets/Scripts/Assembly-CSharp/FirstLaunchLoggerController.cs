using System;
using System.Collections.Generic;
using UnityEngine;

public class FirstLaunchLoggerController : MonoBehaviour
{
	[SerializeField]
	private string _keyToCheck = string.Empty;

	private const string FIRST_LAUNCH_CHECK_KEY = "isLaunched";

	private static FirstLaunchLoggerController _instance;

	private bool isFirstLaunch;

	public static FirstLaunchLoggerController instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType<FirstLaunchLoggerController>();
			}
			if (_instance == null)
			{
				_instance = new GameObject().AddComponent<FirstLaunchLoggerController>();
			}
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			isFirstLaunch = CheckFirstLaunch();
			Debug.Log("### Check1stLaunch: " + isFirstLaunch);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		if (isFirstLaunch)
		{
			LogFirstLaunch();
		}
		Debug.Log(DateTime.UtcNow.Date);
		SetLaunch();
		Dispose();
	}

	private bool CheckFirstLaunch()
	{
		if (!string.IsNullOrEmpty(_keyToCheck) && CPlayerPrefs.HasKey(_keyToCheck))
		{
			return false;
		}
		if (CPlayerPrefs.HasKey("isLaunched"))
		{
			return false;
		}
		return true;
	}

	private void SetLaunch()
	{
		CPlayerPrefs.SetInt("isLaunched", 1);
	}

	private void LogFirstLaunch()
	{
		int unixTime = GetUnixTime(DateTime.UtcNow.Date);
		Debug.Log("### Check1stLaunch: LogEvent with time = " + unixTime);
		FacebookEventLogger.instance.LogFacebookEvent("first_app_launch_custom", new Dictionary<string, object> { { "date_time", unixTime } });
	}

	private int GetUnixTime(DateTime dt)
	{
		return (int)dt.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
	}

	private void Dispose()
	{
		_instance = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
