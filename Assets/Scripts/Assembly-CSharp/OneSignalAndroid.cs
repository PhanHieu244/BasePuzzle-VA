using System;
using System.Collections.Generic;
using OneSignalPush.MiniJSON;
using UnityEngine;

public class OneSignalAndroid : OneSignalPlatform
{
	private static AndroidJavaObject mOneSignal;

	public OneSignalAndroid(string gameObjectName, string googleProjectNumber, string appId, OneSignal.OSInFocusDisplayOption displayOption, OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel)
	{
		mOneSignal = new AndroidJavaObject("com.onesignal.OneSignalUnityProxy", gameObjectName, googleProjectNumber, appId, (int)logLevel, (int)visualLevel);
		SetInFocusDisplaying(displayOption);
	}

	public void SetLogLevel(OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel)
	{
		mOneSignal.Call("setLogLevel", (int)logLevel, (int)visualLevel);
	}

	public void SendTag(string tagName, string tagValue)
	{
		mOneSignal.Call("sendTag", tagName, tagValue);
	}

	public void SendTags(IDictionary<string, string> tags)
	{
		mOneSignal.Call("sendTags", Json.Serialize(tags));
	}

	public void GetTags()
	{
		mOneSignal.Call("getTags");
	}

	public void DeleteTag(string key)
	{
		mOneSignal.Call("deleteTag", key);
	}

	public void DeleteTags(IList<string> keys)
	{
		mOneSignal.Call("deleteTags", Json.Serialize(keys));
	}

	public void IdsAvailable()
	{
		mOneSignal.Call("idsAvailable");
	}

	public void RegisterForPushNotifications()
	{
	}

	public void promptForPushNotificationsWithUserResponse()
	{
	}

	public void EnableVibrate(bool enable)
	{
		mOneSignal.Call("enableVibrate", enable);
	}

	public void EnableSound(bool enable)
	{
		mOneSignal.Call("enableSound", enable);
	}

	public void SetInFocusDisplaying(OneSignal.OSInFocusDisplayOption display)
	{
		mOneSignal.Call("setInFocusDisplaying", (int)display);
	}

	public void SetSubscription(bool enable)
	{
		mOneSignal.Call("setSubscription", enable);
	}

	public void PostNotification(Dictionary<string, object> data)
	{
		mOneSignal.Call("postNotification", Json.Serialize(data));
	}

	public void SyncHashedEmail(string email)
	{
		mOneSignal.Call("syncHashedEmail", email);
	}

	public void PromptLocation()
	{
		mOneSignal.Call("promptLocation");
	}

	public void ClearOneSignalNotifications()
	{
		mOneSignal.Call("clearOneSignalNotifications");
	}

	public void addPermissionObserver()
	{
		mOneSignal.Call("addPermissionObserver");
	}

	public void removePermissionObserver()
	{
		mOneSignal.Call("removePermissionObserver");
	}

	public void addSubscriptionObserver()
	{
		mOneSignal.Call("addSubscriptionObserver");
	}

	public void removeSubscriptionObserver()
	{
		mOneSignal.Call("removeSubscriptionObserver");
	}

	public void addEmailSubscriptionObserver()
	{
		mOneSignal.Call("addEmailSubscriptionObserver");
	}

	public void removeEmailSubscriptionObserver()
	{
		mOneSignal.Call("removeEmailSubscriptionObserver");
	}

	public OSPermissionSubscriptionState getPermissionSubscriptionState()
	{
		return OneSignalPlatformHelper.parsePermissionSubscriptionState(this, mOneSignal.Call<string>("getPermissionSubscriptionState", new object[0]));
	}

	public OSPermissionStateChanges parseOSPermissionStateChanges(string jsonStat)
	{
		return OneSignalPlatformHelper.parseOSPermissionStateChanges(this, jsonStat);
	}

	public OSSubscriptionStateChanges parseOSSubscriptionStateChanges(string jsonStat)
	{
		return OneSignalPlatformHelper.parseOSSubscriptionStateChanges(this, jsonStat);
	}

	public OSEmailSubscriptionStateChanges parseOSEmailSubscriptionStateChanges(string jsonState)
	{
		return OneSignalPlatformHelper.parseOSEmailSubscriptionStateChanges(this, jsonState);
	}

	public OSPermissionState parseOSPermissionState(object stateDict)
	{
		Dictionary<string, object> dictionary = stateDict as Dictionary<string, object>;
		OSPermissionState oSPermissionState = new OSPermissionState();
		oSPermissionState.hasPrompted = true;
		bool flag = Convert.ToBoolean(dictionary["enabled"]);
		oSPermissionState.status = ((!flag) ? OSNotificationPermission.Denied : OSNotificationPermission.Authorized);
		return oSPermissionState;
	}

	public OSSubscriptionState parseOSSubscriptionState(object stateDict)
	{
		Dictionary<string, object> dictionary = stateDict as Dictionary<string, object>;
		OSSubscriptionState oSSubscriptionState = new OSSubscriptionState();
		oSSubscriptionState.subscribed = Convert.ToBoolean(dictionary["subscribed"]);
		oSSubscriptionState.userSubscriptionSetting = Convert.ToBoolean(dictionary["userSubscriptionSetting"]);
		oSSubscriptionState.userId = dictionary["userId"] as string;
		oSSubscriptionState.pushToken = dictionary["pushToken"] as string;
		return oSSubscriptionState;
	}

	public OSEmailSubscriptionState parseOSEmailSubscriptionState(object stateDict)
	{
		Dictionary<string, object> dictionary = stateDict as Dictionary<string, object>;
		OSEmailSubscriptionState oSEmailSubscriptionState = new OSEmailSubscriptionState();
		oSEmailSubscriptionState.subscribed = Convert.ToBoolean(dictionary["subscribed"]);
		oSEmailSubscriptionState.emailUserId = dictionary["emailUserId"] as string;
		oSEmailSubscriptionState.emailAddress = dictionary["emailAddress"] as string;
		return oSEmailSubscriptionState;
	}

	public void SetEmail(string email, string emailAuthCode)
	{
		mOneSignal.Call("setEmail", email, emailAuthCode);
	}

	public void SetEmail(string email)
	{
		mOneSignal.Call("setEmail", email, null);
	}

	public void LogoutEmail()
	{
		mOneSignal.Call("logoutEmail");
	}
}
