using System;
using System.Collections;
using System.Collections.Generic;
//using Facebook.Unity;
using UnityEngine;

public class FacebookEventLogger : MonoBehaviour
{
	private static FacebookEventLogger _instance;

	public static FacebookEventLogger instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType<FacebookEventLogger>();
			}
			if (_instance == null)
			{
				_instance = new GameObject("FacebookEventLogger").AddComponent<FacebookEventLogger>();
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
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void LogFacebookEvent(string name, Dictionary<string, object> parameters)
	{

	}


}
