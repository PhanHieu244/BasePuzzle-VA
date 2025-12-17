using System;
using UnityEngine.Events;

namespace MirkoAquaro.Assets.Subscriptions
{
	[Serializable]
	public class RewardEvent : UnityEvent<SubscriptionItem>
	{
	}
}
