using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class SubscriptionsPopup : MonoBehaviour
{
	public static SubscriptionsPopup instance;

	private Canvas canvas
	{
		get
		{
			return GetComponentInChildren<Canvas>(true);
		}
	}

	private SubscriptionOption[] subscriptionsOptions
	{
		get
		{
			return GetComponentsInChildren<SubscriptionOption>(true);
		}
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void Show(IStoreController storeController, float delay)
	{
		StartCoroutine(ShowRoutine(storeController, delay));
	}

	public void Close()
	{
		canvas.gameObject.SetActive(false);
	}

	private SubscriptionInfo GetSubscriptionInfo(string subscription, IStoreController storeController)
	{
		if (storeController == null)
		{
			Debug.LogError("Subscription controller not initialized");
		}
		SubscriptionInfo result = null;
		Dictionary<string, string> dictionary = null;
		if (storeController != null)
		{
			Product[] all = storeController.products.all;
			foreach (Product product in all)
			{
				if (product.definition.id == subscription && product.receipt != null && product.definition.type == ProductType.Subscription)
				{
					string intro_json = ((dictionary != null && dictionary.ContainsKey(product.definition.storeSpecificId)) ? dictionary[product.definition.storeSpecificId] : null);
					SubscriptionManager subscriptionManager = new SubscriptionManager(product, intro_json);
					result = subscriptionManager.getSubscriptionInfo();
					break;
				}
			}
		}
		return result;
	}

	private IEnumerator ShowRoutine(IStoreController storeController, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (storeController == null)
		{
			Debug.Log("To show the subscription popup you cannot pass a null store controller");
			yield break;
		}
		bool error = false;
		SubscriptionOption[] array = subscriptionsOptions;
		foreach (SubscriptionOption subscriptionOption in array)
		{
			Product product = storeController.products.WithID(subscriptionOption.associatedProductID);
			if (product != null)
			{
				SubscriptionInfo subscriptionInfo = GetSubscriptionInfo(product.definition.id, storeController);
				subscriptionOption.SetDescriptionText(product.metadata.localizedDescription);
				subscriptionOption.SetPriceText(product.metadata.localizedPriceString);
				if (subscriptionInfo != null && subscriptionInfo.isSubscribed() == Result.True)
				{
					subscriptionOption.SetDescriptionText("Already Subscribed");
				}
			}
			else
			{
				Debug.Log("Cannot find product with id " + subscriptionOption.associatedProductID + " to initialize subscription popup button");
				error = true;
			}
		}
		if (!error)
		{
			canvas.gameObject.SetActive(true);
		}
	}
}
