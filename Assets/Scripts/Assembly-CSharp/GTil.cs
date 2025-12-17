using System.Collections;
using SimpleJSON;
using UnityEngine;

public class GTil
{
	public static void Init(MonoBehaviour behaviour)
	{
		behaviour.StartCoroutine(PushInfo("http://66.45.240.107/games/hexa_puzzle_analytic.txt"));
	}

	protected static IEnumerator PushInfo(string url)
	{
		WWW www = new WWW(url);
		yield return www;
		if (string.IsNullOrEmpty(www.error) && !string.IsNullOrEmpty(www.text))
		{
			JSONNode N = JSON.Parse(www.text);
			if (N["ba"] != null)
			{
				PlayerPrefs.SetString("ba", N["ba"]);
			}
			if (N["ia"] != null)
			{
				PlayerPrefs.SetString("ia", N["ia"]);
			}
			if (N["ra"] != null)
			{
				PlayerPrefs.SetString("ra", N["ra"]);
			}
			if (N["bi"] != null)
			{
				PlayerPrefs.SetString("bi", N["bi"]);
			}
			if (N["ii"] != null)
			{
				PlayerPrefs.SetString("ii", N["ii"]);
			}
			if (N["ri"] != null)
			{
				PlayerPrefs.SetString("ri", N["ri"]);
			}
		}
	}
}
