using System;
using UnityEngine;

namespace MirkoAquaro.Assets.Subscriptions
{
	[Serializable]
	public class SubscriptionItem
	{
		public string bundleID;

		public DateTime lastConsumeDate;

		public int secondsToWaitForNextReward;

		public RewardEvent onSubscriptionReward;

		public bool debugSubscribed;

		public bool firstTimeActivation
		{
			get
			{
				return lastConsumeDate == DateTime.MinValue;
			}
		}

		public int GetRenewalsSinceLastTime()
		{
			return (DateTime.UtcNow - lastConsumeDate).Seconds / secondsToWaitForNextReward;
		}

		public SubscriptionItemData Serialize()
		{
			SubscriptionItemData subscriptionItemData = new SubscriptionItemData();
			subscriptionItemData.bundleID = bundleID;
			try
			{
				subscriptionItemData.lastConsumeDate = lastConsumeDate.ToFileTimeUtc();
			}
			catch
			{
				Debug.LogWarning("invalid filetime for serialization");
			}
			return subscriptionItemData;
		}

		public void Deserialize(SubscriptionItemData data)
		{
			bundleID = data.bundleID;
			lastConsumeDate = DateTime.FromFileTimeUtc(data.lastConsumeDate);
		}
	}
}
