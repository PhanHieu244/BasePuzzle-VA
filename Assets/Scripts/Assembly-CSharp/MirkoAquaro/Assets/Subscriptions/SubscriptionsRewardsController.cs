using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Purchasing;

namespace MirkoAquaro.Assets.Subscriptions
{
	public class SubscriptionsRewardsController : MonoBehaviour
	{
		public static SubscriptionsRewardsController instance;

		public SubscriptionItem[] subscriptionItems;

		private IStoreController storeController;

		private IExtensionProvider storeExtensionProvider;

		[HideInInspector]
		public bool hasToCheckForSubscriptionRewards = true;

		private string serializationPath
		{
			get
			{
				return Application.persistentDataPath + "/" + GetType().Name;
			}
		}

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void Start()
		{
			StartCoroutine(RewardCheck());
		}

		public void Initialize(IStoreController storeController, IExtensionProvider extensionProvider)
		{
			if (storeController == null)
			{
				Debug.LogError("The subscription rewards controller has been initialized with a null store controller, please wait that the storeController is not null before initializing");
			}
			this.storeController = storeController;
			storeExtensionProvider = extensionProvider;
			try
			{
				LoadDataFromFile();
			}
			catch
			{
			}
			SubscriptionItem[] array = subscriptionItems;
			foreach (SubscriptionItem subscriptionItem in array)
			{
				if (!IsSubscriptionActive(subscriptionItem))
				{
					subscriptionItem.lastConsumeDate = DateTime.MinValue;
				}
			}
			SaveDataToFile();
		}

		private bool IsSubscriptionActive(SubscriptionItem subscription)
		{
			if (storeController == null)
			{
				Debug.LogError("Subscription controller not initialized");
			}
			if (subscription.debugSubscribed)
			{
				return true;
			}
			SubscriptionInfo subscriptionInfo = GetSubscriptionInfo(subscription);
			if (subscriptionInfo != null)
			{
				if (subscriptionInfo.isSubscribed() == Result.True)
				{
					return true;
				}
				subscription.lastConsumeDate = DateTime.MinValue;
				return false;
			}
			return false;
		}

		private SubscriptionInfo GetSubscriptionInfo(SubscriptionItem subscriptionItem)
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
					if (product.definition.id == subscriptionItem.bundleID && product.receipt != null && product.definition.type == ProductType.Subscription)
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

		private void ConsumeSubscription(string subscriptionID)
		{
			SubscriptionItem subscriptionItemByID = GetSubscriptionItemByID(subscriptionID);
			if (storeController == null)
			{
				Debug.LogError("Subscription controller not initialized");
			}
			int num = subscriptionItemByID.GetRenewalsSinceLastTime();
			if (subscriptionItemByID.firstTimeActivation)
			{
				num = 1;
				subscriptionItemByID.lastConsumeDate = DateTime.UtcNow;
			}
			else
			{
				subscriptionItemByID.lastConsumeDate = subscriptionItemByID.lastConsumeDate.AddSeconds(num * subscriptionItemByID.secondsToWaitForNextReward);
			}
			for (int i = 0; i < num; i++)
			{
				subscriptionItemByID.onSubscriptionReward.Invoke(subscriptionItemByID);
			}
			SaveDataToFile();
		}

		public SubscriptionItem GetSubscriptionItemByID(string id)
		{
			SubscriptionItem result = null;
			SubscriptionItem[] array = subscriptionItems;
			foreach (SubscriptionItem subscriptionItem in array)
			{
				if (subscriptionItem.bundleID == id)
				{
					result = subscriptionItem;
					break;
				}
			}
			return result;
		}

		private void SaveDataToFile()
		{
			SubscriptionControllerData obj = Serialize();
			string contents = JsonUtility.ToJson(obj);
			File.WriteAllText(serializationPath, contents);
		}

		private void LoadDataFromFile()
		{
			string json = File.ReadAllText(serializationPath);
			SubscriptionControllerData subscriptionControllerData = JsonUtility.FromJson<SubscriptionControllerData>(json);
			if (subscriptionControllerData != null)
			{
				Deserialize(subscriptionControllerData);
			}
		}

		private SubscriptionControllerData Serialize()
		{
			SubscriptionControllerData subscriptionControllerData = new SubscriptionControllerData();
			subscriptionControllerData.subscriptionItemData = new SubscriptionItemData[subscriptionItems.Length];
			for (int i = 0; i < subscriptionItems.Length; i++)
			{
				subscriptionControllerData.subscriptionItemData[i] = subscriptionItems[i].Serialize();
			}
			return subscriptionControllerData;
		}

		private void Deserialize(SubscriptionControllerData data)
		{
			if (data.subscriptionItemData == null)
			{
				return;
			}
			SubscriptionItemData[] subscriptionItemData = data.subscriptionItemData;
			foreach (SubscriptionItemData subscriptionItemData2 in subscriptionItemData)
			{
				SubscriptionItem[] array = subscriptionItems;
				foreach (SubscriptionItem subscriptionItem in array)
				{
					if (subscriptionItem.bundleID == subscriptionItemData2.bundleID)
					{
						subscriptionItem.Deserialize(subscriptionItemData2);
					}
				}
			}
		}

		private IEnumerator RewardCheck()
		{
			while (true)
			{
				if (hasToCheckForSubscriptionRewards && storeController != null)
				{
					SubscriptionItem[] array = instance.subscriptionItems;
					foreach (SubscriptionItem subscriptionItem in array)
					{
						if (instance.IsSubscriptionActive(subscriptionItem))
						{
							Debug.Log("subscriptionTimePassed = " + (DateTime.Now - instance.subscriptionItems[0].lastConsumeDate).Seconds);
							if (subscriptionItem.GetRenewalsSinceLastTime() > 0 || subscriptionItem.firstTimeActivation)
							{
								instance.ConsumeSubscription(subscriptionItem.bundleID);
							}
						}
					}
					yield return new WaitForSeconds(1f);
				}
				else
				{
					yield return new WaitForEndOfFrame();
				}
			}
		}
	}
}
