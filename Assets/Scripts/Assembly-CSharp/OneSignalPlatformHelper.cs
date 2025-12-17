using System.Collections.Generic;
using OneSignalPush.MiniJSON;

internal class OneSignalPlatformHelper
{
	internal static OSPermissionSubscriptionState parsePermissionSubscriptionState(OneSignalPlatform platform, string jsonStr)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonStr) as Dictionary<string, object>;
		OSPermissionSubscriptionState oSPermissionSubscriptionState = new OSPermissionSubscriptionState();
		oSPermissionSubscriptionState.permissionStatus = platform.parseOSPermissionState(dictionary["permissionStatus"]);
		oSPermissionSubscriptionState.subscriptionStatus = platform.parseOSSubscriptionState(dictionary["subscriptionStatus"]);
		if (dictionary.ContainsKey("emailSubscriptionStatus"))
		{
			oSPermissionSubscriptionState.emailSubscriptionStatus = platform.parseOSEmailSubscriptionState(dictionary["emailSubscriptionStatus"]);
		}
		return oSPermissionSubscriptionState;
	}

	internal static OSPermissionStateChanges parseOSPermissionStateChanges(OneSignalPlatform platform, string stateChangesJSONString)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(stateChangesJSONString) as Dictionary<string, object>;
		OSPermissionStateChanges oSPermissionStateChanges = new OSPermissionStateChanges();
		oSPermissionStateChanges.to = platform.parseOSPermissionState(dictionary["to"]);
		oSPermissionStateChanges.from = platform.parseOSPermissionState(dictionary["from"]);
		return oSPermissionStateChanges;
	}

	internal static OSSubscriptionStateChanges parseOSSubscriptionStateChanges(OneSignalPlatform platform, string stateChangesJSONString)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(stateChangesJSONString) as Dictionary<string, object>;
		OSSubscriptionStateChanges oSSubscriptionStateChanges = new OSSubscriptionStateChanges();
		oSSubscriptionStateChanges.to = platform.parseOSSubscriptionState(dictionary["to"]);
		oSSubscriptionStateChanges.from = platform.parseOSSubscriptionState(dictionary["from"]);
		return oSSubscriptionStateChanges;
	}

	internal static OSEmailSubscriptionStateChanges parseOSEmailSubscriptionStateChanges(OneSignalPlatform platform, string stateChangesJSONString)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(stateChangesJSONString) as Dictionary<string, object>;
		OSEmailSubscriptionStateChanges oSEmailSubscriptionStateChanges = new OSEmailSubscriptionStateChanges();
		oSEmailSubscriptionStateChanges.to = platform.parseOSEmailSubscriptionState(dictionary["to"]);
		oSEmailSubscriptionStateChanges.from = platform.parseOSEmailSubscriptionState(dictionary["from"]);
		return oSEmailSubscriptionStateChanges;
	}
}
