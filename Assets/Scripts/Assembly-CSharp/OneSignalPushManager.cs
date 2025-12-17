using UnityEngine;

public class OneSignalPushManager : MonoBehaviour
{
	public static OneSignalPushManager instance;

	public string appID;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		OneSignal.StartInit(appID).HandleNotificationOpened(HandleNotificationOpened).EndInit();
		OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;
	}

	private static void HandleNotificationOpened(OSNotificationOpenedResult result)
	{
	}
}
