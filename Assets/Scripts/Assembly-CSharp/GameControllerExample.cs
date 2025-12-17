using System;
using System.Collections.Generic;
using OneSignalPush.MiniJSON;
using UnityEngine;

public class GameControllerExample : MonoBehaviour
{
	private static string extraMessage;

	public string email = "Email Address";

	private void Start()
	{
		extraMessage = null;
		OneSignal.SetLogLevel(OneSignal.LOG_LEVEL.VERBOSE, OneSignal.LOG_LEVEL.NONE);
		OneSignal.StartInit("78e8aff3-7ce2-401f-9da0-2d41f287ebaf").HandleNotificationReceived(HandleNotificationReceived).HandleNotificationOpened(HandleNotificationOpened)
			.EndInit();
		OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;
		OneSignal.permissionObserver += OneSignal_permissionObserver;
		OneSignal.subscriptionObserver += OneSignal_subscriptionObserver;
		OneSignal.emailSubscriptionObserver += OneSignal_emailSubscriptionObserver;
		OSPermissionSubscriptionState permissionSubscriptionState = OneSignal.GetPermissionSubscriptionState();
		Debug.Log("pushState.subscriptionStatus.subscribed : " + permissionSubscriptionState.subscriptionStatus.subscribed);
		Debug.Log("pushState.subscriptionStatus.userId : " + permissionSubscriptionState.subscriptionStatus.userId);
	}

	private void OneSignal_subscriptionObserver(OSSubscriptionStateChanges stateChanges)
	{
		Debug.Log("SUBSCRIPTION stateChanges: " + stateChanges);
		Debug.Log("SUBSCRIPTION stateChanges.to.userId: " + stateChanges.to.userId);
		Debug.Log("SUBSCRIPTION stateChanges.to.subscribed: " + stateChanges.to.subscribed);
	}

	private void OneSignal_permissionObserver(OSPermissionStateChanges stateChanges)
	{
		Debug.Log("PERMISSION stateChanges.from.status: " + stateChanges.from.status);
		Debug.Log("PERMISSION stateChanges.to.status: " + stateChanges.to.status);
	}

	private void OneSignal_emailSubscriptionObserver(OSEmailSubscriptionStateChanges stateChanges)
	{
		Debug.Log("EMAIL stateChanges.from.status: " + stateChanges.from.emailUserId + ", " + stateChanges.from.emailAddress);
		Debug.Log("EMAIL stateChanges.to.status: " + stateChanges.to.emailUserId + ", " + stateChanges.to.emailAddress);
	}

	private static void HandleNotificationReceived(OSNotification notification)
	{
		OSNotificationPayload payload = notification.payload;
		string body = payload.body;
		MonoBehaviour.print("GameControllerExample:HandleNotificationReceived: " + body);
		MonoBehaviour.print("displayType: " + notification.displayType);
		extraMessage = "Notification received with text: " + body;
		Dictionary<string, object> additionalData = payload.additionalData;
		if (additionalData == null)
		{
			Debug.Log("[HandleNotificationReceived] Additional Data == null");
		}
		else
		{
			Debug.Log("[HandleNotificationReceived] message " + body + ", additionalData: " + Json.Serialize(additionalData));
		}
	}

	public static void HandleNotificationOpened(OSNotificationOpenedResult result)
	{
		OSNotificationPayload payload = result.notification.payload;
		string body = payload.body;
		string actionID = result.action.actionID;
		MonoBehaviour.print("GameControllerExample:HandleNotificationOpened: " + body);
		extraMessage = "Notification opened with text: " + body;
		Dictionary<string, object> additionalData = payload.additionalData;
		if (additionalData == null)
		{
			Debug.Log("[HandleNotificationOpened] Additional Data == null");
		}
		else
		{
			Debug.Log("[HandleNotificationOpened] message " + body + ", additionalData: " + Json.Serialize(additionalData));
		}
		if (actionID != null)
		{
			extraMessage = "Pressed ButtonId: " + actionID;
		}
	}

