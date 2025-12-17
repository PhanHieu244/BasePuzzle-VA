using System.Collections.Generic;
using UnityEngine;

public class IosTenjin : BaseTenjin
{
	public override void Init(string apiKey)
	{
		Debug.Log("iOS Initializing " + apiKey);
		base.ApiKey = apiKey;
	}

	public override void Init(string apiKey, string sharedSecret)
	{
		Debug.Log("iOS Initializing with Shared Secret" + apiKey);
		base.ApiKey = apiKey;
		base.SharedSecret = sharedSecret;
	}

	public override void Connect()
	{
		Debug.Log("iOS Connecting " + base.ApiKey);
	}

	public override void Connect(string deferredDeeplink)
	{
		Debug.Log("Connecting with deferredDeeplink " + deferredDeeplink);
	}

	public override void SendEvent(string eventName)
	{
		Debug.Log("iOS Sending Event " + eventName);
	}

	public override void SendEvent(string eventName, string eventValue)
	{
		Debug.Log("iOS Sending Event " + eventName + " : " + eventValue);
	}

	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature)
	{
		Debug.Log("iOS Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
	}

	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate)
	{
		Debug.Log("Sending IosTenjin::GetDeeplink");
	}

	public override void OptIn()
	{
		Debug.Log("iOS OptIn");
	}

	public override void OptOut()
	{
		Debug.Log("iOS OptOut");
	}

	public override void OptInParams(List<string> parameters)
	{
		Debug.Log("iOS OptInParams");
	}

	public override void OptOutParams(List<string> parameters)
	{
		Debug.Log("iOS OptOutParams");
	}
}
