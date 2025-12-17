using UnityEngine;
using UnityEngine.Events;

public class GDPRConsent : MonoBehaviour
{
	public static GDPRConsent instance;

	public static GDPRConsent DefaultPlugin;

	public AndroidJavaClass sbaFacade = new AndroidJavaClass("com.sba.unitypluginsharing.GDPRConsent");

	public AndroidJavaObject currentActivity;

	public string publisherID;

	public UnityEvent onBeforeConsent;

	public UnityBoolEvent onAdsConsent;

	public UnityBoolEvent onAnalyticsConsent;

	public GDPRConsent(AndroidJavaObject activity)
	{
		if (activity == null)
		{
			throw new MissingReferenceException("No parent activity specified");
		}
		currentActivity = activity;
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			GameObject obj = base.gameObject;
			obj.name = obj.name + "_" + base.gameObject.GetInstanceID();
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		GetConsent(publisherID, base.gameObject.name);
	}

	private void submitUserConsentAnalytics(string value)
	{
		Debug.Log("GDPRConsent: submitUserConsentAnalytics called with value:" + value);
		if (value.Equals("true"))
		{
			onAnalyticsConsent.Invoke(true);
		}
		else
		{
			onAnalyticsConsent.Invoke(false);
		}
	}

	private void submitUserConsentAds(string value)
	{
		Debug.Log("GDPRConsent: submitUserConsentAds called with value:" + value);
		if (value.Equals("true"))
		{
			onAdsConsent.Invoke(true);
		}
		else
		{
			onAdsConsent.Invoke(false);
		}
	}

	public static void GetConsent(string publisherId, string gameObjectName)
	{
		Debug.Log("GDPRConsent: called for publisherId:" + publisherId);
		getDefaultPlugin().getConsent(publisherId, gameObjectName);
	}

	public static GDPRConsent getDefaultPlugin()
	{
		if (DefaultPlugin == null)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			DefaultPlugin = new GDPRConsent(androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"));
		}
		return DefaultPlugin;
	}

	public void getConsent(string publisherId, string gameObjectName)
	{
		Debug.Log("GDPRConsent: static getConsent called for plugin");
		Debug.Log("GDPR GAMEOBJECT NAME : = " + gameObjectName);
		sbaFacade.CallStatic("GetUserConsent", currentActivity, publisherId, gameObjectName);
	}
}
