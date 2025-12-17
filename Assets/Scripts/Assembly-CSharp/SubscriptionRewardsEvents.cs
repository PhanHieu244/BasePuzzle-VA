using UnityEngine;

public class SubscriptionRewardsEvents : MonoBehaviour
{
	public void AddHints(int number)
	{
		GameState.hint.ChangeValue(number);
		Toast.instance.ShowMessage("You got " + number + " hints because you are subscribed!");
		SubscriptionsPopup.instance.Close();
	}
}