	private void OnGUI()
	{
		GUIStyle gUIStyle = new GUIStyle("button");
		gUIStyle.fontSize = 30;
		GUIStyle gUIStyle2 = new GUIStyle("box");
		gUIStyle2.fontSize = 30;
		GUIStyle gUIStyle3 = new GUIStyle("textField");
		gUIStyle3.fontSize = 30;
		float x = 50f;
		float width = (float)Screen.width - 120f;
		float width2 = (float)Screen.width - 20f;
		float num = 120f;
		float num2 = 630f;
		float num3 = 200f;
		float num4 = 90f;
		float height = 60f;
		GUI.Box(new Rect(10f, num, width2, num2), "Test Menu", gUIStyle2);
		float num5 = 0f;
		if (GUI.Button(new Rect(x, num3 + num5 * num4, width, height), "SendTags", gUIStyle))
		{
			OneSignal.SendTag("UnityTestKey", "TestValue");
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("UnityTestKey2", "value2");
			dictionary.Add("UnityTestKey3", "value3");
			OneSignal.SendTags(dictionary);
		}
		num5 += 1f;
		if (GUI.Button(new Rect(x, num3 + num5 * num4, width, height), "GetIds", gUIStyle))
		{
			OneSignal.IdsAvailable(delegate(string userId, string pushToken)
			{
				extraMessage = "UserID:\n" + userId + "\n\nPushToken:\n" + pushToken;
			});
		}
		num5 += 1f;
		if (GUI.Button(new Rect(x, num3 + num5 * num4, width, height), "TestNotification", gUIStyle))
		{
			extraMessage = "Waiting to get a OneSignal userId. Uncomment OneSignal.SetLogLevel in the Start method if it hangs here to debug the issue.";
			OneSignal.IdsAvailable(delegate(string userId, string pushToken)
			{
				if (pushToken != null)
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					dictionary2["contents"] = new Dictionary<string, string> { { "en", "Test Message" } };
					dictionary2["include_player_ids"] = new List<string> { userId };
					dictionary2["send_after"] = DateTime.Now.ToUniversalTime().AddSeconds(30.0).ToString("U");
					extraMessage = "Posting test notification now.";
					OneSignal.PostNotification(dictionary2, delegate(Dictionary<string, object> responseSuccess)
					{
						extraMessage = "Notification posted successful! Delayed by about 30 secounds to give you time to press the home button to see a notification vs an in-app alert.\n" + Json.Serialize(responseSuccess);
					}, delegate(Dictionary<string, object> responseFailure)
					{
						extraMessage = "Notification failed to post:\n" + Json.Serialize(responseFailure);
					});
				}
				else
				{
					extraMessage = "ERROR: Device is not registered.";
				}
			});
		}
		num5 += 1f;
		email = GUI.TextField(new Rect(x, num3 + num5 * num4, width, height), email, gUIStyle);
		num5 += 1f;
		if (GUI.Button(new Rect(x, num3 + num5 * num4, width, height), "SetEmail", gUIStyle))
		{
			extraMessage = "Setting email to " + email;
			OneSignal.SetEmail(email, delegate
			{
				Debug.Log("Successfully set email");
			}, delegate(Dictionary<string, object> error)
			{
				Debug.Log("Encountered error setting email: " + Json.Serialize(error));
			});
		}
		num5 += 1f;
		if (GUI.Button(new Rect(x, num3 + num5 * num4, width, height), "LogoutEmail", gUIStyle))
		{
			extraMessage = "Logging Out of example@example.com";
			OneSignal.LogoutEmail(delegate
			{
				Debug.Log("Successfully logged out of email");
			}, delegate(Dictionary<string, object> error)
			{
				Debug.Log("Encountered error logging out of email: " + Json.Serialize(error));
			});
		}
		if (extraMessage != null)
		{
			gUIStyle2.alignment = TextAnchor.UpperLeft;
			gUIStyle2.wordWrap = true;
			GUI.Box(new Rect(10f, num + num2 + 20f, Screen.width - 20, (float)Screen.height - (num + num2 + 40f)), extraMessage, gUIStyle2);
		}
	}
}
